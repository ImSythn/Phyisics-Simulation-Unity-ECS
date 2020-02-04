using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;

public class BuoyancySystem : JobComponentSystem
{
    EntityQuery BuoyantGroup;
    EntityQuery FluidGroup;

    protected override void OnCreate()
    {
        BuoyantGroup = GetEntityQuery(ComponentType.ReadWrite<VelocityData>(), ComponentType.ReadOnly<OrientationData>(),
            ComponentType.ReadOnly<NonUniformScale>(), ComponentType.ReadOnly<Translation>(), ComponentType.ReadOnly<DensityData>(), typeof(BrownSquareTag));
        FluidGroup = GetEntityQuery(ComponentType.ReadOnly<NonUniformScale>(), ComponentType.ReadOnly<Translation>(), 
            ComponentType.ReadOnly<DensityData>(), typeof(FluidTag));
    }
    
    [BurstCompile]
    public struct AABBCollisionJob : IJobChunk
    {
        public float deltaTime;

        public ArchetypeChunkComponentType<VelocityData> buoyantVelT;

        [ReadOnly] public ArchetypeChunkComponentType<OrientationData> buoyantOrT;
        [ReadOnly] public ArchetypeChunkComponentType<Translation> buoyantTransT;
        [ReadOnly] public ArchetypeChunkComponentType<NonUniformScale> buoyantScaleT;
        [ReadOnly] public ArchetypeChunkComponentType<DensityData> buoyantDensityT;

        [DeallocateOnJobCompletion]
        [ReadOnly] public NativeArray<NonUniformScale> waterScale;
        [DeallocateOnJobCompletion]
        [ReadOnly] public NativeArray<Translation> waterPos;
        [DeallocateOnJobCompletion]
        [ReadOnly] public NativeArray<DensityData> fluidDensity;

        public void Execute(ArchetypeChunk chunk, int chunkIndex, int firstEntityIndex)
        {
            var buoyantOr = chunk.GetNativeArray(buoyantOrT);
            var buoyantTrans = chunk.GetNativeArray(buoyantTransT);
            var buoyantScale = chunk.GetNativeArray(buoyantScaleT);
            var buoyantDensity = chunk.GetNativeArray(buoyantDensityT);
            var buoyantVelocity = chunk.GetNativeArray(buoyantVelT);
            for (int i = 0; i < chunk.Count; i++)
            {
                for (int j = 0; j < waterPos.Length; j++)
                {
                    if (CheckCollision(buoyantTrans[i].Value.xy, waterPos[j].Value.xy, buoyantScale[i].Value.xy, waterScale[j].Value.xy))  // Checks to see if the BuoyantGroup is in the water
                    {
                        // Calculate buoyant force
                        float buoyantForce = fluidDensity[j].Density / buoyantDensity[i].Density - 1;

                        // Add force to velocity
                        VelocityData newVelData = buoyantVelocity[i];

                        if (buoyantOr[i].OrientationY < 0)
                        {
                            if(buoyantVelocity[i].LinearVelocity > 0)
                                newVelData.LinearVelocity -= buoyantForce * deltaTime;
                        }
                        else if (buoyantOr[i].OrientationY > 0)
                        {
                            newVelData.LinearVelocity += buoyantForce * deltaTime;
                        }

                        buoyantVelocity[i] = newVelData;
                    }
                }
            }
        }
    }

    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        float deltaTime = Time.DeltaTime;
        var buoyantVel = GetArchetypeChunkComponentType<VelocityData>(false);
        var buoyantOr = GetArchetypeChunkComponentType<OrientationData>(true);
        var buoyantTrans = GetArchetypeChunkComponentType<Translation>(true);
        var buoyantScale = GetArchetypeChunkComponentType<NonUniformScale>(true);
        var buoyantDensity = GetArchetypeChunkComponentType<DensityData>(true);

        var aabbCollisionJob = new AABBCollisionJob()
        {
            deltaTime = deltaTime,
            buoyantOrT = buoyantOr,
            buoyantVelT = buoyantVel,
            buoyantTransT = buoyantTrans,
            buoyantScaleT = buoyantScale,
            buoyantDensityT = buoyantDensity,
            fluidDensity = FluidGroup.ToComponentDataArray<DensityData>(Allocator.TempJob),
            waterScale = FluidGroup.ToComponentDataArray<NonUniformScale>(Allocator.TempJob),
            waterPos = FluidGroup.ToComponentDataArray<Translation>(Allocator.TempJob)
        };

        return aabbCollisionJob.Schedule(BuoyantGroup, inputDeps);
    }

    static bool CheckCollision(float2 squarePos, float2 wallPos, float2 squareScale, float2 wallScale) // Checks if the two rectangles are touching or overlapping
    {
        return (squarePos.x - squareScale.x / 2) < (wallPos.x - wallScale.x / 2) + wallScale.x &&      // Scale.x / 2 offsets the pivit point to the botom left so  
                (squarePos.x - squareScale.x / 2) + squareScale.x > (wallPos.x - wallScale.x / 2) &&   // Axis-Aligned Bounding Box collision works correctly 
                (squarePos.y - squareScale.y / 2) < (wallPos.y - wallScale.y / 2) + wallScale.y &&
                (squarePos.y - squareScale.y / 2) + squareScale.y > (wallPos.y - wallScale.y / 2);
    }

}
