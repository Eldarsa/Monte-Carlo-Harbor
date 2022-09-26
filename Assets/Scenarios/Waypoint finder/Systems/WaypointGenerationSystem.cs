using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public partial class WaypointGenerationSystem : SystemBase
{
    private Entity _waypointEntity;
    
    // The following will have to be fetched from each universe individually in concept
    // private Entity _waypointSpawner?
    // private Random _random;
    // private float3 _minPos = float3.zero;
    // private float3 _maxPos = new float3(50, 0, 50);

    private EndInitializationEntityCommandBufferSystem _ecbSystem;

    protected override void OnStartRunning()
    {
        //_waypointPrefab = GetSingletonEntity<WaypointPrefab>();


        _ecbSystem = World.GetOrCreateSystem<EndInitializationEntityCommandBufferSystem>();
                // Setup EntityManager and make an entity instance of our universe prefab


        // May be useful?
        //_capsuleSpawner = GetSingletonEntity<LastSpawnedCapsule>();
        //EntityManager.AddBuffer<SpawnedCapsuleBufferElement>(_capsuleSpawner);
    }

    protected override void OnUpdate()
    {
        var ecb = _ecbSystem.CreateCommandBuffer().AsParallelWriter();        
        //var spawnerQuery = EntityManager.CreateEntityQuery(typeof(SpawnWaypointsTag), ComponentType.ReadOnly<WaypointGenerationData>()); // Basically ask "give me every entity that have this specific component!"
        
        Entities
            .WithAll<SpawnWaypointsTag>()
            .ForEach((Entity e, int entityInQueryIndex, in WaypointGenerationData data, in AreaDefinition area, in Translation translation) => {

                var newWaypoint = ecb.Instantiate(entityInQueryIndex, data.WaypointPrefab);
                var newPos = new Translation { Value = new float3(20, 0, 20) + translation.Value};
                ecb.SetComponent(entityInQueryIndex, newWaypoint, newPos);

                // We have spawned waypoints now. Remove component..
                ecb.RemoveComponent<SpawnWaypointsTag>(entityInQueryIndex, e);

        }).WithoutBurst().Run();
        _ecbSystem.AddJobHandleForProducer(this.Dependency);
    }
}
