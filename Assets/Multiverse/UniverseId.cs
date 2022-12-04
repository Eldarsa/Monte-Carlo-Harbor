using Unity.Entities;

[GenerateAuthoringComponent]
public struct UniverseId : IComponentData
{
    public int Value;
}