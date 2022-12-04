using System;
using Unity.Entities;

[GenerateAuthoringComponent]
public struct Dimensions : IComponentData
{
    public float Length;
    public float Width;
}
