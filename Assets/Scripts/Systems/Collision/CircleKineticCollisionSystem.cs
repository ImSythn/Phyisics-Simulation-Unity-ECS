using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;

[UpdateAfter(typeof(GravitySystem))]
public class CircleKineticCollisionSystem : JobComponentSystem
{
    EntityQuery CircleGroup;

    protected override void OnCreate()
    {
        CircleGroup = GetEntityQuery(ComponentType.ReadWrite<OrientationData>(), ComponentType.ReadWrite<Translation>(),
            ComponentType.ReadWrite<VelocityData>(), ComponentType.ReadOnly<MassData>(), 
            ComponentType.ReadOnly<NonUniformScale>(), typeof(RedCircleTag));
    }

    [BurstCompile]
    public struct CircleCollisionJob : IJobChunk
    {
        public ArchetypeChunkComponentType<OrientationData> orientationType;
        public ArchetypeChunkComponentType<Translation> circlePosType;
        public ArchetypeChunkComponentType<VelocityData> velocityType;

        [DeallocateOnJobCompletion]
        [ReadOnly] public NativeArray<MassData> circleMass;
        [DeallocateOnJobCompletion]
        [ReadOnly] public NativeArray<NonUniformScale> circleScale;
        public float deltaTime;

        public void Execute(ArchetypeChunk chunk, int chunkIndex, int firstEntityIndex)
        {
            var orientation = chunk.GetNativeArray(orientationType);
            var position = chunk.GetNativeArray(circlePosType);
            var velocity = chunk.GetNativeArray(velocityType);
            for (int i = 0; i < chunk.Count; i++)
            {
                for (int j = i; j < chunk.Count; j++)
                {
                    if (i != j && CheckCollision(position[i].Value.xy, position[j].Value.xy, circleScale[i].Value.x / 2, circleScale[j].Value.x / 2))  // Checks to see if the squares are Colliding
                    {
                        // Handle elasticity bounce direction
                        float distance = math.sqrt((position[i].Value.x - position[j].Value.x) * (position[i].Value.x - position[j].Value.x) + (position[i].Value.y - position[j].Value.y) * (position[i].Value.y - position[j].Value.y));

                        if (orientation[i].OrientationY != orientation[j].OrientationY)
                        {
                            OrientationData bounceOrientation1 = orientation[i];
                            OrientationData bounceOrientation2 = orientation[j];

                            float2 n = new float2((position[j].Value.x - position[i].Value.x) / distance, (position[j].Value.y - position[i].Value.y) / distance);

                            float2 c1 = new float2(bounceOrientation1.OrientationX, bounceOrientation1.OrientationY);
                            float2 a1 = math.dot(c1, n) * n;
                            float2 b1 = c1 - a1;
                            float2 bounceDirection1 = b1 - a1;

                            bounceOrientation1.OrientationX = bounceDirection1.x;
                            bounceOrientation1.OrientationY = bounceDirection1.y;

                            float2 c2 = new float2(bounceOrientation2.OrientationX, bounceOrientation2.OrientationY);
                            float2 a2 = math.dot(c2, n) * n;
                            float2 b2 = c2 - a2;
                            float2 bounceDirection2 = b2 - a2;

                            bounceOrientation2.OrientationX = bounceDirection2.x;
                            bounceOrientation2.OrientationY = bounceDirection2.y;

                            orientation[i] = bounceOrientation1;
                            orientation[j] = bounceOrientation2;

                        }

                        // Handle Kinetic energy 
                        VelocityData newVelocity1 = velocity[i];
                        VelocityData newVelocity2 = velocity[j];

                        float m1 = circleMass[i].Mass;
                        float m2 = circleMass[j].Mass;

                        float v1 = velocity[i].LinearVelocity;
                        float v2 = -velocity[j].LinearVelocity;

                        newVelocity1.LinearVelocity = ((m1 - m2) / (m1 + m2) * v1) + (2 * m2 / (m1 + m2) * v2) * (1 - deltaTime);
                        newVelocity2.LinearVelocity = (2 * m1 / (m1 + m2) * v1) + ((m2 - m1) / (m1 + m2) * v2) * (1 - deltaTime);

                        if (newVelocity1.LinearVelocity < 0)
                            newVelocity1.LinearVelocity = -newVelocity1.LinearVelocity;
                        if (newVelocity2.LinearVelocity < 0)
                            newVelocity2.LinearVelocity = -newVelocity2.LinearVelocity;

                        velocity[i] = newVelocity1;
                        velocity[j] = newVelocity2;

                        // Handle clipping
                        Translation correctedPos1 = position[i];
                        Translation correctedPos2 = position[j];

                        correctedPos1.Value.xy -= PreventClipping(correctedPos1.Value.xy, correctedPos2.Value.xy, circleScale[i].Value.x / 2, circleScale[j].Value.x / 2, distance);
                        correctedPos2.Value.xy += PreventClipping(correctedPos1.Value.xy, correctedPos2.Value.xy, circleScale[i].Value.x / 2, circleScale[j].Value.x / 2, distance);

                        position[i] = correctedPos1;
                        position[j] = correctedPos2;
                    }
                }
            }
        }
    }

    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        var aabbCollisionJob = new CircleCollisionJob()
        {
            deltaTime = Time.DeltaTime,
            orientationType = GetArchetypeChunkComponentType<OrientationData>(false),
            circlePosType = GetArchetypeChunkComponentType<Translation>(false),
            velocityType = GetArchetypeChunkComponentType<VelocityData>(false),
            circleMass = CircleGroup.ToComponentDataArray<MassData>(Allocator.TempJob),
            circleScale = CircleGroup.ToComponentDataArray<NonUniformScale>(Allocator.TempJob)
        };

        return aabbCollisionJob.Schedule(CircleGroup, inputDeps);
    }

    static bool CheckCollision(float2 pos1, float2 pos2, float radius1, float radius2) // Checks if the two circles are touching or overlapping
    {
        float dX = pos1.x - pos2.x;
        float dY = pos1.y - pos2.y;
        float distanceSqr = dX * dX + dY * dY; // NOTE: oviding usage of expensive square root
        float distanceMinSqr = (radius1 + radius2) * (radius1 + radius2);
        return distanceSqr < distanceMinSqr;
    }

    static float2 PreventClipping(float2 pos1, float2 pos2, float radius1, float radius2, float distance)
    {
        float overlappingAmount = (distance - radius1 - radius2) / 2;
        return overlappingAmount * (pos1 - pos2) / distance;
    }

}
