using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public partial class WaypointGenerationSystem : SystemBase
{

    private EndInitializationEntityCommandBufferSystem _ecbSystem;

    protected override void OnStartRunning()
    {
        _ecbSystem = World.GetOrCreateSystem<EndInitializationEntityCommandBufferSystem>();
    }

    protected override void OnUpdate()
    {
        var ecb = _ecbSystem.CreateCommandBuffer().AsParallelWriter();        
        
        // Spawn waypoints in each universe
        Entities
            .WithAll<SpawnWaypointsTag>()
            .ForEach((Entity e, int entityInQueryIndex, in WaypointGenerationData data, in UniverseData universeData, in Translation translation) => {

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
                        UniverseId = universeData.Id, 
                        WaypointNumber = counter
                        };

                    ecb.SetComponent(entityInQueryIndex, newWaypoint, wpPos);
                    ecb.SetComponent(entityInQueryIndex, newWaypoint, wpData);

                    counter++;
                }

                // We have spawned waypoints now. Remove component..
                ecb.RemoveComponent<SpawnWaypointsTag>(entityInQueryIndex, e);

        }).WithoutBurst().Run();
        _ecbSystem.AddJobHandleForProducer(this.Dependency);


        Entities
            .WithAll<FollowWaypointsTag>()
            //.ForEach((Entity e, int entityInQueryIndex, ref TargetWaypointData tp, ref Destination dest, in UniverseData universeData) => {
            .ForEach((Entity e, int entityInQueryIndex, ref Destination dest, in UniverseData universeData, in TargetWaypointData tp ) => {

                // Maybe keep a reference to a waypoint manager that is in the same universe

                if(tp.WaypointSet == false){

                    int thisUniverseId = universeData.Id;
                    int currentWaypointId = tp.WaypointNumber;
                    int nextWaypointId = currentWaypointId + 1;

                    float3 nextDestination = new float3(0,0,0);
                    bool setNextDestination = false;

                    if(setNextDestination){
                        dest.Value = nextDestination;
                    }

                    //tp.WaypointSet = true;

                }

                // WE ARE ONLY SETTING ONE WAYPOINT HERE

                // TODO: Improve this whole logic !!


        }).WithoutBurst().Run();
    
        /*
        Entities
            .WithAll<FollowingWaypointsTag>()
            .ForEach((Entity e, int entityInQueryIndex, ref TargetWaypointData tp, ref Destination dest, in UniverseData universeData) => {

                

                if(tp.WaypointSet == false){

                    int thisUniverseId = universeData.Id;
                    int currentWaypointId = tp.WaypointNumber;
                    int nextWaypointId = currentWaypointId + 1;

                    float3 nextDestination = new float3(0,0,0);
                    bool setNextDestination = false;

                    if(setNextDestination){
                        dest.Value = nextDestination;
                    }

                    tp.WaypointSet = true;

                }

                // WE ARE ONLY SETTING ONE WAYPOINT HERE

                // TODO: Improve this whole logic !!


        }).WithoutBurst().Run();
        */
    

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
