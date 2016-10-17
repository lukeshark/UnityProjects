using UnityEngine;
using System.Collections;
[AddComponentMenu("Easy AI/Target Reachable")]
public class TargetReachable : MonoBehaviour {
    public GameObject goFrom;
    public Transform target;

    public bool isReachable;

	private NavMeshPath path;
    private float elapsed = 0.0f;

    void Start()
	{ 
		path = new NavMeshPath();
		elapsed = 0.0f;
		if (goFrom == null){
			
			goFrom = gameObject;
		}
    }
    void Update()
    {
        // Update the way to the goal every second.
        elapsed += Time.deltaTime;
	    if (elapsed > 0.1f)
        {
            elapsed = 0f;
           isReachable =  NavMesh.CalculatePath(goFrom.transform.position, target.position, NavMesh.AllAreas, path);
        }
	    for (int i = 0; i < path.corners.Length - 1; i++){
		    Debug.DrawLine(path.corners[i], path.corners[i + 1], Color.red);
	    }
		    
    }
	void OnDrawGizmos(){
		
		
		if (path == null){
			path = new NavMeshPath();
			
		}
		if (goFrom == null){
			
			goFrom = gameObject;
		}
		isReachable = NavMesh.CalculatePath(goFrom.transform.position, target.position, NavMesh.AllAreas, path);
		for (int i = 0; i < path.corners.Length - 1; i++)
			Debug.DrawLine(path.corners[i], path.corners[i + 1], Color.red);
		
	}

}
