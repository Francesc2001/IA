using UnityEngine;

namespace Steerings
{

    public class InterfereWithAvoidance : SteeringBehaviour
    {

        
        // remove comments for steerings that must be provided with a target 
        // remove whole block if no explicit target required
        // (if FT or FTI policies make sense, then this method must be present)
        public GameObject target;
        public float requiredDistance;

        public override GameObject GetTarget()
        {
            return target;
        }
        
        
        public override Vector3 GetLinearAcceleration()
        {
            return InterfereWithAvoidance.GetLinearAcceleration(Context, target, requiredDistance /* add extra parameters (target?) if required */);
        }

        
        public static Vector3 GetLinearAcceleration (SteeringContext me, GameObject target, float requiredDistance /* add extra parameters (target?) if required */)
        {
            Vector3 avoidanceAcceleration = ObstacleAvoidance.GetLinearAcceleration(me);
            if (avoidanceAcceleration.Equals(Vector3.zero))
            {
                return Interfere.GetLinearAcceleration(me, target, requiredDistance);
            }
            else
            {
                return avoidanceAcceleration;
            }
            /* COMPLETE this method. It must return the linear acceleration (Vector3) */
        }

    }
}