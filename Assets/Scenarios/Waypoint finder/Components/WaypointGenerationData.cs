using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

[GenerateAuthoringComponent]
public struct WaypointGenerationData : IComponentData
{   
    public Entity WaypointPrefab;
    public float3 StartPoint;
    public float3 EndPoint;
    public int Count;
    // Maybe some data about how the waypoints should be spread?
}