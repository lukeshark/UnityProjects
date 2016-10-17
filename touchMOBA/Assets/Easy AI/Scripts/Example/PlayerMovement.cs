using UnityEngine;
using System.Collections;
using AxlPlay;

public class PlayerMovement : MonoBehaviour {

    public Transform spawnPoint;
    public float speed = 10f;
    Vector3 movement;                   // The vector to store the direction of the player's movement.
    Animator anim;                      // Reference to the animator component.
    Rigidbody playerRigidbody;          // Reference to the player's rigidbody.
    int floorMask;                      // A layer mask so that a ray can be cast just at gameobjects on the floor layer.
    float camRayLength = 100f;          // The length of the ray from the camera into the scene.
    float rotateSpeed = 18f;
    public Camera cameramain;
    public Transform baseGun;


    //private AudioSource audio;
    private int groundLayer;
    Ray shootRay;
    RaycastHit hit;

    bool flagShoot;
    float touchTime;
    bool isTouching;
    // init FSM
    public enum States
    {
        Idle,
        Walking
    }
    private StateMachine<States> fsm;

    void Awake()
    {
        // Create a layer mask for the floor layer.
        floorMask = LayerMask.GetMask("floor");

       // audio = GetComponent<AudioSource>();

        // Set up references.
        anim = GetComponent<Animator>();
        playerRigidbody = GetComponent<Rigidbody>();
        //Initialize State Machine Engine		
        fsm = StateMachine<States>.Initialize(this, States.Idle);


        // para mayor velocidad llamarlo asi =>  componentReference => StateMachineRunner
        //StateMachine<States> fsm = GetComponent<StateMachineRunner>().Initialize<States>(componentReference);


    }
    public void RestartPlayer()
    {

        gameObject.transform.position = spawnPoint.position;
        gameObject.transform.rotation = Quaternion.Euler(new Vector3(0, 180, 0));
        fsm.ChangeState(States.Idle, StateTransition.Overwrite);


    }
    private void Idle_Enter()
    {

        //baseGun.localRotation = Quaternion.Euler(new Vector3(0, 94,  90)); 
        //   PlayerGun.rotation = Quaternion.Euler(new Vector3(15, 90, 0));

        //anim.SetFloat("Body_Vertical_f", 0);
        anim.SetBool("Run", false);
    }

    private IEnumerator Walking_Enter()
    {
        anim.SetBool("Run", true);
        yield return new WaitForSeconds(0.2f);
        // si camina el angulo de tiro es diferente
        //baseGun.localRotation = Quaternion.Euler(new Vector3(0, 80, 90));
       // anim.SetFloat("Body_Vertical_f", 0.2f);
    }
    void Update()
    {
        // Store the input axes.
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

 

        // Create a boolean that is true if either of the input axes is non-zero.
        bool walking = v != 0f || h != 0;
        // Tell the animator whether or not the player is walking.
        if (walking)
        {
            shootRay.origin = transform.position;
            shootRay.direction = transform.up * -1;
            //set layermask to test for "Ground" layer (layer 8)
           /* if (Physics.Raycast(shootRay, out hit, 1, floorMask))
            {
              GetComponent<AudioSource>().clip = stepCalle;
                if (!GetComponent<AudioSource>().isPlaying)
                {
                    GetComponent<AudioSource>().volume = 0.8f;
                    GetComponent<AudioSource>().pitch = 1f;
                    GetComponent<AudioSource>().Play();
                }
            }
            else
            {
                GetComponent<AudioSource>().Stop();
            }
            /*else {
				audio.clip = stepTierra;
				if (!audio.isPlaying) {
					audio.volume = 0.25f;
					audio.pitch = 1.2f;
					audio.Play ();
				}
			
			}*/

            fsm.ChangeState(States.Walking);
            //baseGun.eulerAngles = new Vector3(baseGun.eulerAngles.x, 94, baseGun.eulerAngles.z);
        }
        else
        {
            fsm.ChangeState(States.Idle);

        }

        /*   if (isTouching) {

               touchTime += Time.deltaTime;
               txt.text = touchTime.ToString();
         
           */
        // Move the player around the scene.
        //  Move(h, v);

        //Take input for rotation
        ///   Vector3 rotation = new Vector3(0f, 0.5f, 0f);
        //   rotation *= rotateSpeed;
        //Rotate
        //   playerRigidbody.MoveRotation(transform.rotation * Quaternion.Euler(rotation));

        //Take input for vertical movement

        v *= speed;
        //Move vertically
        playerRigidbody.MovePosition(playerRigidbody.position + (transform.forward * v) * Time.deltaTime);

        //Take input for horizontal movement

        h *= speed;
        //Move horizontally
        playerRigidbody.MovePosition(playerRigidbody.position + (transform.right * h) * Time.deltaTime);


        // Turn the player to face the mouse cursor.
        // Turning();
        rotar(h, v);

    }
    void OnTriggerEnter(Collider other)
    {

       


    }
    void Move(float h, float v)
    {
        // Set the movement vector based on the axis input.
        movement.Set(h, 0f, v);

        // Normalise the movement vector and make it proportional to the speed per second.
        movement = movement.normalized * speed * Time.deltaTime;

        // Move the player to it's current position plus the movement.
        // playerRigidbody.MovePosition(playerRigidbody.position + transform.position + movement);


    }
    public void rotar(float h, float v)
    {

        transform.Rotate(new Vector3(0, h * Time.deltaTime * rotateSpeed, 0f));

    }


    public void LookAtDirection(Vector3 targetDirection)
    {

        gameObject.transform.rotation = Quaternion.Slerp(gameObject.transform.rotation,
                                        Quaternion.LookRotation(targetDirection),
                                        25f * Time.deltaTime);
    }
    void Turning()
    {
        // Create a ray from the mouse cursor on screen in the direction of the camera.
        Ray camRay = cameramain.ScreenPointToRay(Input.mousePosition);

        // Create a RaycastHit variable to store information about what was hit by the ray.
        RaycastHit floorHit;

        // Perform the raycast and if it hits something on the floor layer...
        if (Physics.Raycast(camRay, out floorHit, camRayLength, floorMask))
        {
            // Create a vector from the player to the point on the floor the raycast from the mouse hit.
            Vector3 playerToMouse = floorHit.point - transform.position;

            // Ensure the vector is entirely along the floor plane.
            playerToMouse.y = 0f;

            // Create a quaternion (rotation) based on looking down the vector from the player to the mouse.
            Quaternion newRotation = Quaternion.LookRotation(playerToMouse);

            // Set the player's rotation to this new rotation.

            playerRigidbody.MoveRotation(newRotation);


            gameObject.transform.rotation = Quaternion.Slerp(gameObject.transform.rotation,
                                     Quaternion.LookRotation(playerToMouse),
                                      rotateSpeed * Time.deltaTime);


        }
    }
}
