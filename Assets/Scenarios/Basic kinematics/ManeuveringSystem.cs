using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;

public partial class ManeuveringSystem : SystemBase
{
    static float g = 9.81f;

    static double k_pos = 0.02216/2;                           // Positive Bollard, one propeller 
    static double k_neg = 0.01289/2;                           // Negative Bollard, one propeller 
    static double n_max = math.sqrt((0.5*24.4 * g) / k_pos);     // maximum propeller rev. (rad/s)
    static double n_min = -math.sqrt((0.5*13.6 * g) / k_neg);    // minimum propeller rev. (rad/s)

    protected override void OnUpdate()
    {

        float deltaTime = Time.DeltaTime;
        
        
        Entities.ForEach((  ref Translation translation, 
                            ref Rotation rotation,
                            in VelocityVector velVector,
                            in PositionVector posVector, 
                            in Dimensions dims,
                            in Mass mass,
                            in Thrusters thrusters
                            ) => {

            //float current_surge =             
            // MA = -diag([Xudot, Yvdot, Zwdot, Kpdot, Mqdot, Nrdot]);  
            // M = MRB + MA
            // CA = m2c(MA, nu_r)


            // Linear damping terms 
            //float Xu = -24.4 * g / Umax;


            // Calculate thrust 



            // Implement the work to perform for each entity here.
            // You should only access data that is local or that is a
            // field on this job. Note that the 'rotation' parameter is
            // marked as 'in', which means it cannot be modified,
            // but allows this job to run in parallel with other jobs
            // that want to read Rotation component data.
            // For example,
            //     translation.Value += math.mul(rotation.Value, new float3(0, 0, 1)) * deltaTime;
        }).Schedule();
    }
}
