using UnityEngine;
using System.Collections;
using AxlPlay;

namespace AxlPlay {
[AddComponentMenu("Easy AI/Can See Object")]
public class CanSeeObject : MonoBehaviour {

    public bool usePhysics2D;
    [Tooltip("The object that we are searching for. If this value is null then the objectLayerMask will be used")]
    public GameObject targetObject;
    [Tooltip("The LayerMask of the objects that we are searching for")]
    public LayerMask objectLayerMask;
    [Tooltip("The LayerMask of the objects to ignore when performing the line of sight check")]
    public LayerMask ignoreLayerMask;
    [Tooltip("The field of view angle of the agent (in degrees)")]
    public float fieldOfViewAngle = 90;
    [Tooltip("The distance that the agent can see")]
    public float viewDistance = 25;
    [Tooltip("The offset relative to the pivot position")]
    public Vector3 offset;
    [Tooltip("The target offset relative to the pivot position")]
    public Vector3 targetOffset;
    [Tooltip("The object that is within sight")]
    public GameObject returnedObject;
    [Tooltip("The angle offset relative to the pivot position 2D")]
    public float angleOffset2D;

    void Awake()
    {
	    ignoreLayerMask = 1 << LayerMask.NameToLayer("Ignore Raycast");

    }
		void Update() {
			CanSee ();
		}
  
    public GameObject CanSee()
    {
        if (usePhysics2D)
        {
            // If the target object is null then determine if there are any objects within sight based on the layer mask
            if (targetObject == null)
            {
                returnedObject = MovementUtility.WithinSight2D(transform, offset, fieldOfViewAngle, viewDistance, objectLayerMask, targetOffset, angleOffset2D, ignoreLayerMask);
            }
            else { // If the target is not null then determine if that object is within sight
                returnedObject = MovementUtility.WithinSight2D(transform, offset, fieldOfViewAngle, viewDistance, targetObject, targetOffset, angleOffset2D, ignoreLayerMask);
            }
        }
        else {
            // If the target object is null then determine if there are any objects within sight based on the layer mask
            if (targetObject == null)
            {
                returnedObject = MovementUtility.WithinSight(transform, offset, fieldOfViewAngle, viewDistance, objectLayerMask, targetOffset, ignoreLayerMask);
            }
            else { // If the target is not null then determine if that object is within sight
                returnedObject = MovementUtility.WithinSight(transform, offset, fieldOfViewAngle, viewDistance, targetObject, targetOffset, ignoreLayerMask);
            }
        }
        if (returnedObject != null)
        {
            // Return success if an object was found
            return returnedObject;
        }
        // An object is not within sight so return failure
        return null;


    }
    // Draw the line of sight representation within the scene window
    public void OnDrawGizmos()
    {
        DrawLineOfSight(this.transform, offset, fieldOfViewAngle, viewDistance, usePhysics2D);
    }
    public static void DrawLineOfSight(Transform transform, Vector3 positionOffset, float fieldOfViewAngle, float viewDistance, bool usePhysics2D)
    {
#if UNITY_EDITOR
        var oldColor = UnityEditor.Handles.color;
        var color = Color.yellow;
        color.a = 0.1f;
        UnityEditor.Handles.color = color;

        var halfFOV = fieldOfViewAngle * 0.5f;
        var beginDirection = Quaternion.AngleAxis(-halfFOV, (usePhysics2D ? Vector3.forward : Vector3.up)) * (usePhysics2D ? transform.up : transform.forward);
        UnityEditor.Handles.DrawSolidArc(transform.TransformPoint(positionOffset), (usePhysics2D ? transform.forward : transform.up), beginDirection, fieldOfViewAngle, viewDistance);

        UnityEditor.Handles.color = oldColor;
#endif
    }
}
}
