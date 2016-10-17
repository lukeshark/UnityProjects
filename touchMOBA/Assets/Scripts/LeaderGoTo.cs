using UnityEngine;
using System.Collections;
using AxlPlay;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class LeaderGoTo : MonoBehaviour {
    #region Public Variables
    public GameObject target;
    #endregion

    #region Private Variables
    Quaternion _angle;
    private NavMeshAgent _agent;
    #endregion
    // init FSM
    public enum States
    {
        Idle,
        GoTo,
        Finished

    }
    public StateMachine<States> fsm;
    #region Main Methods
    void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        //Initialize State Machine Engine		
        fsm = StateMachine<States>.Initialize(this);
    }
    // Use this for initialization
    void Start()
	{
		if (target == null){
			Debug.Log("Please set Target in the inspector => " + gameObject.name);
			
		}else{
			Invoke("GoToTarget", 8f);
		}
    }
    public void Update()
    {

    }
    #endregion

    #region  Utility Methodos
    public void GoToTarget()
    {
        _agent.SetDestination(target.transform.position);
        fsm.ChangeState(States.GoTo);
    }
    void GoTo_Enter() {

    }
    void GoTo_Update()
    {
        if (HasArrived())
        {
            fsm.ChangeState(States.Finished);
        }

    }
    void Finished_Enter() {

        NotificationCenter.DefaultCenter.PostNotification(this, "LeaderArrived");
    }
    private bool HasArrived()
    {
        return !_agent.pathPending && _agent.remainingDistance <= _agent.stoppingDistance + 0.001f;
    }

    #endregion

    #region  Editor Methodos
    public void OnDrawGizmos() {
#if UNITY_EDITOR
        Handles.ArrowCap(0, transform.position, transform.localRotation, 4f);
#endif
    }
    #endregion

}
