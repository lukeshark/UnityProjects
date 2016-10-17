using UnityEngine;
using System.Collections;
using AxlPlay;

public class EnemySearch : MonoBehaviour {

    public TextMesh txt;
    public GameObject target;

    private Search _search;
    private Animator _anim;
    private Pursue _pursue;

    private EnemyShoot _shoot;
    // init FSM
    public enum States
    {
        Idle,
        Search,
        Pursue,
        PursueWait,
        Attack,
        Finish

    }
    public StateMachine<States> fsm;
    void Awake() {
        _search = GetComponent<Search>();
        _anim = GetComponent<Animator>();
        _pursue = GetComponent<Pursue>();
        _shoot = GetComponentInChildren<EnemyShoot>();

    }
    // Use this for initialization
    void Start () {
        //Initialize State Machine Engine		
        fsm = StateMachine<States>.Initialize(this, States.Idle);
    }
    IEnumerator Idle_Enter()
    {
        _anim.SetBool("Run", false);
        txt.text = "Idle_Enter";
        yield return new WaitForSeconds(1f);
        fsm.ChangeState(States.Search);
    }
    void Search_Enter() {
        txt.text = "Search_Enter";
        _anim.SetBool("Run", true);
        _search.startSearch();
    }
    void Search_Update() {
        target = _search.goSearch();
        if (target != null)
        {
            
            fsm.ChangeState(States.Pursue, StateTransition.Overwrite);
        }
    }
    void Pursue_Enter()
    {
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
        Debug.Log("Pursue_OnTriggerEnter");
        if (other.tag == "Player")
        {
            txt.text = "Pursue_ArrivedEvent";
            _anim.SetBool("Run", false);
            _pursue.fsm.ChangeState(Pursue.States.ArrivedEvent);
        }
    }
    void Pursue_OnTriggerExit(Collider other)
    {
        Debug.Log("Pursue_OnTriggerExit");
        if (other.tag == "Player")
        {
            _anim.SetBool("Run", true);
            fsm.ChangeState(States.PursueWait);
        }
    }
    private void RotateTowards(Transform target)
    {
        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    }
    IEnumerator PursueWait_Enter()
    {
        Debug.Log("PursueWait_Enter");
        yield return new WaitForSeconds(2f);
        fsm.ChangeState(States.Search, StateTransition.Overwrite);
    }
}
