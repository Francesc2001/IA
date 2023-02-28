using UnityEngine;

namespace Steerings
{

    public class Interfere : SteeringBehaviour
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
            return Interfere.GetLinearAcceleration(Context, target, requiredDistance /* add extra parameters (target?) if required */);
        }

        
        public static Vector3 GetLinearAcceleration (SteeringContext me, GameObject target, float requiredDistance /* add extra parameters (target?) if required */)
        {
            SteeringContext targetContext = target.GetComponent<SteeringContext>();
            Vector3 displacementFromTarget = targetContext.velocity.normalized * requiredDistance;
            Vector3 desiredPosition = target.transform.position + displacementFromTarget;

            SURROGATE_TARGET.transform.position = desiredPosition;

            return Arrive.GetLinearAcceleration(me, SURROGATE_TARGET);
            /* COMPLETE this method. It must return the linear acceleration (Vector3) */
        }

    }
}