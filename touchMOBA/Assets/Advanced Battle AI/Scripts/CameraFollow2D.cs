using UnityEngine;
using System.Collections;

public class CameraFollow2D : MonoBehaviour 
{
	public Transform player = null;
	private float targetX = 0;
	private Vector3 newPos = Vector3.zero;


	void Awake ()
	{
		// Setting up the reference.
		//player = GameObject.FindGameObjectWithTag("Player").transform;
	}


	void FixedUpdate ()
	{
		targetX = player.position.x;

		newPos = transform.position;

		newPos.x = targetX;
		transform.position = newPos;
	}
	
	
}
