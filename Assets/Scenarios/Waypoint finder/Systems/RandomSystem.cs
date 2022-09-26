using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Jobs.LowLevel.Unsafe;
using Unity.Mathematics;
using Unity.Transforms;

/* The point of this class is to ensure random number generation across multiple threads.
    We basically use a number generator to generate random numbers for another number generator.
    We then have as many random generators as there are threads and store them in a randomArray that
    other systems can access later. 

    To ensure that this is created when everyone else tries to access it, we use UpdateInGroup -> InitializationSystemGroup
*/

[UpdateInGroup(typeof(InitializationSystemGroup))]
public partial class RandomSystem : SystemBase
{

    public NativeArray<Unity.Mathematics.Random> RandomArray { get; private set; }

    protected override void OnCreate()
    {
        var randomArray = new Unity.Mathematics.Random[JobsUtility.MaxJobThreadCount];
        var seed = new System.Random();
        
        for (int i = 0; i < JobsUtility.MaxJobThreadCount; i++){
            randomArray[i] = new Unity.Mathematics.Random((uint)seed.Next());
        }

        RandomArray = new NativeArray<Unity.Mathematics.Random>(randomArray, Allocator.Persistent);
    }

    protected override void OnUpdate() { }

    protected override void OnDestroy()
    {
        RandomArray.Dispose();
    }
}
