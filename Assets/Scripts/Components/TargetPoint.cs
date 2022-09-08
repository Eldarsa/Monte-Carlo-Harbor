using System;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;

[Serializable]
[GenerateAuthoringComponent]
public struct TargetPoint : IComponentData
{   
    public float3 Value;   
}
