using System;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;

[GenerateAuthoringComponent]
public struct PositionVector : IComponentData
{
    public float x;
    public float y;
    public float psi;
}
