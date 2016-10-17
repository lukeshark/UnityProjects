using UnityEngine;
using AxlPlay;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory ("Agents AI")]
	[Tooltip ("Can See Object.")]
	public class CanSeeObject : FsmStateAction
	{
		
		public FsmOwnerDefault gameObject;
		[Tooltip ("Should the 2D version be used?")]
		public FsmBool usePhysics2D;
		[Tooltip ("The field of view angle of the agent (in degrees)")]
		public FsmFloat fieldOfViewAngle;
		[Tooltip ("The distance that the agent can see")]
		public FsmFloat viewDistance;
		[Tooltip ("The offset relative to the pivot position")]
		public FsmVector3 offset;
		[Tooltip ("The object that is within sight")]
		public FsmGameObject returnedObject;

		public FsmEvent finishEvent;

		[Tooltip ("The object that we are searching for. If this value is null then the objectLayerMask will be used")]
		public FsmGameObject targetObject;
		[Tooltip ("The LayerMask of the objects that we are searching for")]
		[UIHint (UIHint.Layer)]
		public FsmInt HitLayerMask;

		[UIHint (UIHint.Layer)]
		[Tooltip ("The LayerMask of the objects to ignore when performing the line of sight check")]
		public FsmInt IgnoreLayerMask;

		[Tooltip ("The target offset relative to the pivot position")]
		public FsmVector3 targetOffset;
        [Tooltip("The angle offset relative to the pivot position 2D")]
        public FsmFloat angleOffset2D;

        private int ignoreLayerMask;
		private int objectLayerMask;
		private GameObject go;

		public override void Reset ()
		{
			if (gameObject != null)
				go = Fsm.GetOwnerDefaultTarget (gameObject);
			fieldOfViewAngle = 90f;
			viewDistance = 10f;
		}

		public override void Awake ()
		{
			go = Fsm.GetOwnerDefaultTarget (gameObject);
			objectLayerMask = 1 << HitLayerMask.Value;
			ignoreLayerMask = 1 << IgnoreLayerMask.Value;
		}

		public override void OnUpdate ()
		{

			if (usePhysics2D.Value) {
				// If the target object is null then determine if there are any objects within sight based on the layer mask
				if (targetObject.Value == null) {
					returnedObject.Value = MovementUtility.WithinSight2D (go.transform, offset.Value, fieldOfViewAngle.Value, viewDistance.Value, objectLayerMask, targetOffset.Value, angleOffset2D.Value, ignoreLayerMask);
				} else { // If the target is not null then determine if that object is within sight
					returnedObject.Value = MovementUtility.WithinSight2D (go.transform, offset.Value, fieldOfViewAngle.Value, viewDistance.Value, targetObject.Value, targetOffset.Value, angleOffset2D.Value, ignoreLayerMask);
				}
			} else {
				// If the target object is null then determine if there are any objects within sight based on the layer mask
				if (targetObject.Value == null) {
					returnedObject.Value = MovementUtility.WithinSight (go.transform, offset.Value, fieldOfViewAngle.Value, viewDistance.Value, objectLayerMask, targetOffset.Value, ignoreLayerMask);
				} else { // If the target is not null then determine if that object is within sight
					returnedObject.Value = MovementUtility.WithinSight (go.transform, offset.Value, fieldOfViewAngle.Value, viewDistance.Value, targetObject.Value, targetOffset.Value, ignoreLayerMask);
				}
			}
			if (returnedObject.Value != null) {
               
				Finish ();
				if (finishEvent != null) {
					Fsm.Event (finishEvent);
				}
			}
		}
		// Draw the line of sight representation within the scene window
		public override void OnDrawActionGizmos ()
		{
			if (Owner != null) 
			{
				if (gameObject.GameObject.Value != null)
					DrawLineOfSight (gameObject.GameObject.Value.transform, offset.Value, fieldOfViewAngle.Value, viewDistance.Value, usePhysics2D.Value);
				else
					DrawLineOfSight (Owner.transform, offset.Value, fieldOfViewAngle.Value, viewDistance.Value, usePhysics2D.Value);
			}
			
		}
		
		public static void DrawLineOfSight(Transform transform, Vector3 positionOffset, float fieldOfViewAngle, float viewDistance, bool usePhysics2D)
		{
			#if UNITY_EDITOR
			var oldColor = UnityEditor.Handles.color;
			var color = Color.yellow;
			color.a = 0.1f;
			UnityEditor.Handles.color = color;
			
			var halfFOV = fieldOfViewAngle * 0.5f;
			var beginDirection = Quaternion.AngleAxis(-halfFOV, (usePhysics2D ? Vector3.forward : Vector3.up)) * (usePhysics2D ? transform.up : transform.forward);
			UnityEditor.Handles.DrawSolidArc(transform.TransformPoint(positionOffset), (usePhysics2D ? transform.forward : transform.up), beginDirection, fieldOfViewAngle, viewDistance);
			
			UnityEditor.Handles.color = oldColor;
			#endif
		}
	

	}
}
