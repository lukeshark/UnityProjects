using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{

	[ActionCategory (ActionCategory.Camera)]
	public class CameraFollow : FsmStateAction
	{

		public FsmGameObject target;

		[Tooltip ("Distance the camera from target object.")]
		public FsmFloat Distance = 10f;

		[Tooltip ("Max distance the camera can be from target object.")]
		public FsmFloat maxDistance = 20f;
		[Tooltip ("Min distance the camera can be from target object.")]
		public float minDistance = 3f;

		[Tooltip ("The speed the camera zooms in.")]
		public float zoomSpeed = 20f;

		[Tooltip ("The amount from the target object pivot the camera should look at.")]
		public float targetHeight = 2.0f;

		[Tooltip ("The speed at which  the camera rotates.")]
		public float camRotationSpeed = 70;

		[Tooltip ("How fast it should rotate to target angles.")]
		public float rotationDamping = 3.0f;

		[Tooltip ("The camera x euler angle.")]
		public float camXAngle = 15.0f;


		[Tooltip ("Pick only from these layers.")]
		[UIHint (UIHint.Layer)]
		public FsmInt[] layerToTransparent;



		Ray shootRay;
		// A ray from the gun end forwards.
		RaycastHit shootHit;
		// A raycast hit to get information about what was hit.

		private Transform myTransform;
		private Transform prevHit;
		private float minCameraAngle = 0.0f;
		private float maxCameraAngle = 90.0f;

		private GameObject gameObjectFaded;
		private Color prevColor;
		private float timer;

		// Code that runs on entering the state.
		public override void OnEnter ()
		{

			myTransform = Owner.transform;		
			myTransform.position = target.Value.transform.position;			

			if (target == null) {			
				Debug.LogWarning ("No target added, please add target Game object ");
			}
		}

		// Code that runs every frame.
		public override void OnLateUpdate ()
		{
			if (target == null) {			
				return;
			}

			// Zoom Camera and keep the distance between [minDistance, maxDistance].
			float mw = Input.GetAxis ("Mouse ScrollWheel");
			if (mw > 0) {			
				Distance.Value -= Time.deltaTime * zoomSpeed;
				if (Distance.Value < minDistance)
					Distance.Value = minDistance;
			} else if (mw < 0) {
				Distance.Value += Time.deltaTime * zoomSpeed;
				if (Distance.Value > maxDistance.Value)
					Distance.Value = maxDistance.Value;
			}

			// Rotate Camera around character.
			if (Input.GetButton ("Fire3")) { // 0 is left, 1 is right, 2 is middle mouse button.						
				float v = Input.GetAxis ("Mouse Y"); // The vertical movement of the mouse.
				if (v > 0) {
					camXAngle += camRotationSpeed * Time.deltaTime;
					if (camXAngle > maxCameraAngle) {
						camXAngle = maxCameraAngle;
					}
				} else if (v < 0) {
					camXAngle += -camRotationSpeed * Time.deltaTime;
					if (camXAngle < minCameraAngle) {
						camXAngle = minCameraAngle;
					}
				}
			}

			// Calculate the current rotation angles
			float wantedRotationAngle = target.Value.transform.eulerAngles.y;			
			float currentRotationAngle = myTransform.eulerAngles.y;		
			// Damp the rotation around the y-axis
			currentRotationAngle = Mathf.LerpAngle (currentRotationAngle, wantedRotationAngle, rotationDamping * Time.deltaTime);	
			// Convert the angle into a rotation
			Quaternion currentRotation = Quaternion.Euler (camXAngle, currentRotationAngle, 0);

			// Position Camera.
			myTransform.position = target.Value.transform.position;
			myTransform.position -= currentRotation * Vector3.forward * Distance.Value + new Vector3 (0, -1 * targetHeight, 0);

			Vector3 targetToLookAt = target.Value.transform.position;
			targetToLookAt.y += targetHeight;
			myTransform.LookAt (targetToLookAt);

			//Start checking if object between camera and target

			// Cast ray from camera.position to target.position and check if the specified layers are between them.
			Ray ray = new Ray (myTransform.position, (target.Value.transform.position - myTransform.position).normalized);
			Debug.DrawRay (ray.origin, ray.direction * 5f);
			RaycastHit hit;
			if (Physics.Raycast (ray, out hit, 5f, ActionHelpers.LayerArrayToLayerMask (layerToTransparent, false))) {

				if (gameObjectFaded != hit.transform.gameObject && gameObjectFaded != null) {
					if (gameObjectFaded.GetComponent<MeshRenderer> () != null) {
						gameObjectFaded.GetComponent<MeshRenderer> ().enabled = true;
					}
				}

				if (hit.transform.gameObject.GetComponent<MeshRenderer> () != null) {
					hit.transform.gameObject.GetComponent<MeshRenderer> ().enabled = false;
					gameObjectFaded = hit.transform.gameObject;
				}
			} else {
				if (gameObjectFaded != null) {
					if (gameObjectFaded.GetComponent<MeshRenderer> () != null) {
						gameObjectFaded.GetComponent<MeshRenderer> ().enabled = true;
					}
				}
			}

		}


	}

}