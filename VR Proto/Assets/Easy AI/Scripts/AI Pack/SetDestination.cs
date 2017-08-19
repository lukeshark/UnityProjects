using UnityEngine;
using System.Collections;
using AxlPlay;

namespace AxlPlay {
[RequireComponent(typeof(UnityEngine.AI.NavMeshAgent))]
[AddComponentMenu("Easy AI/Go To Destination")]
public class SetDestination : MonoBehaviour {

    public GameObject target;
	public float arrivedDistance = 0.1f;
	[Tooltip("The Agent speed.")]
	public float AgentSpeed = 3.5f;
	
	[HideInInspector]
    public UnityEngine.AI.NavMeshAgent agent;
    private Vector3 localitation;

	private GameObject _gameObject;
	// used in Easy AI Base
	[HideInInspector]
	public bool hasArrived;
    // init FSM
    public enum States
    {
        goDestination,
        ArrivedEvent
    }
    public StateMachine<States> fsm;
    void Awake() {
	    agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
	    agent.speed = AgentSpeed;
	    agent.stoppingDistance = arrivedDistance;
	    //Initialize State Machine Engine		
	    fsm = StateMachine<States>.Initialize(this);
	    
    }
	void Start(){
		hasArrived = false;
		
	}

	void goDestination_Enter() {
		agent.Resume();
	 agent.SetDestination(target.transform.position);
	}
    void goDestination_Update() {

        if (HasArrived())
        {
            fsm.ChangeState(States.ArrivedEvent);
        }

    }
	void ArrivedEvent_Enter() {
		
	    hasArrived = true;
    }
	
    private bool HasArrived()
    {
        return !agent.pathPending && agent.remainingDistance <= agent.stoppingDistance + 0.001f;
    }
}
}
