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
    //private MLApp.MLApp _matlab;

    private EndInitializationEntityCommandBufferSystem _ecbSystem;

    protected override void OnStartRunning()
    {
        //_matlab = new MLApp.MLApp();
        _ecbSystem = World.GetOrCreateSystem<EndInitializationEntityCommandBufferSystem>();
    }

    protected override void OnUpdate()
    {
        var ecb = _ecbSystem.CreateCommandBuffer().AsParallelWriter();        
        
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


    /*
        Entities
            .WithAll<FollowWaypointsTag>()
            .ForEach() => {

            }
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
