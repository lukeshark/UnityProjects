using UnityEngine;
using AxlPlay;
using System.Collections;

namespace AxlPlay {
//Evade the target specified using the Unity NavMesh.
[RequireComponent(typeof(UnityEngine.AI.NavMeshAgent))]
[AddComponentMenu("Easy AI/Evade")]

public class Evade : MonoBehaviour {

    [Tooltip("The agent has evaded when the magnitude is greater than this value")]
    public float evadeDistance = 10;
    [Tooltip("The distance to look ahead when evading")]
    public float lookAheadDistance = 5;
    [Tooltip("How far to predict the distance ahead of the target. Lower values indicate less distance should be predicated")]
    public float targetDistPrediction = 20;
    [Tooltip("Multiplier for predicting the look ahead distance")]
    public float targetDistPredictionMult = 20;
    [Tooltip("The GameObject that the agent is evading")]
	public GameObject target;
	[Tooltip("The Agent speed.")]
	public float AgentSpeed = 3.5f;
	[Tooltip("Stopping Distance.")]
    [Range(0.1f,100)]
	public float StopDistance = 0.1f;
    // The position of the target at the last frame
    private Vector3 targetPosition;
	private UnityEngine.AI.NavMeshAgent agent;
	// used in Easy AI Base
	[HideInInspector]
	public bool hasArrived;
    // init FSM
    public enum States
    {
        Idle,
        Evade,
        Finish
    }
    public StateMachine<States> fsm;
    void Awake() {

	    agent = GetComponent<UnityEngine.AI.NavMeshAgent>();

	    agent.speed = AgentSpeed;
	    agent.stoppingDistance = StopDistance;


	    if (agent.stoppingDistance <= 0) {
		    Debug.Log ("The Nav Mesh Agent Stopping Distance cannot be less or equal than 0");
	    }
	    
	    if (target == null) {
		    Debug.Log ("The target is empty, Add the target => " + gameObject.name );  
	    }
		  //Initialize State Machine Engine		
	    fsm = StateMachine<States>.Initialize(this);
	    
    }
    void Idle_Enter() {
	    hasArrived = false;
	    //Debug.Log("Evade => Idle_Enter");

    }
	void Evade_Enter(){
		targetPosition = target.transform.position;
		agent.SetDestination(Target());
	}
    // Evade from the target. 
    void Evade_Update()
    {
        if (Vector3.Magnitude(transform.position - target.transform.position) > evadeDistance)
        {
            // finish event
            fsm.ChangeState(States.Finish);
        }

        agent.SetDestination(Target());

    }
    void Finish_Enter() {
	    hasArrived = true;
	    //Debug.Log("Evade => Finish_Enter");
    }

    // Evade in the opposite direction
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
        var position = targetPosition + (targetPosition - prevTargetPosition) * futurePrediction;

        return transform.position + (transform.position - position).normalized * lookAheadDistance;
    }


}
}
