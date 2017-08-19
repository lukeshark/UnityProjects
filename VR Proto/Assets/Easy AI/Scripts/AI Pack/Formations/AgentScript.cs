using UnityEngine;
using System.Collections;
using AxlPlay;
#if UNITY_5_5_OR_NEWER
using UnityEngine.AI;
#endif
[AddComponentMenu ("Easy AI/Formation/Agent")]
public class AgentScript : MonoBehaviour
{
	#region Public Variables
	
	public FormationManager _manager;
	[Tooltip("(Animator) Set the name of the bool variable for walk")]
	public string animBoolWalk = "";
	
	// position in formation of the agent
	[HideInInspector]
	public int index;


	[HideInInspector]
	public bool isLeader;

	[Tooltip ("For test purposes")]
	public bool isDead;

	#endregion

	#region Private Variables

	private Agents _agent;

	private Vector3 target;
	[HideInInspector]
	public NavMeshAgent navMeshAgent;

	private Animator _anim;

	#endregion

	// init FSM
	public enum States
	{
		Idle,
		goPos,
		inPos,
		goLeader,
		LeaderArrived,
		goToLastLeaderPos,
		Dead
	}

	public StateMachine<States> fsm;

	private int notificationCount = 0;

	void Awake ()
	{
		
		
		notificationCount = 0;
		navMeshAgent = GetComponent<NavMeshAgent> ();

		_anim = GetComponent<Animator> ();

		//Initialize State Machine Engine		
		fsm = StateMachine<States>.Initialize (this);
	}

	void OnEnable ()
	{
		notificationCount = 0;
	}

	void Start ()
	{
		if (gameObject.tag.Contains ("Untagged")) {
			Debug.Log ("You need to set the tag to the agent. => " + gameObject.name);
			return;
		}
		// create a notification center delegate
		NotificationCenter.DefaultCenter.AddObserver (this, "AgentInPosition");

		NotificationCenter.DefaultCenter.AddObserver (this, "LeaderArrived");

		NotificationCenter.DefaultCenter.AddObserver (this, "agentIsDead");

		//Initialize State Machine Engine		
		fsm.ChangeState (States.Idle);
	}

	void Update ()
	{

		if (_anim != null) {
			if (navMeshAgent.velocity.z <= 0.6f && navMeshAgent.velocity.x <= 0.6f) {
				if (!string.IsNullOrEmpty( animBoolWalk))
					_anim.SetBool (animBoolWalk, false);
			} else {
				if (!string.IsNullOrEmpty( animBoolWalk))
					_anim.SetBool (animBoolWalk, true);
			}
		}

	}

	IEnumerator Idle_Enter ()
	{

		// wait for create formation  => AgentManager
		yield return new WaitForSeconds (2f);

		index = getMyIndex ();


		// is 0 is Leader
		if (index == 0) {
			navMeshAgent.speed = _manager.formationSpeed;

		}
		//  si no hay agentes en el manager no hacer nada
		if (_manager.agentsNum > 0 && isLeader == false)
			fsm.ChangeState (States.goPos);

	}

	void goPos_Update ()
	{
		target = _manager.GetPosition (index);
		navMeshAgent.SetDestination (target);
		if (HasArrived ()) {
			fsm.ChangeState (States.inPos);
		}
		if (isDead) {
			fsm.ChangeState (States.Dead);
		}
	}
	// already in position move formation to target
	void inPos_Enter ()
	{
		NotificationCenter.DefaultCenter.PostNotification (this, "AgentInPosition");
	}

	void inPos_Update ()
	{
		target = _manager.GetPosition (index);
		navMeshAgent.SetDestination (target);
		if (isDead) {
			fsm.ChangeState (States.Dead);
		}
	}
	// this is for future release
	void goLeader_Enter ()
	{
		navMeshAgent.SetDestination (_manager.target.transform.position);
		Debug.Log ("goLeader_Enter");
	}

	void goLeader_Update ()
	{
		if (isDead) {
			fsm.ChangeState (States.Dead);
		}
		if (HasArrived ()) {
			fsm.ChangeState (States.LeaderArrived);
		}

	}

	void LeaderArrived_Enter ()
	{
		NotificationCenter.DefaultCenter.PostNotification (this, "LeaderArrived");
	}

	void goToLastLeaderPos_Enter ()
	{

		navMeshAgent.SetDestination (_manager.LastLeaderPos);

	}

	void goToLastLeaderPos_Update ()
	{
		if (HasArrived ()) {
			fsm.ChangeState (States.goLeader);

		}
	}

	void Dead_Enter ()
	{
		navMeshAgent.Stop ();
		if (isDead) {
			if (isLeader) {
				_manager.LastLeaderPos = transform.position;
			}
			_manager.LoadAgentList ();
			NotificationCenter.DefaultCenter.PostNotification (this, "agentIsDead");

		}

	}
	// call when agent is in position
	void AgentInPosition (NotificationCenter.Notification note)
	{
		notificationCount++;
		if (notificationCount == _manager.agentsNum - 1) {

			Debug.Log ("All in position.");

			if (isLeader) {
				fsm.ChangeState (States.goLeader);
			}

		}

	}

	void agentIsDead ()
	{
		index = getMyIndex ();

		if (index == 0) {

			Renderer rend = GetComponent<Renderer> ();

			rend.material.color = Color.red;
			navMeshAgent.speed = _manager.formationSpeed;

			fsm.ChangeState (States.goLeader, StateTransition.Overwrite);
		}
	}

	void LeaderArrived ()
	{
		Debug.Log ("Leader Arrived => All agent waiting for order ex: shoot target ...");
	}

	private int getMyIndex ()
	{

		// find my index
		foreach (var item in _manager.listAgents) {
			// assign a position to the agent
			if (item.go == gameObject) {
				index = item.idAgent;
				return index;
			}
		}
		return -1;

	}

	private bool HasArrived ()
	{
		return !navMeshAgent.pathPending && navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance + 0.001f;
	}

}


