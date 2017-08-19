using UnityEngine;
using AxlPlay;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory ("Agents AI")]
	[Tooltip ("Not See Object.")]
	public class NotSeeObject : FsmStateAction
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
		[Tooltip ("The angle offset relative to the pivot position 2D")]
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

		public override void OnEnter ()
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
			if (returnedObject.Value == null) {
               
				Finish ();
				if (finishEvent != null) {
					Fsm.Event (finishEvent);
				}
			}
        
		}


		/*
        // Draw the line of sight representation within the scene window
        public override void OnDrawActionGizmos()
        {
            if (Owner != null)
				CanSeeObject.DrawLineOfSight(Owner.transform, offset.Value, fieldOfViewAngle.Value, viewDistance.Value, usePhysics2D.Value);
        }
		#endif
		*/
	}
}
