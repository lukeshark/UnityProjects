using UnityEngine;
using System.Collections;
using AxlPlay;

// Wander using Unity NavMesh
[RequireComponent(typeof(NavMeshAgent))]
[AddComponentMenu("Easy AI/Wander")]
public class Wander : MonoBehaviour
{
    [Tooltip("How far ahead of the current position to look ahead for a wander")]
    public float wanderDistance = 20;
    [Tooltip("The amount that the agent rotates direction")]
    public float wanderRate = 2;
    [Tooltip("The Agent speed.")]
    public float AgentSpeed = 3.5f;

    private NavMeshAgent navMeshAgent;
    // init FSM
    public enum States
    {
        Idle,
        Wander,
        Finish
    }
    public StateMachine<States> fsm;
    void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.speed = AgentSpeed;
        //Initialize State Machine Engine		
        fsm = StateMachine<States>.Initialize(this);
    }
    void Start()
    {	
        fsm.ChangeState( States.Wander);
    }
    void Idle_Enter() {

        Debug.Log("Wander => Idle_Enter");
    }
    void Wander_Enter()
    { 
        SetDestination(Target());
    }
    void Wander_Update()
    {
        if (HasArrived())
        {
            SetDestination(Target());
        }
    }
    void Finish_Enter() {

        Debug.Log("Wander => Finish_Enter");
    }
    private Vector3 Target()
    {
        // point in a new random direction and then multiply that by the wander distance
        var direction = transform.forward + Random.insideUnitSphere * wanderRate;
        return transform.position + direction.normalized * wanderDistance;
    }
    private bool SetDestination(Vector3 target)
    {
        if (navMeshAgent.destination == target)
        {
            return true;
        }
        if (navMeshAgent.SetDestination(target))
        {
            return true;
        }
        return false;
    }

    private bool HasArrived()
    {
        return !navMeshAgent.pathPending && navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance + 0.001f;
    }

}
