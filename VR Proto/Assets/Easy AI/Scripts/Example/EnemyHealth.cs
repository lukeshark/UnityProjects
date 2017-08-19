using UnityEngine;
using System.Collections;

public class EnemyHealth : MonoBehaviour
{
    public int startingHealth = 100;
    public int currentHealth;
    public float sinkSpeed = 2.5f;
    public int scoreValue = 10;
    public AudioClip deathClip;

    private UnityEngine.AI.NavMeshAgent _navMeshAgent;
    private Team2 _team2;
    private Team1 _team1;
    private enemyPatrol _patrol;

    public GameObject _target;

    Animator anim;
    //AudioSource enemyAudio;
    bool isDead;
    void Awake()
    {
        _navMeshAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        anim = GetComponent<Animator>();

        // enemyAudio = GetComponent<AudioSource>();
        // hitParticles = GetComponentInChildren <ParticleSystem> ();
        // capsuleCollider = GetComponent <CapsuleCollider> ();

        _team1 = GetComponent<Team1>();
        _team2 = GetComponent<Team2>();
        _patrol = GetComponent<enemyPatrol>();

        currentHealth = startingHealth;
    }
    public void TakeDamage(int amount)
    {


        if (isDead)
            return;

        if (_team1 != null)
        {
            _team1.target = _target;
            _team1.fsm.ChangeState(Team1.States.Attack);
        }
        if (_team2 != null)
        {
            _team2.target = _target;
            _team2.fsm.ChangeState(Team2.States.Attack);
        }
        if (_patrol != null) {

            _patrol.target = _target;
            _patrol.fsm.ChangeState(enemyPatrol.States.Pursue);

        }
        //enemyAudio.Play ();

        currentHealth -= amount;


        if (currentHealth <= 0)
        {
            Death();
        }
    }


    void Death()
    {
        if (tag == "Team1")
            TeamManager.current.Team1Dead();
        if (tag == "Team2")
            TeamManager.current.Team2Dead();

        _navMeshAgent.Stop();
        isDead = true;


        anim.SetTrigger("Dead");

        //  enemyAudio.clip = deathClip;
        //  enemyAudio.Play ();
        Invoke("deactive", 2f);

    }
    void deactive()
    {

        // notificar al manager
        currentHealth = 100;
        gameObject.SetActive(false);
    }

    public void StartSinking()
    {
        GetComponent<UnityEngine.AI.NavMeshAgent>().enabled = false;
        // GetComponent<Rigidbody>().isKinematic = true;
        //ScoreManager.score += scoreValue;
        // Destroy(gameObject, 2f);
    }
}
