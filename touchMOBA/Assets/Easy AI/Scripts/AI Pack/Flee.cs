using UnityEngine;
using System.Collections;
using AxlPlay;
//Flee from the target specified using the Unity NavMesh
[RequireComponent(typeof(NavMeshAgent))]
[AddComponentMenu("Easy AI/Flee")]

public class Flee : MonoBehaviour {

    [Tooltip("The agent has fleed when the magnitude is greater than this value")]
    public float fleedDistance = 10;
    [Tooltip("The distance to look ahead when fleeing")]
    public float lookAheadDistance = 5;
    [Tooltip("The agent has arrived when they are less than the specified distance")]
    public float arrivedDistance = 2;

    [Tooltip("The GameObject that the agent is fleeing from")]
    public GameObject target;
    [Tooltip("The Agent speed.")]
    public float AgentSpeed = 3.5f;
    [Tooltip("Stopping Distance.")]
    [Range(0.1f, 100)]
    public float StoppingDistance = 0.1f;
    [HideInInspector]
    public NavMeshAgent agent;
    // init FSM
    public enum States
    {
        Idle,
        Flee,
        Finish
    }
    public StateMachine<States> fsm;
    void Awake() {

	    agent = GetComponent<NavMeshAgent>();
        agent.speed = AgentSpeed;
        agent.stoppingDistance = StoppingDistance;
        if (target == null) {
		    Debug.Log ("The target is empty, Add the target => " + gameObject.name );
		    
	    }
        //Initialize State Machine Engine	
        fsm = StateMachine<States>.Initialize(this);
    }
    void Start() {
       	
        fsm.ChangeState(States.Idle);
    }
    void Idle_Enter()
    {
        Debug.Log("Flee => Idle_Enter");
        fsm.ChangeState(States.Flee);
    }
    void Flee_Update() {

        if (Vector3.Magnitude(transform.position - target.transform.position) > fleedDistance)
        {
            // finish event
            fsm.ChangeState(States.Finish);
        }
        SetDestination(Target());
    }
    void Finish_Enter() {
        Debug.Log("Flee => Finish_Enter");
    }
    // Flee in the opposite direction
    private Vector3 Target()
    {
        return transform.position + (transform.position - target.transform.position).normalized * lookAheadDistance;
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

}
