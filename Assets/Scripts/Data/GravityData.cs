using Unity.Entities;
using UnityEngine;

[GenerateAuthoringComponent]
public struct GravityData : IComponentData
{
    public float Gravity; // Gravity amount in xy directions
}
