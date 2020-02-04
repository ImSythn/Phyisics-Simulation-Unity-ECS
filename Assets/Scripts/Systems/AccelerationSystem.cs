using Unity.Entities;
using Unity.Jobs;

public class AccelerationSystem : JobComponentSystem
{
    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        float deltaTime = Time.DeltaTime;

        JobHandle accelJob = Entities
            .WithBurst()
            .ForEach((ref VelocityData velData, in AccelerationData accelData) =>
        {
            float AccelerationPerSecond = accelData.AccelerationPerSecond;

            velData.LinearVelocity += AccelerationPerSecond * deltaTime;
        }).Schedule(inputDeps);

        return accelJob;
    }
}
