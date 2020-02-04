using Unity.Entities;
using Unity.Jobs;
using Unity.Transforms;

public class GravitySystem : JobComponentSystem
{
    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        float deltaTime = Time.DeltaTime;
        JobHandle accelJob = Entities
            .WithBurst()
            .ForEach((ref OrientationData orData, ref VelocityData velData, ref LastPositionData lastPosData, in GravityData gravData, in Translation posData) =>
            {
                // Orientate to move direction
                if (posData.Value.y < lastPosData.Position.y)
                {
                    orData.OrientationY = -1;
                }
                else if(posData.Value.y > lastPosData.Position.y)
                {
                    orData.OrientationY = 1;
                }
                orData.OrientationX = 0;

                // Add gravity to entity
                if (orData.OrientationY < 0)
                {
                    velData.LinearVelocity += gravData.Gravity * deltaTime;
                }
                else if (orData.OrientationY > 0)
                {
                    if(velData.LinearVelocity > 0)
                        velData.LinearVelocity -= gravData.Gravity * deltaTime;
                }

                lastPosData.Position = posData.Value.xy;
            }).Schedule(inputDeps);

        return accelJob;
    }
}

