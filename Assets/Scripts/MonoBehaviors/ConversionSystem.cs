using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;

[UpdateInGroup(typeof(InitializationSystemGroup))]
public partial class ConversionSystem : GameObjectConversionSystem
{
    protected override void OnUpdate()
    {
    }
}
