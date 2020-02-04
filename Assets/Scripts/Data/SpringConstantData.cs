using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

[GenerateAuthoringComponent]
public struct SpringConstantData : IComponentData
{
    public float Stiffness;
}