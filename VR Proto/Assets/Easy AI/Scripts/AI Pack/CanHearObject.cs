using UnityEngine;
using System.Collections;
using AxlPlay;

namespace AxlPlay {
//Check to see if the any objects are within hearing range of the current agent.
	[AddComponentMenu("Easy AI/Can Hear Object")]
public class CanHearObject : MonoBehaviour
{
    [Tooltip("Should the 2D version be used?")]
    public bool usePhysics2D;

    [Tooltip("The object that we are searching for. If this value is null then the objectLayerMask will be used")]
    public GameObject targetObject;

	[Tooltip("The tag of the object that we are searching for")]
	
    public string targetTag;

    [Tooltip("The LayerMask of the objects that we are searching for")]
    public LayerMask objectLayerMask;

    [Tooltip("How far away the unit can hear")]
    public float hearingRadius = 5f;

    [Tooltip("The further away a sound source is the less likely the agent will be able to hear it. " +
        "Set a threshold for the the minimum audibility level that the agent can hear")]
    public float audibilityThreshold = 0.05f;

    [Tooltip("The offset relative to the pivot position")]
    public Vector3 offset;

    [Tooltip("The returned object that is heard")]
    public GameObject returnedObject;

 
    public GameObject Hear()
	{
        // If the target object is null then determine if there are any objects within hearing range based on the layer mask
        if (targetObject == null)
        {
            if (usePhysics2D)
            {
                returnedObject = MovementUtility.WithinHearingRange2D(transform, offset, audibilityThreshold, hearingRadius, objectLayerMask);
            }
            else
            {

                returnedObject = MovementUtility.WithinHearingRange(transform, offset, audibilityThreshold, hearingRadius, objectLayerMask);
            }

        }
        else
        {
	        GameObject target = null;
	        
	        if (!string.IsNullOrEmpty(targetTag) && !targetTag.Contains("Untagged"))
            {
                target = GameObject.FindGameObjectWithTag(targetTag);
            }
            else
            {
                target = targetObject;
            }
            if (target == null) {
                target = targetObject;
            }
            if (Vector3.Distance(target.transform.position, transform.position) < hearingRadius)
            {
                returnedObject = MovementUtility.WithinHearingRange(transform, offset, audibilityThreshold, targetObject);
            }
        }
        if (returnedObject != null)
        {

            return returnedObject;

        }
        else {

            return null;
        }


    }
    // Draw the hearing radius
    public void OnDrawGizmos()
    {
#if UNITY_EDITOR
        var oldColor = UnityEditor.Handles.color;
        var color = Color.green;
        color.a = 0.1f;
        UnityEditor.Handles.color = color;
	    float fieldOfViewAngle = 360f;
        var halfFOV = fieldOfViewAngle * 0.5f;
        var beginDirection = Quaternion.AngleAxis(-halfFOV, (usePhysics2D ? Vector3.forward : Vector3.up)) * (usePhysics2D ? transform.up : transform.forward);
        UnityEditor.Handles.DrawSolidArc(transform.TransformPoint(offset), (usePhysics2D ? transform.forward : transform.up), beginDirection, fieldOfViewAngle, hearingRadius);

        UnityEditor.Handles.color = oldColor;
#endif
	    
    }
 
}
}
