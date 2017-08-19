using UnityEngine;
using System.Collections;
using AxlPlay;

public class EnemyShoot : MonoBehaviour
{

    public int damagePerShot = 20;
    public float timeBetweenBullets = 0.15f;
    public float range = 100f;
    public int tipoArma = 1;
    public bool caminando = false;

    public bool shoothing;


    public string TagToShoot;

    public LayerMask shootableMask;

    float _timer;
    RaycastHit shootHit;
    //int _shootableMask;
    ParticleSystem _gunParticles;
    //LineRenderer gunLine;
    Light _gunLight;
    private Animator _anim;
    int _balas;
    // init FSM
    public enum States
    {
        Shoot,
        Reload
    }
    private StateMachine<States> fsm;
    void Awake()
    {
 
        // _shootableMask = LayerMask.GetMask("Team1");
        _gunParticles = GetComponent<ParticleSystem>();
        //  gunLine = GetComponent <LineRenderer> ();
        _gunLight = GetComponent<Light>();
        _anim = GetComponentInParent<Animator>();
        //Initialize State Machine Engine		
        fsm = StateMachine<States>.Initialize(this, States.Shoot);
    }
    // Use this for initialization
    void Start()
    {

    }
    void Update() {
        Debug.DrawRay(transform.position, transform.forward * 100, Color.red);
    }
    // Update is called once per frame
    void Shoot_Update()
    {
       
        _timer += Time.deltaTime;
        if (shoothing && _timer >= timeBetweenBullets && Time.timeScale != 0)
        {
            if (_balas < 10)
            {
                Shoot();
                StartCoroutine("DisableEffects");
            }
            else {
                fsm.ChangeState(States.Reload);
            }

        }
        // _shootRay.origin = transform.position;
        // _shootRay.direction = transform.forward;

        // Debug.DrawRay(_shootRay.origin, _shootRay.direction * 100f, Color.red);
    }
    IEnumerator DisableEffects()
    {
        //Debug.Log(gameObject + " Idle_Enter");
        yield return new WaitForSeconds(0.03f);
        _anim.SetBool("Shoot", false);
        _gunLight.enabled = false;
    }
    void Shoot()
    {
        _balas = _balas + 1;

        _anim.SetBool("Shoot", true);
        _timer = 0f;

        //_gunAudio.Play();

        _gunLight.enabled = true;

        _gunParticles.Stop();
        _gunParticles.Play();

        // gunLine.enabled = true;
        //gunLine.SetPosition (0, transform.position);
		Ray _shootRay = new Ray();

        _shootRay.origin = transform.position;
        _shootRay.direction = transform.forward;

        
        if (Physics.Raycast(_shootRay, out shootHit, range, shootableMask))
        {
            // hay que añadir aqui los demas tags
            if (shootHit.transform.tag == TagToShoot)
            {
                EnemyHealth playerHealth = shootHit.collider.GetComponent<EnemyHealth>();
                if (playerHealth != null)
                {
                    playerHealth._target = this.transform.root.gameObject;
                    playerHealth.TakeDamage(damagePerShot);
                }

                
                /*var temp ; // aqui blood

                if (temp != null)
                {
                    temp.transform.position = shootHit.point;
                    temp.transform.rotation = Quaternion.FromToRotation(Vector3.up, shootHit.normal);
                    temp.SetActive(true);
                }*/
            }


            // aqui aplicar daño al jugador
             
            // gunLine.SetPosition (1, shootHit.point);
        }
        else
        {
            //gunLine.SetPosition (1, shootRay.origin + shootRay.direction * range);
        }
    }
    IEnumerator Reload_Enter ()
    {
       

        _anim.SetTrigger("Reload");
        yield return new WaitForSeconds(1.5f);
        _balas = 0;
        fsm.ChangeState(States.Shoot);
    }
    private void RotateTowards(Transform target)
    {

        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);


    }
}
