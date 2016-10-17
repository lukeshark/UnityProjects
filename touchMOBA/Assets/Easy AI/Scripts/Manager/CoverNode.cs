using UnityEngine;
using System.Collections;

public class CoverNode : MonoBehaviour {

    public Vector3 SightNodeOffSet = new Vector3(0, 1, 0);

    private Vector3 myPosition;
    private Vector3 sightNodePosition;
    public float nodeRadiusVisualization = 0.1f;

    public bool alwaysDisplay = true;

    public LayerMask layerMask;

    public bool isActive = true;

    private bool occupied = false;

    // Use this for initialization
    void Start () {
        SetPositions();
    }
    void SetPositions()
    {
        myPosition = transform.position;

        sightNodePosition = transform.position;

        sightNodePosition += (transform.forward * SightNodeOffSet.x);
        sightNodePosition += (transform.up * SightNodeOffSet.y);
        sightNodePosition += (transform.right * SightNodeOffSet.z);
    }
    public Vector3 GetPosition()
    {
        return myPosition;
    }
    public void DeActivateNode()
    {
        isActive = false;
    }

    public bool isOccupied()
    {
        return occupied;
    }

    public void setOccupied(bool b)
    {
        occupied = b;
    }
    // validar si hay campo de vista para disparar
    public bool ValidCoverCheck(Vector3 targetPos)
    {
        //Check to see if this cover node is safe
        if (isActive)
        {
            if (Physics.Linecast(myPosition, targetPos, layerMask))
            {
                //Check to see if this cover node has LOS to target from firingPos
                if (!Physics.Linecast(sightNodePosition, targetPos, layerMask))
                {
                    return true;
                }
            }
        }
        return false;
    }
    // check if there is a line to the position (sin bloqueo)
    public bool CheckForSafety(Vector3 targetPos)
    {
        Debug.DrawLine(myPosition, targetPos, Color.green);

     
        return (!Physics.Linecast(myPosition, targetPos));
    }
    void OnDrawGizmos()
    {
        if (alwaysDisplay)
        {
            SetPositions();

            if (occupied)
                Gizmos.color = Color.yellow;
            else if (isActive)
                Gizmos.color = Color.green;
            else
                Gizmos.color = Color.red;

            Gizmos.DrawSphere(myPosition, nodeRadiusVisualization);
            Gizmos.DrawWireSphere(sightNodePosition, nodeRadiusVisualization * 2);

          
        }
    }

}
