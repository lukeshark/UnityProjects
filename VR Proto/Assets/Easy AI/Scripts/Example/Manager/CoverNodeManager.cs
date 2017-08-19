using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class CoverNodeManager : MonoBehaviour
{

    public static CoverNodeManager current;

    public GameObject[] nodes;

    public bool test;
    void Awake()
    {
        if (current == null)
            current = this;

    }

    public GameObject FindClosestTarget(Vector3 position, string tag)
    {

        // return nodes
        //    .OrderBy(o => (o.transform.position - position).sqrMagnitude)
        //   .FirstOrDefault();

        GameObject closest = null;
        float distance = Mathf.Infinity;

        foreach (GameObject go in nodes)
        {
            // verificar si el gameobject no esta asignado

            CoverNode temp = go.GetComponent<CoverNode>();

            if (temp != null)
            {


                if (temp.tag == tag)
                {
                    if (temp.isOccupied())
                    {

                        continue;
                    }

                    Vector3 diff = go.transform.position - position;
                    float curDistance = diff.sqrMagnitude;

                    if (curDistance < distance)
                    {
                        closest = go;

                        distance = curDistance;
                    }
                }
            }


        }

        return closest;
    }
    public GameObject FindFreeNode(Vector3 position)
    {
        GameObject free = null;
        foreach (GameObject go in nodes)
        {
            // verificar si el gameobject no esta asignado

            CoverNode temp = go.GetComponent<CoverNode>();

            if (temp != null)
            {
                if (temp.isOccupied())
                {

                    continue;
                }
                else
                {

                    free = go;

                }
            }
        }
        return free;
    }


}
