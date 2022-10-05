using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public partial class WaypointGenerationSystem : SystemBase
{

    // Setup EntityManager and make an entity instance of our universe prefab
    private EndInitializationEntityCommandBufferSystem _ecbSystem;

    public NativeParallelHashMap<int, Entity> WaypointManagerMap;

    protected override void OnStartRunning()
    {
        _ecbSystem = World.GetOrCreateSystem<EndInitializationEntityCommandBufferSystem>();

        // Has to be constructed in special way https://forum.unity.com/threads/solved-nativecontainer-result-has-not-been-assigned-or-constructed-but-i-did.545483/
        WaypointManagerMap = new NativeParallelHashMap<int, Entity>(100000, Allocator.Persistent); // TODO: Use the number of universes as capacity

    }

    protected override void OnUpdate()
    {
        var ecb = _ecbSystem.CreateCommandBuffer().AsParallelWriter();       

        var wpm = WaypointManagerMap.AsParallelWriter(); 
        
        // TODO: Give waypoint buffer so that we can see all the waypoints for this particular manager
        var entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;

        // Spawn Waypoint managers in every universe with an InitWaypointManagerTag 
        Entities
            .WithAll<InitWaypointManagerTag>()
            .ForEach((Entity e, int entityInQueryIndex, in UniverseId id, in Translation translation) => {
                
                // The Entity here will be a Universe

                Entity newWaypointManager = ecb.CreateEntity(entityInQueryIndex);

                // Set up the waypoint manager to spawn waypoints
                ecb.AddComponent<SpawnWaypointsTag>(entityInQueryIndex, newWaypointManager);

                // Pass all the components the waypointManager needs
                ecb.AddComponent(entityInQueryIndex, newWaypointManager, new UniverseId { Value = id.Value});
                ecb.AddComponent(entityInQueryIndex, newWaypointManager, new Translation { Value = translation.Value});

                // Add a dynamic buffer to store waypoints in such that the boat can find them
                ecb.AddBuffer<WaypointBufferElement>(entityInQueryIndex, newWaypointManager);

                // Pass the waypoint generation data from the parent entity to the new waypoint manager
                var waypointGenerationData = entityManager.GetComponentData<WaypointGenerationData>(e);
                ecb.AddComponent(entityInQueryIndex, newWaypointManager, waypointGenerationData);

                // Add it to a hashmap
                wpm.TryAdd(id.Value, newWaypointManager);
                //WaypointManagerMap.Add(id.Value, newWaypointManager);

                // Remove this component to signal that operation is complete
                ecb.RemoveComponent<InitWaypointManagerTag>(entityInQueryIndex, e);

            }).Schedule(); //.WithoutBurst().Run(); //.Schedule(); //WithoutBurst().Run();

        // Spawn waypoints in each universe
        /* We need to find a way for the waypoint following entities to find their own waypoint manager 
         * One option could be to make a buffer that has all the waypoint managers, but it would have to have
         * as much memory allocation as there are universes. Maybe a better way is to somehow loop throgh all the 
         * waypoint managers and try to store a reference to the correct one for each waypoint finding entity. 
         * This is more difficult now that we can't do nested ForEach loops.
        */
        Entities
            .WithAll<SpawnWaypointsTag>()
            .ForEach((Entity e, int entityInQueryIndex, in WaypointGenerationData data, in UniverseId id, in Translation translation) => {

                // The Entity here will be a WaypointManager

                float3[] waypoints = calculateWaypoints(
                    data.StartPoint, 
                    data.EndPoint, 
                    data.Count);

                int counter = 0;
                foreach(float3 wp in waypoints){
                    var newWaypoint = ecb.Instantiate(entityInQueryIndex, data.WaypointPrefab);

                    var wpPos = new Translation { 
                        Value = wp + translation.Value
                        };

                    // Give the waypoint some metadata for future reference
                    var wpData = new WaypointData { 
                        UniverseId = id.Value, 
                        WaypointNumber = counter
                        };

                    ecb.SetComponent(entityInQueryIndex, newWaypoint, wpPos);
                    ecb.SetComponent(entityInQueryIndex, newWaypoint, wpData);

                    // Add a reference to the waypoint in the WaypointManagers waypoint buffer
                    var wpBufferElement = new WaypointBufferElement() { Waypoint = newWaypoint };
                    entityManager.GetBuffer<WaypointBufferElement>(e, false).Add(wpBufferElement);

                    counter++;

                }

                // We have spawned waypoints now. Remove component..
                ecb.RemoveComponent<SpawnWaypointsTag>(entityInQueryIndex, e);

        }).WithoutBurst().Run();
        _ecbSystem.AddJobHandleForProducer(this.Dependency);
    

    }

    float3[] calculateWaypoints(float3 startPoint, float3 endPoint, int count){

        float3[] list = new float3[count];
        float3 spacing = math.abs(( endPoint - startPoint ) / count);
        Debug.Log(spacing.ToString());
        Debug.Log(count.ToString());

        for(int i = 0; i < count; i++){
            list[i] = startPoint + spacing * i;
            Debug.Log(list[i].ToString());
        }

        return list;
    }
}
