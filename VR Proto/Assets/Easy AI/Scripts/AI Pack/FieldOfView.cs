using UnityEngine;
using System.Collections;
using System.Collections.Generic;
namespace AxlPlay {
[AddComponentMenu("Easy AI/Field Of View")]
public class FieldOfView : MonoBehaviour
{ 

    public float viewDistance = 10f;
    [Range(0, 360)]
    public float viewAngle = 90f;
    [Tooltip("Target mask, Pick only from these layers.")]
    public LayerMask targetMask;
    [Tooltip("Obstacle Mask, Pick only from these layers.")]
    public LayerMask obstacleMask;

    public List<Transform> visibleTargets = new List<Transform>();

    [Range(10, 100)]
    public int numRays = 20;

    void Start()
    {

        StartCoroutine("FindTargetsWithDelay", .2f);
    }
    IEnumerator FindTargetsWithDelay(float delay)
    {
        while (true)
        {
            yield return new WaitForSeconds(delay);
            FindVisibleTargets();
        }
    }
    void Update()
    {
        FindVisibleTargets();

    }
    public void FindVisibleTargets()
    {
        visibleTargets.Clear();
        Collider[] targetsInViewRadius = Physics.OverlapSphere(transform.position, viewDistance, targetMask);

        for (int i = 0; i < targetsInViewRadius.Length; i++)
        {
            Transform target = targetsInViewRadius[i].transform;
            Vector3 dirToTarget = (target.position - transform.position).normalized;
            if (Vector3.Angle(transform.forward, dirToTarget) < viewAngle / 2)
            {
                float dstToTarget = Vector3.Distance(transform.position, target.position);
                if (!Physics.Raycast(transform.position, dirToTarget, dstToTarget, obstacleMask))
                {
                    visibleTargets.Add(target);
                }
            }
        }
    }
    public Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal)
    {
        if (!angleIsGlobal)
        {
            angleInDegrees += transform.eulerAngles.y;
        }
        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }
    public GameObject findClosetsTarget()
    {

        GameObject closest = null;
        float distance = Mathf.Infinity;

        foreach (var go in visibleTargets)
        {
            Vector3 diff = go.transform.position - transform.position;
            float curDistance = diff.sqrMagnitude;

            if (curDistance < distance)
            {
                closest = go.gameObject;

                distance = curDistance;
            }


        }
        return closest;
    }

}
}
