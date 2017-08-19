using UnityEngine;
using System.Collections;
using AxlPlay;

namespace AxlPlay {
[RequireComponent(typeof(UnityEngine.AI.NavMeshAgent))]
//Patrol around the specified waypoints using the Unity NavMesh.
[AddComponentMenu("Easy AI/Patrol")]

public class Patrol : MonoBehaviour
{

    [Tooltip("Should the agent patrol the waypoints randomly?")]
    public bool randomPatrol = false;
    [Tooltip("The length of time that the agent should pause when arriving at a waypoint")]
    public float waypointPauseDuration = 0f;
    [Tooltip("The Agent speed.")]
    public float AgentSpeed = 3.5f;
    [Tooltip("The waypoints to move to")]
    public GameObject[] waypoints;

    // The current index that we are heading towards within the waypoints array
    private int waypointIndex;
    private float waypointReachedTime;
    private float arrivedDistance = 1f;
    private UnityEngine.AI.NavMeshAgent agent;
    // init FSM
    public enum States
    {
        Idle,
        Patrol,
        Finish
    }
    public StateMachine<States> fsm;
    void Awake()
    {
	    agent = GetComponent<UnityEngine.AI.NavMeshAgent>();

        agent.speed = AgentSpeed;
        //Initialize State Machine Engine		
        fsm = StateMachine<States>.Initialize(this);
    }
    void Patrol_Enter()
    {
        agent.Resume();
        // initially move towards the closest waypoint
        float distance = Mathf.Infinity;
        float localDistance;
        for (int i = 0; i < waypoints.Length; ++i)
        {
            if ((localDistance = Vector3.Magnitude(transform.position - waypoints[i].transform.position)) < distance)
            {
                distance = localDistance;
                waypointIndex = i;
            }
        }
        waypointReachedTime = -1;
        agent.SetDestination(Target());
    }
    void Patrol_Update()
    {
        if (HasArrived())
        {
            if (waypointReachedTime == -1)
            {
                waypointReachedTime = Time.time;
            }


            // wait the required duration before switching waypoints.
            if (waypointReachedTime + waypointPauseDuration <= Time.time)
            {
                if (randomPatrol)
                {
                    if (waypoints.Length == 1)
                    {
                        waypointIndex = 0;
                    }
                    else
                    {
                        // prevent the same waypoint from being selected
                        var newWaypointIndex = waypointIndex;
                        while (newWaypointIndex == waypointIndex)
                        {
                            newWaypointIndex = Random.Range(0, waypoints.Length);
                        }
                        waypointIndex = newWaypointIndex;
                    }
                }
                else
                {
                    waypointIndex = (waypointIndex + 1) % waypoints.Length;
                }
                agent.SetDestination(Target());
                waypointReachedTime = -1;
            }
        }

    }
    void Finish_Enter()
	{
		agent.Stop();
		// Debug.Log("Patrol => Finish_Enter");
    }
    // Return the current waypoint index position
    private Vector3 Target()
    {
        if (waypointIndex >= waypoints.Length)
        {
            return transform.position;
        }
        return waypoints[waypointIndex].transform.position;
    }

    bool HasArrived()
    {
        var direction = (agent.destination - transform.position);
        // Do not account for the y difference if it is close to zero.
        if (Mathf.Abs(direction.y) < 0.1f)
        {
            direction.y = 0;
        }
        return !agent.pathPending && direction.magnitude <= arrivedDistance;
    }

    // Draw a gizmo indicating a patrol 
    public void OnDrawGizmos()
    {
#if UNITY_EDITOR
        if (waypoints == null)
        {
            return;
        }
        var oldColor = UnityEditor.Handles.color;
        UnityEditor.Handles.color = Color.yellow;
        for (int i = 0; i < waypoints.Length; ++i)
        {
            if (waypoints[i] != null)
            {
                UnityEditor.Handles.SphereCap(0, waypoints[i].transform.position, waypoints[i].transform.rotation, 1);
            }
        }
        UnityEditor.Handles.color = oldColor;
#endif
    }
}
}
