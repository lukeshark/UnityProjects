using UnityEngine;
using System.Collections;

public class FollowCamera : MonoBehaviour {

    public static FollowCamera current;

    public Transform target;
    public float distance = 10.0f;
    public float height = 3.0f;
    public float heightDamping = 2.0f;
    public float rotationDamping = 3.0f;
    public AudioClip musica;
    public AudioClip audioAmbiente;

    private AudioSource _audio;

    void Awake()
    {

        if (current == null)
            current = this;

        distance = 15f;
        height = 10f;
        _audio = GetComponent<AudioSource>();
    }
    void Start()
    {

#if UNITY_IOS
        UnityEngine.iOS.DeviceGeneration iOSGen = UnityEngine.iOS.Device.generation;


        if (Debug.isDebugBuild)
        {
            Debug.Log("iPhone.generation     : " + UnityEngine.iOS.Device.generation);
            Debug.Log("SystemInfo.deviceType : " + SystemInfo.deviceType);
            Debug.Log("SystemInfo.deviceModel: " + SystemInfo.deviceModel);
            Debug.Log("QualityLevel=>" + QualitySettings.GetQualityLevel(), gameObject);
        }

        if (UnityEngine.iOS.Device.generation == UnityEngine.iOS.DeviceGeneration.iPadMini1Gen)
            QualitySettings.SetQualityLevel(1, true);
        else if (UnityEngine.iOS.Device.generation == UnityEngine.iOS.DeviceGeneration.iPodTouch5Gen)
            QualitySettings.SetQualityLevel(1, true);
        else
            QualitySettings.SetQualityLevel(3, true);

#endif




    }
    void LateUpdate()
    {
        if (!target)
            return;

        float wantedRotationAngle = target.eulerAngles.y;
        float wantedHeight = target.position.y + height;  // height;

        float currentRotationAngle = transform.eulerAngles.y;
        float currentHeight = transform.position.y;

        currentRotationAngle = Mathf.LerpAngle(currentRotationAngle, wantedRotationAngle, rotationDamping * Time.deltaTime);

        currentHeight = Mathf.Lerp(currentHeight, wantedHeight, heightDamping * Time.deltaTime);

        Quaternion currentRotation = Quaternion.Euler(0, currentRotationAngle, 0);

        transform.position = target.position;
        transform.position -= currentRotation * Vector3.forward * distance;  // distance;

        Vector3 temp = transform.position;
        temp.y = currentHeight;
        transform.position = temp;

        transform.LookAt(target);
    }
    public void PlayMusic()
    {
        _audio.clip = musica;
        _audio.Play();
    }
    public void StopMusic()
    {
        _audio.Stop();
    }
    public void PlayAmbient()
    {
        _audio.clip = audioAmbiente;
        _audio.Play();
    }
}
