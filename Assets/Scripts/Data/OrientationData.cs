using Unity.Entities;

[GenerateAuthoringComponent]
public struct OrientationData : IComponentData
{
    public float OrientationX;
    public float OrientationY;
}
