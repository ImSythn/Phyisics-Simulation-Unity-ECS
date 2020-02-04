using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

[GenerateAuthoringComponent]
public struct SpringData : IComponentData
{
    [HideInInspector]
    public bool HasStarted;
    [HideInInspector]
    public float RestLength;
    [HideInInspector]
    public float2 Anchor;
}