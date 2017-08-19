using UnityEngine;
using System.Collections;
using AxlPlay;

public class enemyPatrol : MonoBehaviour {

    public TextMesh txt;
    public GameObject target;
    private Patrol _patrol;
    private Animator _anim;
    private CanSeeObject canSee;
    private Pursue _pursue;
    private EnemyShoot _shoot;
    // init FSM
    public enum States
    {
        Idle,
        Patrol,
        Pursue,
        PatrolWait,
        Finish

    }
    public StateMachine<States> fsm;
    void Awake() {
        _patrol = GetComponent<Patrol>();
        _anim = GetComponent<Animator>();
        _pursue = GetComponent<Pursue>();
        canSee = GetComponent<CanSeeObject>();
	    _shoot = GetComponentInChildren<EnemyShoot>();
	    //Initialize State Machine Engine		
	    fsm = StateMachine<States>.Initialize(this);
    }
    void Start()
    {
         	
	    fsm.ChangeState(States.Idle);
       
    }
    IEnumerator Idle_Enter()
    {
        txt.text = "Idle_Enter";
        yield return new WaitForSeconds(0.5f);
        fsm.ChangeState(States.Patrol);
    }
    void Patrol_Enter() {
        _anim.SetBool("Run", true);
        txt.text = "Patrol_Enter";
        _patrol.fsm.ChangeState(Patrol.States.Patrol);

    }
    void Patrol_Update() {
        if (canSee.CanSee() != null)
        {
            target = canSee.CanSee();
            fsm.ChangeState(States.Pursue, StateTransition.Overwrite);
        }
    }
    void Patrol_Exit() {
        _anim.SetBool("Run", false);

    }
    void Pursue_Enter() {
        _patrol.fsm.ChangeState(Patrol.States.Idle);
        _pursue.target = target;
        _pursue.fsm.ChangeState(Pursue.States.Pursue);
        _anim.SetBool("Run", true);
        txt.text = "Pursue_Enter";

    }
    void Pursue_Update()
    {
        RotateTowards(target.transform);
        _shoot.shoothing = true;
    }
    void Pursue_Exit() {
        _shoot.shoothing = false;
    }
    void Pursue_OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
             txt.text = "Pursue_ArrivedEvent";
            _anim.SetBool("Run", false);
            _pursue.fsm.ChangeState(Pursue.States.ArrivedEvent,StateTransition.Overwrite);
        }
    }
    void Pursue_OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            _anim.SetBool("Run", true);
            fsm.ChangeState(States.PatrolWait);
        }
    }
    private void RotateTowards(Transform target)
    {
        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    }
    IEnumerator PatrolWait_Enter() {
        _anim.SetBool("Run", false);
        Debug.Log("PatrolWait_Enter");
        yield return new WaitForSeconds(2f);
        fsm.ChangeState(States.Patrol,StateTransition.Overwrite);
    }
}
