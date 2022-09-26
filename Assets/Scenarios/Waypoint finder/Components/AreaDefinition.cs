using System;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;

[GenerateAuthoringComponent]
public struct AreaDefinition : IComponentData
{
    public float3 GlobalOrigin;
    public float Width;    
    public float Length;
}
