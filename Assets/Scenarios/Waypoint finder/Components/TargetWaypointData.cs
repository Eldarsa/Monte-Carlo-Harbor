using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

[GenerateAuthoringComponent]
public class TargetWaypointData : IComponentData
{
    public bool WaypointSet = false;
    public int WaypointNumber = -1;
    public float3 Pos;
}
