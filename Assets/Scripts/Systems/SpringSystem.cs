using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;

public class SpringSystem : JobComponentSystem
{
    EntityQuery BobGroup;
    EntityQuery SpringGroup;

    protected override void OnCreate()
    {
        BobGroup = GetEntityQuery(ComponentType.ReadOnly<Translation>(), ComponentType.ReadOnly<NonUniformScale>(), typeof(GreenSquareTag));
        SpringGroup = GetEntityQuery(ComponentType.ReadWrite<SpringData>(), ComponentType.ReadWrite<Translation>(),
            ComponentType.ReadWrite<NonUniformScale>(), typeof(SpringTag));
    }

    [BurstCompile]
    public struct SpringJob : IJobChunk
    {
        public ArchetypeChunkComponentType<SpringData> springDataT;
        public ArchetypeChunkComponentType<Translation> springPosT;
        public ArchetypeChunkComponentType<NonUniformScale> springScaleT;

        [DeallocateOnJobCompletion]
        [ReadOnly] public NativeArray<Translation> bobPos;
        [DeallocateOnJobCompletion]
        [ReadOnly] public NativeArray<NonUniformScale> bobScale;

        public void Execute(ArchetypeChunk chunk, int chunkIndex, int firstEntityIndex)
        {
            var springData = chunk.GetNativeArray(springDataT);
            var springPosition = chunk.GetNativeArray(springPosT);
            var springScale = chunk.GetNativeArray(springScaleT);

            for (int i = 0; i < chunk.Count; i++)
            {
                SpringData spring = springData[i];
                // Sets the starting porperties of the strings
                if (!spring.HasStarted)
                {
                    spring.RestLength = springScale[i].Value.x / 1.5f;
                    if (i == 1)
                        spring.RestLength = springScale[i].Value.x / 1.45f;
                    if (i == 2)
                        spring.RestLength = springScale[i].Value.x / 1.2f;
                    spring.Anchor = new float2(springPosition[i].Value.x - (springScale[i].Value.x / 2), springPosition[i].Value.y);
                    spring.HasStarted = true;
                }
                // sets the anchor of second string in series to the position of the green square before it.
                if (i == 1)
                    spring.Anchor = new float2(bobPos[2].Value.x + (bobScale[2].Value.x / 2), springPosition[2].Value.y);

                springData[i] = spring;

                // Handle Spring scale
                NonUniformScale scale = springScale[i];

                scale.Value.x = (bobPos[i].Value.x + (bobScale[i].Value.x / 2)) - springData[i].Anchor.x - bobScale[i].Value.x;

                springScale[i] = scale;

                // Handle Spring position
                Translation pos = springPosition[i];

                pos.Value.x = (springData[i].Anchor.x + (bobPos[i].Value.x - (bobScale[i].Value.x / 2))) / 2;

                springPosition[i] = pos;

            }
        }
    }

    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        var springData = GetArchetypeChunkComponentType<SpringData>(false);
        var springPos = GetArchetypeChunkComponentType<Translation>(false);
        var springScale = GetArchetypeChunkComponentType<NonUniformScale>(false);

        var aabbCollisionJob = new SpringJob()
        {
            springDataT = springData,
            springPosT = springPos,
            springScaleT = springScale,
            bobPos = BobGroup.ToComponentDataArray<Translation>(Allocator.TempJob),
            bobScale = BobGroup.ToComponentDataArray<NonUniformScale>(Allocator.TempJob),
        };

        return aabbCollisionJob.Schedule(SpringGroup, inputDeps);
    }
}
