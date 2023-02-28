using UnityEngine;

namespace Steerings
{

    public class Interpose : SteeringBehaviour
    {

        
        // remove comments for steerings that must be provided with a target 
        // remove whole block if no explicit target required
        // (if FT or FTI policies make sense, then this method must be present)
        public GameObject target;
        public GameObject secondTarget;
        public float requiredDistance;

        public override GameObject GetTarget()
        {
            return target;
        }
        
        public override Vector3 GetLinearAcceleration()
        {
            return Interpose.GetLinearAcceleration(Context, target, secondTarget, requiredDistance /* add extra parameters (target?) if required */);
        }
        
        public static Vector3 GetLinearAcceleration (SteeringContext me, GameObject target, GameObject secondTarget, float requiredDistance /* add extra parameters (target?) if required */)
        {
            Vector3 directionFromTarget = me.transform.position - ((target.transform.position + secondTarget.transform.position)/2);
            Vector3 displacementFromTarget = directionFromTarget.normalized * requiredDistance;
            Vector3 desiredPosition = ((target.transform.position + secondTarget.transform.position) / 2) + displacementFromTarget;

            SURROGATE_TARGET.transform.position = desiredPosition;
            me.transform.rotation = target.transform.rotation;

            return Seek.GetLinearAcceleration(me, SURROGATE_TARGET);
            /* COMPLETE this method. It must return the linear acceleration (Vector3) */
        }

    }
}