using UnityEngine;
using AxlPlay;
namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("Agents AI")]
	[Tooltip("Check to see if the any objects are within hearing range of the current agent.")]
	public class CanHearObject : FsmStateAction {
    

		[Tooltip("Should the 2D version be used?")]
		public FsmBool usePhysics2D;

		[Tooltip("The object that we are searching for. If this value is null then the objectLayerMask will be used")]

		public FsmGameObject targetObject;

		[Tooltip("The tag of the object that we are searching for")]
		[UIHint(UIHint.Tag)]
		public FsmString targetTag;

		[Tooltip("The LayerMask of the objects that we are searching for")]
		[UIHint(UIHint.Layer)]
		public FsmInt[] objectLayerMask;

		[Tooltip("Invert the mask, so you pick from all layers except those defined above.")]

		public FsmBool invertMask;

		[Tooltip("How far away the unit can hear")]
		public FsmFloat hearingRadius = 5f;

		[Tooltip("The further away a sound source is the less likely the agent will be able to hear it. " +
			"Set a threshold for the the minimum audibility level that the agent can hear")]
		public FsmFloat audibilityThreshold = 0.05f;

		[Tooltip("The offset relative to the pivot position")]
		public FsmVector3 offset;

		[Tooltip("The returned object that is heard")]
		[UIHint(UIHint.Variable)]
		public FsmGameObject returnedObject;

		public FsmEvent Heard;

		private LayerMask layerMask;
 
		public override void OnUpdate()
		{

			// If the target object is null then determine if there are any objects within hearing range based on the layer mask
			if (targetObject.Value == null) {
				if (usePhysics2D.Value) {
					returnedObject.Value = MovementUtility.WithinHearingRange2D(Owner.transform, offset.Value, audibilityThreshold.Value, hearingRadius.Value, ActionHelpers.LayerArrayToLayerMask(objectLayerMask, invertMask.Value));
				} else {

					returnedObject.Value = MovementUtility.WithinHearingRange(Owner.transform, offset.Value, audibilityThreshold.Value, hearingRadius.Value, ActionHelpers.LayerArrayToLayerMask(objectLayerMask, invertMask.Value));
				}

			} else {
				GameObject target;
				if (string.IsNullOrEmpty(targetTag.Value)) {
					target = GameObject.FindGameObjectWithTag(targetTag.Value);
				} else {
					target = targetObject.Value;
				}
				if (Vector3.Distance(target.transform.position, Owner.transform.position) < hearingRadius.Value) { 
					returnedObject.Value = MovementUtility.WithinHearingRange(Owner.transform, offset.Value, audibilityThreshold.Value, targetObject.Value);
				}
			}
			if (returnedObject.Value != null) {
				Fsm.Event(Heard);
			}
	

		}

		// Draw the hearing radius
		public override void OnDrawActionGizmos()
		{
			#if UNITY_EDITOR
			var oldColor = UnityEditor.Handles.color;
			var color = Color.green;
			color.a = 0.1f;
			UnityEditor.Handles.color = color;

			var halfFOV = 360f * 0.5f;
			var beginDirection = Quaternion.AngleAxis(-halfFOV, (usePhysics2D.Value ? Vector3.forward : Vector3.up)) * (usePhysics2D.Value ? Owner.transform.up : Owner.transform.forward);
			UnityEditor.Handles.DrawSolidArc(Owner.transform.TransformPoint(offset.Value), (usePhysics2D.Value ? Owner.transform.forward : Owner.transform.up), beginDirection, 360f, hearingRadius.Value);

			UnityEditor.Handles.color = oldColor;
			#endif
		}
	

}// class

}// namespace playmaker
