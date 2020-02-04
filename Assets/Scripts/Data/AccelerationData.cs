using Unity.Entities;

[GenerateAuthoringComponent]
public struct AccelerationData : IComponentData
{
    public float AccelerationPerSecond; // Speed the entity increses in velocity per second
}