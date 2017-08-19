using UnityEngine;
using System.Collections;
using AxlPlay;

public class Team2 : MonoBehaviour
{

    public float wanderDistance = 20;
    public float wanderRate = 2;
    public LayerMask layerMask;

    private UnityEngine.AI.NavMeshAgent nav;
    private Animator anim;
    private float rotationSpeed;

    private GameObject node;
    public GameObject target;
    private EnemyShoot _shoot;


    public bool inCover;
    public bool isEngaged;
    public bool haveTarget;

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

    // Use this for initialization
    void Awake()
    {
        rotationSpeed = 25f;
        _shoot = GetComponentInChildren<EnemyShoot>();
        nav = GetComponent<UnityEngine.AI.NavMeshAgent>();
        anim = GetComponent<Animator>();

        //Initialize State Machine Engine		
        fsm = StateMachine<States>.Initialize(this, States.Idle);

    }
    IEnumerator Idle_Enter()
    {

        yield return new WaitForSeconds(0.5f);

        fsm.ChangeState(States.getCover);

    }
    void getCover_Enter()
    {

        node = CoverNodeManager.current.FindClosestTarget(gameObject.transform.position, tag);


        if (node != null)
        {

            // enviar al nodo que esta asignado
            CoverNode temp = node.GetComponent<CoverNode>();

            if (temp != null)
            {
                temp.setOccupied(true);
                // Debug.Log(Physics.Linecast(gameObject.transform.position, temp.transform.position));

                fsm.ChangeState(States.goToCover);

            }

        }
        else {
           
            fsm.ChangeState(States.Wander);
        }

    }
    void goToCover_Enter()
    {
        if (inCover == false)
        {
            nav.Resume();
            anim.SetBool("Crouch", false);
	        anim.SetBool("Run", true);
	        if ( node != null)
        	 nav.SetDestination(node.transform.position);
        }
        else {

        }
    }
    void goToCover_Update()
    {
        if (HasArrived())
        {
            inCover = true;
            fsm.ChangeState(States.inCover);
        }
    }
    void inCover_Enter()
    {
        
        anim.SetBool("Run", false);
        anim.SetBool("Crouch", true);


        target = TeamManager.current.FindClosestTargetTeam1(gameObject.transform.position);


        if (target != null) {

            fsm.ChangeState(States.Attack);
        }
    }
    void Wander_Enter()
    {
        anim.SetBool("Run", true);

        target =  TeamManager.current.FindClosestTargetTeam1(gameObject.transform.position);
    }
    void Wander_Update()
    {
        if (HasArrived())
        {
            SetDestination(Target());
        }
        if (target != null)
        {
            if (!Physics.Linecast(gameObject.transform.position, target.transform.position, layerMask))
            {

                fsm.ChangeState(States.Attack);
            }
        }

       
    }
    void Attack_Enter()
    {
        nav.Stop();
        anim.SetBool("Crouch", true);
        _shoot.shoothing = true;

    }
    void Attack_Update()
    {
        RotateTowards(target.transform);
        if (!target.activeInHierarchy) {
            target = null;
            fsm.ChangeState(States.FindTarget);
        }

    }
    void Attack_Exit()
    {
        _shoot.shoothing = false;
    }
    void FindTarget_Enter() {
        if (inCover == false)
        {
            fsm.ChangeState(States.goToCover);
        }
        else {

            fsm.ChangeState(States.Wander);
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
