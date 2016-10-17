using UnityEngine;
using System.Collections;
using AxlPlay;

public class Team1vsPlayer : MonoBehaviour {

    public float wanderDistance = 20;
    public float wanderRate = 2;
    public LayerMask layerMask;

    public GameObject target;
    private GameObject node;

    private NavMeshAgent nav;
    private Animator anim;
    private float rotationSpeed;
    // private CanSeeObject canSee;

    private EnemyShoot _shoot;

    public bool inCover;
    public bool isEngaged;
    public bool haveTarget;

    public TextMesh txt;
    private CanSeeObject canSee;
    // init FSM
    public enum States
    {
        Idle,
        getCover,
        goToCover,
        inCover,
        Wander,
        Attack,
        FindTarget
    }
    public StateMachine<States> fsm;
    void Awake()
    {
        rotationSpeed = 5f;
        _shoot = GetComponentInChildren<EnemyShoot>();
        nav = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        canSee = GetComponent<CanSeeObject>();
        //Initialize State Machine Engine		
        fsm = StateMachine<States>.Initialize(this, States.Idle);

    }
    IEnumerator Idle_Enter()
    {

        yield return new WaitForSeconds(0.5f);

        txt.text = "Idle_Enter";
        fsm.ChangeState(States.getCover);

    }
    void getCover_Enter()
    {
        txt.text = "getCover_Enter";
        node = CoverNodeManager.current.FindClosestTarget(gameObject.transform.position, tag);


        if (node != null)
        {
            // Debug.Log(node);
            // enviar al nodo que esta asignado
            CoverNode temp = node.GetComponent<CoverNode>();
            if (temp != null)
            {
                temp.setOccupied(true);
                fsm.ChangeState(States.goToCover);
            }

        }
        else
        {
            fsm.ChangeState(States.Wander);
        }
    }
    void goToCover_Enter()
    {
        txt.text = "goToCover_Enter";
        if (inCover == false)
        {
            nav.Resume();
            anim.SetBool("Crouch", false);
            anim.SetBool("Run", true);
            if (node != null)
                nav.SetDestination(node.transform.position);
        }
        else
        {

        }

    }
    void goToCover_Update()
    {
        if (HasArrived())
        {
            inCover = true;
            fsm.ChangeState(States.inCover);
        }
        else
        {
          
        }

    }
    void inCover_Enter()
    {

        anim.SetBool("Run", false);
        anim.SetBool("Crouch", true);
        // target = TeamManager.current.FindClosestTargetTeam(gameObject.transform.position, tag);


        if (target != null)
        {

           // fsm.ChangeState(States.Attack);
            Debug.Log("Attack");
        }
    }
    bool HasArrived()
    {
        return !nav.pathPending && nav.remainingDistance <= nav.stoppingDistance + 0.001f;
    }
    private void RotateTowards(Transform target)
    {
        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
    }
    private Vector3 Target()
    {
        // point in a new random direction and then multiply that by the wander distance
        var direction = transform.forward + Random.insideUnitSphere * wanderRate;
        return transform.position + direction.normalized * wanderDistance;
    }
    bool SetDestination(Vector3 target)
    {
        if (nav.destination == target)
        {
            return true;
        }
        if (nav.SetDestination(target))
        {
            return true;
        }
        return false;
    }
}
