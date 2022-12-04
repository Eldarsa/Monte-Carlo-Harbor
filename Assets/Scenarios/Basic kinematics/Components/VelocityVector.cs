using System;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;

[GenerateAuthoringComponent]
public struct VelocityVector : IComponentData
{
    public float u;
    public float v;
    public float r;
}
