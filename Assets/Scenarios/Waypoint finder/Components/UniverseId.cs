using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

[GenerateAuthoringComponent]
public struct UniverseId : IComponentData
{
    public int Value;
}
