using UnityEngine;
using System.Collections;

public class IgnoreTimeScale : MonoBehaviour
{
    private float lastTime;
    public GameObject ps;

    private void Awake()
    {
        //ps = GetComponent<ParticleSystem>();
    }

    void Start()
    {
        lastTime = Time.realtimeSinceStartup;
    }

    void Update()
    {
        float deltaTime = Time.realtimeSinceStartup - lastTime;
        //ps.Simulate(deltaTime, true, false);
        lastTime = Time.realtimeSinceStartup;
    }
}
