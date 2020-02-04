using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;

public class SquareCollisionSystem : JobComponentSystem
{
    EntityQuery SquareGroup;
    EntityQuery WallGroup;

    protected override void OnCreate()
    {
        SquareGroup = GetEntityQuery(ComponentType.ReadWrite<OrientationData>(), ComponentType.ReadOnly<NonUniformScale>(),
            ComponentType.ReadOnly<Translation>(), ComponentType.ReadOnly<SquareTag>());
        WallGroup = GetEntityQuery(ComponentType.ReadWrite<OrientationData>(), ComponentType.ReadOnly<NonUniformScale>(), 
            ComponentType.ReadOnly<Translation>(), ComponentType.ReadOnly<WallTag>());
    }

    [BurstCompile]
    public struct AABBCollisionJob : IJobChunk
    {
        public ArchetypeChunkComponentType<OrientationData> orientationType;
        public ArchetypeChunkComponentType<Translation> transType;

        [ReadOnly] public ArchetypeChunkComponentType<NonUniformScale> scaleType;

        [DeallocateOnJobCompletion]
        [ReadOnly] public NativeArray<OrientationData> orientationToTestAgainst;
        [DeallocateOnJobCompletion]
        [ReadOnly] public NativeArray<NonUniformScale> scaleToTestAgainst;
        [DeallocateOnJobCompletion]
        [ReadOnly] public NativeArray<Translation> transToTestAgainst;


        public void Execute(ArchetypeChunk chunk, int chunkIndex, int firstEntityIndex)
        {
            var chunkMoveDirections = chunk.GetNativeArray(orientationType);
            var chunkTranslations = chunk.GetNativeArray(transType);
            var chunkScales = chunk.GetNativeArray(scaleType);
            for (int i = 0; i < chunk.Count; i++)
            {
                for (int j = 0; j < transToTestAgainst.Length; j++)
                {
                    if (CheckCollision(chunkTranslations[i].Value.xy, transToTestAgainst[j].Value.xy, chunkScales[i].Value.xy, scaleToTestAgainst[j].Value.xy))  // Checks to see if the squares are Colliding
                    {
                        // Handle elasticity bounce direction
                        OrientationData bounceOrientation = chunkMoveDirections[i];

                        float2 n = new float2(orientationToTestAgainst[j].OrientationX, orientationToTestAgainst[j].OrientationY);
                        float2 c = new float2(bounceOrientation.OrientationX, bounceOrientation.OrientationY);
                        float2 a = math.dot(c, n) * n;
                        float2 b = c - a;
                        float2 bounceDirection = b - a;

                        bounceOrientation.OrientationX = bounceDirection.x;
                        bounceOrientation.OrientationY = bounceDirection.y;

                        chunkMoveDirections[i] = bounceOrientation;

                        // Handle clipping
                        Translation correctedPos1 = chunkTranslations[i];

                        correctedPos1.Value.xy += PreventClipping(chunkScales[i].Value.xy / 2, scaleToTestAgainst[j].Value.xy / 2, chunkTranslations[i].Value.xy, transToTestAgainst[j].Value.xy, n);

                        chunkTranslations[i] = correctedPos1;
                    }
                }
            }
        }
    }

    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {

        var moveDirectionType = GetArchetypeChunkComponentType<OrientationData>(false);
        var transType = GetArchetypeChunkComponentType<Translation>(false);
        var scaleType = GetArchetypeChunkComponentType<NonUniformScale>(true);

        var aabbCollisionJob = new AABBCollisionJob()
        {
            orientationType = moveDirectionType,
            transType = transType,
            scaleType = scaleType,
            orientationToTestAgainst = WallGroup.ToComponentDataArray<OrientationData>(Allocator.TempJob),
            scaleToTestAgainst = WallGroup.ToComponentDataArray<NonUniformScale>(Allocator.TempJob),
            transToTestAgainst = WallGroup.ToComponentDataArray<Translation>(Allocator.TempJob)
        };

        return aabbCollisionJob.Schedule(SquareGroup, inputDeps);
    }

    static bool CheckCollision(float2 squarePos, float2 wallPos, float2 squareScale, float2 wallScale) // Checks if the two rectangles are touching or overlapping
    {
        return (squarePos.x - squareScale.x / 2) <= (wallPos.x - wallScale.x / 2) + wallScale.x &&      // Scale.x / 2 offsets the pivit point to the botom left so  
                (squarePos.x - squareScale.x / 2) + squareScale.x >= (wallPos.x - wallScale.x / 2) &&   // Axis-Aligned Bounding Box collision works correctly 
                (squarePos.y - squareScale.y / 2) <= (wallPos.y - wallScale.y / 2) + wallScale.y &&
                (squarePos.y - squareScale.y / 2) + squareScale.y >= (wallPos.y - wallScale.y / 2);
    }

    static float2 PreventClipping(float2 scale1, float2 scale2, float2 pos1, float2 pos2, float2 normal)
    {
        if (normal.x > 0)
        {
            float minimumDistance = scale1.x + scale2.x;
            float distance = pos1.x - pos2.x;
            return new float2(minimumDistance - distance, 0);
        }
        else if (normal.x < 0)
        {
            float minimumDistance = scale1.x + scale2.x;
            float distance = pos2.x - pos1.x;
            return new float2(-(minimumDistance - distance), 0);
        }
        else if (normal.y > 0)
        {
            float minimumDistance = scale1.y + scale2.y;
            float distance = pos1.y - pos2.y;
            return new float2(0, minimumDistance - distance);
        }
        else if (normal.y < 0)
        {
            float minimumDistance = scale1.y + scale2.y;
            float distance = pos2.y - pos1.y;
            return new float2(0, -(minimumDistance - distance));
        }
        return new float2(0, 0);
    }
}
