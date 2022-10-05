using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

[GenerateAuthoringComponent]
public struct TargetWaypoint : IComponentData
{
    public int WaypointNumber;
    public float3 Pos;
}
