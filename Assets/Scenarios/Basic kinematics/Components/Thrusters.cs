using System;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;

[GenerateAuthoringComponent]
public struct Thrusters : IComponentData
{
    // [rad/s]
    public float LeftThrusterSpeed;
    public float RightThrusterSpeed;
    public float MaxThrusterSpeed;
}
