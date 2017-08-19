using UnityEngine;
using System.Collections;
using AxlPlay;

[RequireComponent(typeof(UnityEngine.AI.NavMeshAgent))]
//Search for a target by combining the wander, within hearing range, and the within seeing range tasks using the Unity NavMesh.v
[AddComponentMenu("Easy AI/Search")]

public class Search : MonoBehaviour
{

    [Tooltip("How far ahead of the current position to look ahead for a wander")]
    public float wanderDistance = 10;
    [Tooltip("The amount that the agent rotates direction")]
    public float wanderRate = 1;
    [Tooltip("The field of view angle of the agent (in degrees)")]
    public float fieldOfViewAngle = 90;
    [Tooltip("The distance that the agent can see")]
	public float viewDistance = 25f;

    public bool senseAudio = true;
    [Tooltip("How far away the unit can hear")]
	public float hearingRadius = 10f;
    [Tooltip("The offset relative to the pivot position")]
    public Vector3 offset;
    [Tooltip("The target offset relative to the pivot position")]
    public Vector3 targetOffset;
    [Tooltip("The LayerMask of the objects that we are searching for")]
    public LayerMask objectLayerMask;

    [Tooltip("The further away a sound source is the less likely the agent will be able to hear it. " +
        "Set a threshold for the the minimum audibility level that the agent can hear")]
    public float audibilityThreshold = 0.05f;
    [Tooltip("The object that is found")]
    public GameObject returnedObject;
	[Tooltip("The Agent speed.")]
	public float AgentSpeed = 3.5f;
    private UnityEngine.AI.NavMeshAgent agent;
	
	// init FSM
	public enum States
	{
		Idle,
		Search,
		Finish
	}
	public StateMachine<States> fsm;
    void Awake()
    {
	    agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
	    agent.speed = AgentSpeed;
	    fsm = StateMachine<States>.Initialize(this);
    }
	void Seach_Enter(){
		agent.Resume();
		SetDestination(Target());
		
	}
	void Search_Update(){
		goSearch();
		
	}
	public void startSearch() {
        agent.Resume();
        SetDestination(Target());
    }
    public GameObject goSearch()
    {
         if (HasArrived())
        {
            SetDestination(Target());
        }
        // Detect if any objects are within sight
	    returnedObject = MovementUtility.WithinSight(transform, offset, fieldOfViewAngle, viewDistance, objectLayerMask, targetOffset, -1);

        if (returnedObject != null)
        {
            return returnedObject;
        }
        // Detect if any object are within audio range (if enabled)
        if (senseAudio)
        {
            returnedObject = MovementUtility.WithinHearingRange(transform, offset, audibilityThreshold, hearingRadius, objectLayerMask);

            if (returnedObject != null)
            {
                return returnedObject;
            }
        }
        return returnedObject;
    }
    private Vector3 Target()
    {
        // point in a new random direction and then multiply that by the wander distance
        var direction = transform.forward + Random.insideUnitSphere * wanderRate;
        return transform.position + direction.normalized * wanderDistance;
    }
    private bool SetDestination(Vector3 target)
    {
        if (agent.destination == target)
        {
            return true;
        }
        if (agent.SetDestination(target))
        {
            return true;
        }
        return false;
    }
    private bool HasArrived()
    {
        return !agent.pathPending && agent.remainingDistance <= agent.stoppingDistance + 0.001f;
    }
  // Draw the hearing radius
	public void OnDrawGizmos()
	{
		#if UNITY_EDITOR
		var color = Color.green;
		color.a = 0.1f;
		UnityEditor.Handles.color = color;
		
		var halfFOV = fieldOfViewAngle * 0.5f;
		var beginDirection = Quaternion.AngleAxis(-halfFOV, (Vector3.up)) * ( transform.forward);
		UnityEditor.Handles.DrawSolidArc(transform.TransformPoint(offset), ( transform.up), beginDirection, 360f, hearingRadius);
		
		#endif
		
		DrawLineOfSight(this.transform, offset, fieldOfViewAngle, viewDistance, false);

	}
 
	public static void DrawLineOfSight(Transform transform, Vector3 positionOffset, float fieldOfViewAngle, float viewDistance, bool usePhysics2D)
	{
#if UNITY_EDITOR
		Color oldColor = UnityEditor.Handles.color;
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
