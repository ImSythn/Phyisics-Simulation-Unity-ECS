using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;

public class MovementSystem : JobComponentSystem
{
    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        float deltaTime = Time.DeltaTime;

        JobHandle moveJob = Entities
            .WithBurst()
            .ForEach((ref Translation pos, in VelocityData velData, in OrientationData orData) =>
        {
            float moveVel = velData.LinearVelocity;
            
            float magnitude = math.sqrt(orData.OrientationX * orData.OrientationX + orData.OrientationY * orData.OrientationY);
            float2 normalizedMoveDir = new float2(orData.OrientationX / magnitude, orData.OrientationY / magnitude);
            
            pos.Value.xy += moveVel * normalizedMoveDir * deltaTime;
        }).Schedule(inputDeps);
        
        return moveJob;
    }
}
