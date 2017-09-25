using UnityEngine;
using System;
using System.Collections;
using UnityEngine.AI;

public class GravGun : MonoBehaviour
{

    private GameObject playerObject;
    private CharacterController controller;
    private GameObject mainCamera;
    private Vector3 currentPosition;
    private GameObject[] latchables;

    private bool hooked = false;
    private bool crouching = false;
    private bool sprinting = false;
    public bool carrying = false;
    private bool dJumped = false;
    private float mouseX = 0.0f;
    private float mouseY = 0.0f;
    private Vector3 rotateDirection = Vector3.zero;
    private Vector3 moveDirection = Vector3.zero;

    private GameObject carriedObject;
    private GameObject thrownObject;


    public float pickupRadius = 1.0f;
    private Rigidbody r;
    private Collider[] hitColliders;
    public float distance = 1.0f;


    public float standingHeight = 0.75f;
    public float crouchHeight = 0.1f;
    public float cameraCrouch = 1.0f;

    public float threshold = 1f;
    private float currentSpeed;
    public GameObject pickup;
    public float speed = 4.0f;
    public float sprintSpeed = 6.0f;
    public float crouchSpeed = 1.0f;
    public float climbSpeed = 16.0f;
    public float jumpSpeed = 8.0f;
    public float gravity = 17.0f;
    public float rotateSpeed = 8.0f;
    public float thrust = 1024.0f;
    public float smooth = 14.0f;
    public float rotation = 2.0f;
    public Transform selectedController;

    [Header("-Controller Settings-")]
    public OVRInput.Controller myController;  //Controller Choice
    public OVRInput.Button ForwardButton;        //Button for Default Movement
    public OVRInput.Button BackwardButton;
    public OVRInput.Button TeleportButton;     //Teleport Button
    public OVRInput.Button BlinkButton;       //Blink Foward
    public OVRInput.Button RotateButton;      //Rotate to Where Controller Is Located
    public OVRInput.Button GravButton;        //Trigger for grav gun                           

    // Use this for initialization
    void Start()
    {
        //playerObject = GameObject.Find("Player");
        //controller = GetComponent<CharacterController>();
        mainCamera = GameObject.FindWithTag("MainCamera");
        Debug.Log(mainCamera.ToString());
        //currentSpeed = speed;
        //currentPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (carrying == true)
        {
            if (carriedObject != null)
            {
                Carry(carriedObject);
                CheckThrow();
                CheckDrop();
            }
            else if (carriedObject != null)
            {
                // precautionary measure - possibly unnecessary ("if (carriedObject) != null)" above, "carrying = false" below)
                carrying = false;
            }
        }
        else if (carrying == false)
        {
             Pickup();
        }
    }

    public void Pickup()
    {

        Ray ray = new Ray(selectedController.position, selectedController.forward);
        RaycastHit hit;

        if (OVRInput.GetDown(GravButton, myController))
        {
            Debug.Log("mouse button down - grabbing now");
            if (Physics.Raycast(ray, out hit))
            {
              pickup = hit.collider.gameObject;
                Debug.Log("Object hit - checking for tag");
                if (pickup.GetComponent<Rigidbody>() != null)
                {
                    Debug.Log("grabbable tag found - picking up now");
                    carrying = true;
                    carriedObject = pickup;
                    r = carriedObject.GetComponent<Rigidbody>();
                    r.useGravity = false;
                    if (pickup.GetComponent<NavMeshAgent>() != null)
                    {
                        pickup.GetComponent<NavMeshAgent>().enabled = false;
                    }
                }
            }
        }
    }

    public void Carry(GameObject o)
    {
        if (carrying == true && carriedObject != null)
        {
            o.transform.position = Vector3.Lerp(
                o.transform.position,
                selectedController.transform.position + (selectedController.transform.forward * distance),
                Time.deltaTime * smooth
                );
            //o.transform.Rotate(Vector3.right * rotation);
        }
        else if (carriedObject == null)
        {
            carrying = false;
        }
    }

    // Check if item should be dropped after pickup
    public void CheckDrop()
    {
        if (carriedObject != null)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                DropObject();
            }
        }
    }

    // Drop objects that have been picked up
    public void DropObject()
    {
        carrying = false;
        carriedObject.GetComponent<Rigidbody>().useGravity = true;
        carriedObject = null;
    }

    public void CheckThrow()
    {
        if (carrying == true && OVRInput.GetUp(GravButton, myController))
        {
            carriedObject.GetComponent<Rigidbody>().isKinematic = false;
            ThrowObject();
        }
    }

    public void ThrowObject()
    {
        carrying = false;
        thrownObject = carriedObject;
        carriedObject = null;

        //thrownObject.tag = "PlayerBullet";
        thrownObject.GetComponent<Rigidbody>().useGravity = true;
        thrownObject.GetComponent<Rigidbody>().AddForce(selectedController.transform.forward * thrust);
        if (thrownObject.GetComponent<NavMeshAgent>() != null)
        {
            Debug.Log("starting coroutine");
            thrownObject.GetComponent<NavMeshAgent>().enabled = true;
        }

        thrownObject = null;
    }

    //public IEnumerator ThrowEnemy(GameObject enemy)
    //{
    //    Debug.Log("enemy is thrown - looking for velocity");
    //    if (enemy.GetComponent<Rigidbody>().velocity.magnitude < threshold)
    //    {
    //        Debug.Log("enemy thrown is below velocity threshold");
    //        enemy.GetComponent<NavMeshAgent>().enabled = true;
    //    }
    //    yield return new WaitForSeconds(1f);
    //}
}