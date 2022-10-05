using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;

public partial class SetDestinationSystem : SystemBase
{

    private RandomSystem randomSystem;

    // We need to get a reference to the local coordinate system somehow
    //private float3 shiftedOrigin = new float3(-50, 0, -50);
    //private float3 shiftedEndpoint = new float3(50, 0, 50);

    // Maybe take in universe parameters as a system??

    protected override void OnCreate()
    {
        randomSystem = World.GetExistingSystem<RandomSystem>(); // Notice how we can use this method to get system references
    }

    protected override void OnUpdate()
    {

        var randomArray = randomSystem.RandomArray;
        
        /*
        Entities
            .WithNativeDisableParallelForRestriction(randomArray)
            .ForEach((int nativeThreadIndex, ref Destination destination, in Translation translation) => 
        {
            float distance = math.abs(math.length(destination.Value - translation.Value));

            if(distance < 0.1f) 
            {
                var random = randomArray[nativeThreadIndex];

                destination.Value.x = random.NextFloat(-50f, 50f);
                destination.Value.z = random.NextFloat(-50f, 50f);

                randomArray[nativeThreadIndex] = random; // Have to do this to tell the random array we have made changes ( used it essentially )
            }

        }).ScheduleParallel();
        */
    }
}
