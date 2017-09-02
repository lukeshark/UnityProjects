using UnityEngine;
using System.Collections;


public class SelfDestruct : MonoBehaviour {
	public float selfdestruct_in = 4;

	void Start () {
		Destroy (gameObject, selfdestruct_in);
	}
}
