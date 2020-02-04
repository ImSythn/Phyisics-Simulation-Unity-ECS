using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Transforms;

public class HookesLawSystem : JobComponentSystem
{
    EntityQuery BobGroup;
    EntityQuery SpringGroup;

    protected override void OnCreate()
    {
        BobGroup = GetEntityQuery(ComponentType.ReadWrite<AccelerationData>(), ComponentType.ReadOnly<NonUniformScale>(),
            ComponentType.ReadOnly<Translation>(), typeof(GreenSquareTag));
        SpringGroup = GetEntityQuery(ComponentType.ReadOnly<SpringData>(), ComponentType.ReadOnly<NonUniformScale>(), 
            ComponentType.ReadOnly<SpringConstantData>(), typeof(SpringTag));
    }

    [BurstCompile]
    public struct HookesLawJob : IJobChunk
    {
        public float deltaTime;

        public ArchetypeChunkComponentType<AccelerationData> bobAccelT;
        [ReadOnly] public ArchetypeChunkComponentType<Translation> bobPosT;
        [ReadOnly] public ArchetypeChunkComponentType<NonUniformScale> bobScaleT;

        [DeallocateOnJobCompletion]
        [ReadOnly] public NativeArray<SpringData> springData;
        [DeallocateOnJobCompletion]
        [ReadOnly] public NativeArray<NonUniformScale> springScale;
        [DeallocateOnJobCompletion]
        [ReadOnly] public NativeArray<SpringConstantData> springConstant;

        public void Execute(ArchetypeChunk chunk, int chunkIndex, int firstEntityIndex)
        {
            var bobAcceleration = chunk.GetNativeArray(bobAccelT);
            var bobPosition = chunk.GetNativeArray(bobPosT);
            var bobScale = chunk.GetNativeArray(bobScaleT);


            for (int i = 0; i < chunk.Count; i++)
            {
                // Add forces to bob
                AccelerationData newVelocityData = bobAcceleration[i];

                float k = springConstant[i].Stiffness;
                newVelocityData.AccelerationPerSecond =  k * (bobPosition[i].Value.x - (bobScale[i].Value.x / 2) - springData[i].Anchor.x - springData[i].RestLength) / deltaTime;
                
                bobAcceleration[i] = newVelocityData;
            }
        }
    }

    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        float deltaTime = Time.DeltaTime;
        var bobAccel = GetArchetypeChunkComponentType<AccelerationData>(false);
        var bobPos = GetArchetypeChunkComponentType<Translation>(true);
        var bobScale = GetArchetypeChunkComponentType<NonUniformScale>(true);

        var aabbCollisionJob = new HookesLawJob()
        {
            deltaTime = deltaTime,
            bobAccelT = bobAccel,
            bobPosT = bobPos,
            bobScaleT = bobScale,
            springData = SpringGroup.ToComponentDataArray<SpringData>(Allocator.TempJob),
            springScale = SpringGroup.ToComponentDataArray<NonUniformScale>(Allocator.TempJob),
            springConstant = SpringGroup.ToComponentDataArray<SpringConstantData>(Allocator.TempJob),
        };

        return aabbCollisionJob.Schedule(BobGroup, inputDeps);
    }
}
