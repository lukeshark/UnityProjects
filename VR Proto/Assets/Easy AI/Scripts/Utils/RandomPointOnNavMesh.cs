using UnityEngine;
using System.Collections;
public class RandomPointOnNavMesh : MonoBehaviour {
	public float range = 10.0f;
	private UnityEngine.AI.NavMeshPath path;
	void Start(){
		path = new UnityEngine.AI.NavMeshPath();
	}
	void Update() {
		Vector3 point;
		if (RandomPoint(transform.position, range, out point)) {
			Debug.DrawRay(point, Vector3.up, Color.blue, 1.0f);
		}
	}
	bool RandomPoint(Vector3 center, float range, out Vector3 result) {
		for (int i = 0; i < 30; i++) {
			Vector3 randomPoint = center + Random.insideUnitSphere * range;
			UnityEngine.AI.NavMeshHit hit;
			if (UnityEngine.AI.NavMesh.SamplePosition(randomPoint, out hit, 1.0f, UnityEngine.AI.NavMesh.AllAreas)){
				if (UnityEngine.AI.NavMesh.CalculatePath(transform.position, hit.position, UnityEngine.AI.NavMesh.AllAreas, path)){
					
					
					switch (path.status)
					{
					case UnityEngine.AI.NavMeshPathStatus.PathComplete:
						result = hit.position;
						return true;
					case UnityEngine.AI.NavMeshPathStatus.PathPartial:
						result = Vector3.zero;
						return false;
						
						
					case UnityEngine.AI.NavMeshPathStatus.PathInvalid:
						result = Vector3.zero;
						return false;
						
					}
	
				}
		    }
		}
		result = Vector3.zero;
		return false;
	}
}