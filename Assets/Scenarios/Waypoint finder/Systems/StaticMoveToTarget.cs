using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;

public partial class StaticMoveToTarget : SystemBase
{
    protected override void OnUpdate()
    {
        float deltaTime = Time.DeltaTime;
        
        /*
        Entities.ForEach((ref Translation translation, in Rotation rotation, in Destination destination, in MovementSpeed speed) => {
            
            float3 toDestination = destination.Value - translation.Value;

            float3 movement = math.normalize(toDestination) * speed.Value * deltaTime;


            if(math.length(movement) >= math.length(toDestination)){

                translation.Value = destination.Value;
            }
            else
            {
                translation.Value += movement;
            }
          
            // translation.Value += movement;

        }).ScheduleParallel(); // Spred across multiple different cores
        */
    }
}
