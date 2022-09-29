using System;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;

[GenerateAuthoringComponent]
public struct WaypointData : IComponentData
{
    public int UniverseId;
    public int WaypointNumber; // The number in its own queue
}
