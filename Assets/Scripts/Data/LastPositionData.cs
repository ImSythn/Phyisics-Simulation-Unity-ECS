using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

[GenerateAuthoringComponent]
public struct LastPositionData : IComponentData
{
    [HideInInspector]
    public float2 Position;
}