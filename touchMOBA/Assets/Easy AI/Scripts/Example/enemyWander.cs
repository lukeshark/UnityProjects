using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using AxlPlay;

public class enemyWander : MonoBehaviour {

    public GameObject target;
    public TextMesh txt;

    private Pursue _pursue;
    private Wander _wander;
    private CanSeeObject canSee;
    private float timer;
    private Animator _anim;
    private EnemyShoot _shoot;
    // init FSM
    public enum States
    {
        Idle,
        Wander,
        Chase,
        PursueWait,
        Finish

    }
    public StateMachine<States> fsm;
    void Awake()
    {
        _wander = GetComponent<Wander>();
        _pursue = GetComponent<Pursue>();
        canSee = GetComponent<CanSeeObject>();
        _anim = GetComponent<Animator>();
        _shoot = GetComponentInChildren<EnemyShoot>();
    }
    void Start() {
        //Initialize State Machine Engine		
        fsm = StateMachine<States>.Initialize(this, States.Idle);
    }
    IEnumerator Idle_Enter() {
        txt.text = "Idle_Enter";
        _anim.SetBool("Run", false);
        yield return new WaitForSeconds(0.5f);
        fsm.ChangeState(States.Wander);
    }
    void Wander_Enter() {
        txt.text = "Wander_Enter";
        _anim.SetBool("Run", true);
        _wander.fsm.ChangeState(Wander.States.Wander, StateTransition.Overwrite);

    }
    void Wander_Update() {
        if (canSee.CanSee() != null)
        {
            target = canSee.CanSee();
            fsm.ChangeState(States.Chase, StateTransition.Overwrite);
        }
    }
    void Wander_OnTriggerEnter(Collider other) {

        if (other.tag == "Player")
        {
            target = other.gameObject;
            fsm.ChangeState(States.Chase);
        }
    }
    void Wander_Exit() {
        _anim.SetBool("Run", false);
    }
    void Chase_Enter() {
        // time of pursue the target
        timer = 0;
 
        _wander.fsm.ChangeState(Wander.States.Idle);
        _pursue.target = target;
        _pursue.fsm.ChangeState(Pursue.States.Pursue);
        _anim.SetBool("Run", true);
        txt.text = "Pursue_Enter";
    
    }
    void Chase_Update() {

        RotateTowards(target.transform);

        _shoot.shoothing = true;
    }
    void Chase_Exit() {
        _shoot.shoothing = false;
    }
    void Chase_OnTriggerEnter(Collider other) {
 
        if (other.tag == "Player") {
            txt.text = "Pursue_ArrivedEvent";
            _anim.SetBool("Run", false);
            _pursue.fsm.ChangeState(Pursue.States.ArrivedEvent);
        }
    }
    void Chase_OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            txt.text = "Pursue_Enter";
            _anim.SetBool("Run", true);
            _pursue.fsm.ChangeState(Pursue.States.Pursue);
        }
    }
    IEnumerator PursueWait_Enter() {

        yield return new WaitForSeconds(1f);
        fsm.ChangeState(States.Chase);

    }
    private void RotateTowards(Transform target)
    {
        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    }

}
