using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ShowGoldenPath : MonoBehaviour {

	public Transform target;
	public Text _txt;
	private NavMeshPath path;
	private float elapsed = 0.0f; 
	void Start () {
		path = new NavMeshPath();
		elapsed = 0.0f;
		
		Debug.Log("source tree");
		
	}
	void Update () {
		// Update the way to the goal every second.
		elapsed += Time.deltaTime;
		if (elapsed > 1.0f) {
			elapsed -= 1.0f;
			_txt.text =  NavMesh.CalculatePath(transform.position, target.position, NavMesh.AllAreas, path).ToString();
		}
		for (int i = 0; i < path.corners.Length-1; i++)
			Debug.DrawLine(path.corners[i], path.corners[i+1], Color.red);		
	}
}
