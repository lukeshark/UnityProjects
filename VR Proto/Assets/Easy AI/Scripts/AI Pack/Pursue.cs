using UnityEngine;
using System.Collections;
using AxlPlay;

namespace AxlPlay {
// Pursue the target specified using the Unity NavMesh.
[RequireComponent(typeof(UnityEngine.AI.NavMeshAgent))]
[AddComponentMenu("Easy AI/Pursue")]

public class Pursue : MonoBehaviour {

    [Tooltip("How far to predict the distance ahead of the target. Lower values indicate less distance should be predicated")]
    public float targetDistPrediction = 20;
    [Tooltip("Multiplier for predicting the look ahead distance")]
    public float targetDistPredictionMult = 20;
    [Tooltip("The GameObject that the agent is pursuing")]
    public GameObject target;
    [Tooltip("The agent has arrived when they are less than the specified distance")]
	public float arrivedDistance = 1f;
	[Tooltip("The Agent speed.")]
	public float AgentSpeed = 3.5f;
    // The position of the target at the last frame
	private Vector3 targetPosition;
	
	[HideInInspector]
	public Vector3 destination;
	
	[HideInInspector]
    public UnityEngine.AI.NavMeshAgent agent;
    // init FSM
    public enum States
    {
        Idle,
        Pursue,
        ArrivedEvent
    }
    public StateMachine<States> fsm;
    void Awake() {
	    agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
	    agent.speed = AgentSpeed;
	     //Initialize State Machine Engine		
	    fsm = StateMachine<States>.Initialize(this);
    }
    void Start()
    {
	    fsm.ChangeState(States.Idle);
    }
    void Idle_Enter() {
        
	    //Debug.Log("Pursue => Idle_Enter");
    }
    void Pursue_Enter() {
	    //Debug.Log("Pursue => Pursue_Enter");
        agent.Resume();
	    targetPosition = target.transform.position;
	    destination = Target();
	    agent.SetDestination(destination);
    } 
    void Pursue_Update() {

	    if (HasArrived())
            fsm.ChangeState(States.ArrivedEvent);
	    
	    destination = Target();
        // Target will return the predicated position
	    agent.SetDestination(destination);
    }
    void ArrivedEvent_Enter() {
        agent.Stop();
	    // Debug.Log("Pursue => ArrivedEvent");
    }
	
	public bool HasArrived()
	{
		Vector3 position = destination;
		position.y = 0;
		if(Vector3.Distance(transform.position,position) < 1f)
			return true;
		return false;
	}
	
	/*
	public bool HasArrived()
    {
        var direction = (agent.destination - transform.position);
        // Do not account for the y difference if it is close to zero.
        if (Mathf.Abs(direction.y) < 0.1f)
        {
            direction.y = 0;
        }
        return !agent.pathPending && direction.magnitude <= arrivedDistance;
    }
	*/
    // Predict the position of the target
    private Vector3 Target()
    {
        // Calculate the current distance to the target and the current speed
        var distance = (target.transform.position - transform.position).magnitude;
        var speed = agent.velocity.magnitude;

        float futurePrediction = 0;
        // Set the future prediction to max prediction if the speed is too small to give an accurate prediction
        if (speed <= distance / targetDistPrediction)
        {
            futurePrediction = targetDistPrediction;
        }
        else
        {
            futurePrediction = (distance / speed) * targetDistPredictionMult; // the prediction should be accurate enough
        }

        // Predict the future by taking the velocity of the target and multiply it by the future prediction
        var prevTargetPosition = targetPosition;
        targetPosition = target.transform.position;
        return targetPosition + (targetPosition - prevTargetPosition) * futurePrediction;
    }
}
}
