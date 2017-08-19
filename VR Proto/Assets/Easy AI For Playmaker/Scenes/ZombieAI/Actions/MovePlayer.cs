using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{

	[ActionCategory (ActionCategory.GameObject)]
	public class MovePlayer : FsmStateAction
	{
		[RequiredField]
		[CheckForComponent (typeof(Rigidbody))]

		public FsmOwnerDefault Player;
		public FsmFloat Speed;

		public GameObject Spine;


		[RequiredField]
		[UIHint (UIHint.AnimatorBool)]
		[Tooltip ("The animator parameter")]
		public FsmString MoveForward;

		[RequiredField]
		[UIHint (UIHint.AnimatorBool)]
		[Tooltip ("The animator parameter")]
		public FsmString MoveBackward;

		[RequiredField]
		[UIHint (UIHint.AnimatorBool)]
		[Tooltip ("The animator parameter")]
		public FsmString MoveRight;

		[RequiredField]
		[UIHint (UIHint.AnimatorBool)]
		[Tooltip ("The animator parameter")]
		public FsmString MoveLeft;

		[RequiredField]
		[UIHint (UIHint.AnimatorBool)]
		[Tooltip ("The animator parameter")]
		public FsmString Idle;

	
		private float h;
		private float v;
	
		private Vector3 movement;
	
		private Rigidbody playerRigidbody;
		
		private GameObject go;

		public enum RotationAxes
		{
			MouseXAndY = 0,
			MouseX = 1,
			MouseY = 2

		}

		private RotationAxes axes = RotationAxes.MouseXAndY;


		public float sensitivityX = 10f;
		public float sensitivityY = 10f;


		private float minimumY = -18;
		private float maximumY = 25;

		float rotationY = 0F;

		private Animator _animator;
		private int _paramForwardID;
		private int _paramBackwardID;
		private int _paramLeftID;
		private int _paramRightID;
		private int _paramIdleID;




		public override void Awake ()
		{
			go = Fsm.GetOwnerDefaultTarget (Player);
			playerRigidbody = go.GetComponent<Rigidbody> ();


		}

		public override void OnEnter ()
		{
			_animator = go.GetComponent<Animator> ();
			// get hash from the param for efficiency:
			_paramForwardID = Animator.StringToHash (MoveForward.Value);
			_paramBackwardID = Animator.StringToHash (MoveBackward.Value);
			_paramLeftID = Animator.StringToHash (MoveLeft.Value);
			_paramRightID = Animator.StringToHash (MoveRight.Value);
			_paramIdleID = Animator.StringToHash (Idle.Value);
		}



		public override void OnLateUpdate ()
		{

			h = Input.GetAxis ("Horizontal");
			v = Input.GetAxis ("Vertical");



			// Move Forward Animation
			if (v > 0 && h == 0) {
				_animator.SetBool (_paramBackwardID, false);
				_animator.SetBool (_paramRightID, false);
				_animator.SetBool (_paramLeftID, false);
				_animator.SetBool (_paramIdleID, false);
				_animator.SetBool (_paramForwardID, true);
			}
			// Move Backward Animation
			if (v < 0 && h == 0) {
				_animator.SetBool (_paramRightID, false);
				_animator.SetBool (_paramLeftID, false);
				_animator.SetBool (_paramForwardID, false);
				_animator.SetBool (_paramIdleID, false);

				_animator.SetBool (_paramBackwardID, true);

			}
			// Move Left Animation
			if (h < 0 && v == 0) {
				_animator.SetBool (_paramRightID, false);
				_animator.SetBool (_paramForwardID, false);
				_animator.SetBool (_paramBackwardID, false);
				_animator.SetBool (_paramIdleID, false);

				_animator.SetBool (_paramLeftID, true);

			}
			// Move Right Animation
			if (h > 0 && v == 0) {
				_animator.SetBool (_paramForwardID, false);
				_animator.SetBool (_paramBackwardID, false);
				_animator.SetBool (_paramLeftID, false);
				_animator.SetBool (_paramIdleID, false);

				_animator.SetBool (_paramRightID, true);

			}
			// Idle Animation
			if (h == 0 && v == 0) {
				_animator.SetBool (_paramBackwardID, false);
				_animator.SetBool (_paramRightID, false);
				_animator.SetBool (_paramLeftID, false);
				_animator.SetBool (_paramForwardID, false);
				_animator.SetBool (_paramIdleID, true);
			}

			v *= Speed.Value;
			h *= Speed.Value;
			//Move horizontally
			playerRigidbody.MovePosition (playerRigidbody.position + (go.transform.right * h) * Time.deltaTime);
			//Move vertically
			playerRigidbody.MovePosition (playerRigidbody.position + (go.transform.forward * v) * Time.deltaTime);

			if (axes == RotationAxes.MouseXAndY) {
				float rotationX = go.transform.localEulerAngles.y + Input.GetAxis ("Mouse X") * sensitivityX;

				rotationY += Input.GetAxis ("Mouse Y") * sensitivityY;
				rotationY = Mathf.Clamp (rotationY, minimumY, maximumY);

			
				go.transform.localEulerAngles = new Vector3 (0, rotationX, 0);

				Spine.transform.localEulerAngles = new Vector3 (-rotationY, 0, 0);
			
			
				

//				RightArm.transform.localEulerAngles = new Vector3 (-rotationY, 0, 0);
			} 


		}


	}

}