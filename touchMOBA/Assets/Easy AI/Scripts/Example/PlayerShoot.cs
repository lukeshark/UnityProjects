using UnityEngine;
using System.Collections;

public class PlayerShoot : MonoBehaviour
{

    public int damagePerShot = 20;
    public float timeBetweenBullets = 0.15f;
    public float range = 100f;
    public int tipoArma = 1;
    public bool caminando = false;

    float timer;
    Ray shootRay;
    RaycastHit shootHit;
    int shootableMask;
    ParticleSystem gunParticles;
    //LineRenderer gunLine;
    AudioSource gunAudio;
    Light gunLight;
    float effectsDisplayTime = 0.2f;
    Animator anim;
    int _balas;

    public int _balas_cartucho;
    void Awake()
    {
        shootableMask = LayerMask.GetMask("Shootable");
        gunParticles = GetComponent<ParticleSystem>();
        gunAudio = GetComponent<AudioSource>();
        gunLight = GetComponent<Light>();
        anim = GetComponentInParent<Animator>();
        _balas_cartucho = 15;
    }
    void Update()
    {
        timer += Time.deltaTime;
        Debug.DrawRay(transform.position, transform.forward * 100f, Color.blue);

        if (Input.GetMouseButton(0) && timer >= timeBetweenBullets && Time.timeScale != 0 && _balas_cartucho > 0)
        {
            Shoot();
            StartCoroutine("DisableEffects");

        }
        else if (_balas_cartucho <= 0)
        {

            StartCoroutine("Reload");
        }
    }

    IEnumerator Reload()
    {
        anim.SetTrigger("Reload");
        yield return new WaitForSeconds(1.5f);
        _balas_cartucho = 10;
    }

    IEnumerator DisableEffects()
    {
        //Debug.Log(gameObject + " Idle_Enter");
        yield return new WaitForSeconds(0.03f);
        anim.SetBool("Shoot", false);
        gunLight.enabled = false;
    }

    void Shoot()
    {
  
        _balas_cartucho = _balas_cartucho - 1;
        anim.SetBool("Shoot",true);
        timer = 0f;

        gunAudio.Play();

        gunLight.enabled = true;

        gunParticles.Stop();
        gunParticles.Play();

        shootRay.origin = transform.position;
        shootRay.direction = transform.forward;

        if (Physics.Raycast(shootRay, out shootHit, range, shootableMask))
        {

            // hay que añadir aqui los demas tags
            if (shootHit.transform.tag == "Team1")
            {

                var temp = TeamManager.current.getBloodPooled();
                temp.transform.position = shootHit.point;
                temp.transform.rotation = Quaternion.FromToRotation(Vector3.up, shootHit.normal);
                temp.SetActive(true);
            }

            EnemyHealth enemyHealth = shootHit.collider.GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(damagePerShot);
            }
            // PlayMakerFSM rebeldeHealth = shootHit.collider.GetComponent<PlayMakerFSM>();

            /*
            var tempFSM = PlayMakerFSM.FindFsmOnGameObject(shootHit.collider.gameObject, "Health");


            if (tempFSM != null)
            {
                tempFSM.FsmVariables.FindFsmInt("hit").Value = 20;
                tempFSM.SendEvent("hit");


            }*/
           
        }
   

    }
}
