using UnityEngine;
using System.Collections;
namespace  AxlPlay{
public class CameraFollow : MonoBehaviour {
	
 	public float distance = 10.0f;
	public float height = 3.0f;
	public float heightDamping = 2.0f;
	public float rotationDamping = 3.0f;
	public Transform target;            // The position that that camera will be following.
 	
 	
	void Start ()
	{
 
	}
	
	void LateUpdate ()
	{
		
		if (!target)
			return;
		
		float wantedRotationAngle = target.eulerAngles.y;
		float wantedHeight = target.position.y + height;  // height;
		
		float currentRotationAngle = transform.eulerAngles.y;
		float currentHeight = transform.position.y;
		
		currentRotationAngle = Mathf.LerpAngle (currentRotationAngle, wantedRotationAngle, rotationDamping * Time.deltaTime);
		
		currentHeight = Mathf.Lerp (currentHeight, wantedHeight, heightDamping * Time.deltaTime);
		
		Quaternion currentRotation = Quaternion.Euler (0, currentRotationAngle, 0);
		
		transform.position = target.position;
		transform.position -= currentRotation * Vector3.forward * distance;  // distance;
		
		Vector3 temp = transform.position;
		temp.y = currentHeight;
		transform.position = temp;
		
		transform.LookAt (target);
	}
}
}