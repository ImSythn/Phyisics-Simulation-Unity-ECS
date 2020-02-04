using Unity.Entities;

[GenerateAuthoringComponent]
public struct EnergyLossData : IComponentData
{
    public float Amount;
}