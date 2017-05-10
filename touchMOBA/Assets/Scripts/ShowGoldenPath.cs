using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ShowGoldenPath : MonoBehaviour {

	public Transform target;
	public Text _txt;
	private UnityEngine.AI.NavMeshPath path;
	private float elapsed = 0.0f; 
	void Start () {
		path = new UnityEngine.AI.NavMeshPath();
		elapsed = 0.0f;
		
		Debug.Log("source tree");
		
	}
	void Update () {
		// Update the way to the goal every second.
		elapsed += Time.deltaTime;
		if (elapsed > 1.0f) {
			elapsed -= 1.0f;
			_txt.text =  UnityEngine.AI.NavMesh.CalculatePath(transform.position, target.position, UnityEngine.AI.NavMesh.AllAreas, path).ToString();
		}
		for (int i = 0; i < path.corners.Length-1; i++)
			Debug.DrawLine(path.corners[i], path.corners[i+1], Color.red);		
	}
}
