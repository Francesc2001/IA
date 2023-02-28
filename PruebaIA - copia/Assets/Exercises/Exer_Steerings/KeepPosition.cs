using UnityEngine;

namespace Steerings
{

    public class KeepPosition : SteeringBehaviour
    {

        public GameObject target;
        public float requiredDistance;
        public float requiredAngle;

        /* COMPLETE */

        public override GameObject GetTarget()
        {
            return target;
        }

        public override Vector3 GetLinearAcceleration()
        {
            /* COMPLETE */
            return KeepPosition.GetLinearAcceleration(Context, target, requiredDistance, requiredAngle);
        }

        
        public static Vector3 GetLinearAcceleration (SteeringContext me, GameObject target, float distance, float angle)
        {
            float desiredAngle = target.transform.rotation.eulerAngles.z + angle;
            Vector3 directionFromTarget = Utils.OrientationToVector(desiredAngle);
            Vector3 displacementFromTarget = directionFromTarget.normalized * distance;
            Vector3 desiredPosition = target.transform.position + displacementFromTarget;

            SURROGATE_TARGET.transform.position = desiredPosition;
            /* COMPLETE */
            return Arrive.GetLinearAcceleration(me, SURROGATE_TARGET); // remove this line when exercise completed
        }
    }
}