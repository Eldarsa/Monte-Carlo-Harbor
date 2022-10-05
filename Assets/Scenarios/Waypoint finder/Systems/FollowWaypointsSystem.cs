using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;

// Guy doing the same: https://forum.unity.com/threads/something-able-to-put-nativehashmap-or-nativelist-into-icomponentdata-or-send-to-job.628396/

public partial class FollowWaypointsSystem : SystemBase
{

    private EndInitializationEntityCommandBufferSystem _ecbSystem;

    // Access external hashmap for referencing
    private WaypointGenerationSystem _generateWaypointSystem;
    private NativeParallelHashMap<int, Entity> _waypointManagerMap;

    protected override void OnStartRunning()
    {

        _ecbSystem = World.GetOrCreateSystem<EndInitializationEntityCommandBufferSystem>();

        _generateWaypointSystem = World.GetExistingSystem<WaypointGenerationSystem>();
        _waypointManagerMap = _generateWaypointSystem.WaypointManagerMap;
    }

    protected override void OnUpdate()
    {

        float deltaTime = Time.DeltaTime;

        var ecb = _ecbSystem.CreateCommandBuffer().AsParallelWriter();       

        var entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
        /*

        // If the entity is not following a waypoint at the moment, check buffer to see if there are any available
        Entities
            .WithAll<FollowWaypointsTag>()
            .ForEach((Entity e, int entityInQueryIndex, in UniverseId uid) => {

            Entity wpManager = _waypointManagerMap[uid.Value];

            DynamicBuffer<WaypointBufferElement> wpBuffer = entityManager.GetBuffer<WaypointBufferElement>(wpManager);

            Entity wp = wpBuffer[0].Waypoint;
            float3 wpPos = entityManager.GetComponentData<Translation>(wp).Value;

            ecb.SetComponent(entityInQueryIndex, e, new TargetWaypoint { WaypointNumber = 0, Pos = wpPos });
            
            // Set the state to start following waypoints
            ecb.AddComponent<FollowingWaypointsTag>(entityInQueryIndex, e);

        }).WithoutBurst().Run();

        Entities
            .WithAll<FollowingWaypointsTag>()
            .ForEach((ref TargetWaypoint tp, ref Translation translation, in MovementSpeed speed, in Rotation rotation) => {

            var destination = tp.Pos;
            float3 toDestination = destination - translation.Value;

            float3 movement = math.normalize(toDestination) * speed.Value * deltaTime;

            if(math.length(movement) >= math.length(toDestination)){

                translation.Value = destination;
            }
            else
            {
                translation.Value += movement;
            }
          


            }).ScheduleParallel();
    */
    }
    
}
