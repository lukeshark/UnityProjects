using UnityEngine;
using System.Collections;
using AxlPlay;
namespace AxlPlay {
//Check to see if the any objects not are within hearing range of the current agent
[AddComponentMenu("Easy AI/Not See Object")]

public class NotSeeObject : MonoBehaviour
{
    [Tooltip("Should the 2D version be used?")]
    public bool usePhysics2D;
    [Tooltip("The field of view angle of the agent (in degrees)")]
    public float fieldOfViewAngle = 90f;
    [Tooltip("The distance that the agent can see")]
    public float viewDistance = 10f;
    [Tooltip("The offset relative to the pivot position")]
    public Vector3 offset;
    [Tooltip("The object that is within sight")]
    public GameObject returnedObject;

    [Tooltip("The object that we are searching for. If this value is null then the objectLayerMask will be used")]
    public GameObject targetObject;
    [Tooltip("The LayerMask of the objects that we are searching for")]
    public LayerMask HitLayerMask;
    [Tooltip("The LayerMask of the objects to ignore when performing the line of sight check")]
    public LayerMask ignoreLayerMask;
    [Tooltip("The target offset relative to the pivot position")]
    public Vector3 targetOffset;
    [Tooltip("The angle offset relative to the pivot position 2D")]
    public float angleOffset2D;

    public GameObject NotSee()
    {
        if (usePhysics2D)
        {
            // If the target object is null then determine if there are any objects within sight based on the layer mask
            if (targetObject == null)
            {
	            returnedObject = MovementUtility.WithinSight2D(transform, offset, fieldOfViewAngle, viewDistance, -1, targetOffset, angleOffset2D, ignoreLayerMask);
            }
            else
            { // If the target is not null then determine if that object is within sight
                returnedObject = MovementUtility.WithinSight2D(transform, offset, fieldOfViewAngle, viewDistance, targetObject, targetOffset, angleOffset2D, ignoreLayerMask);
            }
        }
        else
        {
            // If the target object is null then determine if there are any objects within sight based on the layer mask
            if (targetObject == null)
            {
	            returnedObject = MovementUtility.WithinSight(transform, offset, fieldOfViewAngle, viewDistance, -1, targetOffset, ignoreLayerMask);
            }
            else
            { // If the target is not null then determine if that object is within sight
                returnedObject = MovementUtility.WithinSight(transform, offset, fieldOfViewAngle, viewDistance, targetObject, targetOffset, ignoreLayerMask);
            }
        }
        if (returnedObject == null)
        {
            return returnedObject;
        }
        else
        {
            return null;
        }

    }
    // Draw the line of sight representation within the scene window
    void OnDrawGizmos()
    {
        if (this.gameObject != null)
            CanSeeObject.DrawLineOfSight(transform, offset, fieldOfViewAngle, viewDistance, usePhysics2D);
    }

}
}
