using UnityEngine;
using System.Collections;
using AxlPlay;
#if UNITY_5_5_OR_NEWER
using UnityEngine.AI;
#endif
namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("Agents AI")]
	[Tooltip("Find a place to hide and move to it using the Unity NavMesh.")]
	public class Cover : FsmStateAction {

		[RequiredField]
		[CheckForComponent(typeof(NavMeshAgent))]


		[Tooltip("The distance to search for cover")]
		public FsmFloat maxCoverDistance = 1000;
		[Tooltip("The layermask of the available cover positions")]
		 [UIHint(UIHint.Layer)]
		public FsmInt[] availableLayerCovers;

		[Tooltip("Invert the mask, so you pick from all layers except those defined above.")]

		public FsmBool invertMask;

		[Tooltip("The maximum number of raycasts that should be fired before the agent gives up looking for an agent to find cover behind")]
		public FsmInt maxRaycasts = 200;
		[Tooltip("How large the step should be between raycasts")]
		public FsmFloat rayStep = 2;
		[Tooltip("Once a cover point has been found, multiply this offset by the normal to prevent the agent from hugging the wall")]
		public FsmFloat coverOffset = 2;
		[Tooltip("Should the agent look at the cover point after it has arrived?")]
		public FsmBool lookAtCoverPoint = false;
		[Tooltip("The agent is done rotating to the cover point when the square magnitude is less than this value")]
		public FsmFloat rotationEpsilon = 0.5f;
		[Tooltip("Max rotation delta if lookAtCoverPoint")]
		public FsmFloat maxLookAtRotationDelta;

		[Tooltip("The agent has arrived when they are less than the specified distance")]
		public FsmFloat arrivedDistance = 2;

		[Tooltip("The event that will be run if not found cover")]
		public FsmEvent NotFoundCover;

		[Tooltip("The event that will be run if found cover")]
		public FsmEvent FoundCover;

		private Vector3 coverPoint;
		// The position to reach, offsetted from coverPoint
		private Vector3 coverTarget;
		// Was cover found?
		private bool foundCover;



		private NavMeshAgent agent;

		bool HasArrived()
		{
			var direction = (agent.destination - Owner.transform.position);
			// Do not account for the y difference if it is close to zero.
			if (Mathf.Abs(direction.y) < 0.1f) {
				direction.y = 0;
			}
			return !agent.pathPending && direction.magnitude <= arrivedDistance.Value;
		}
 
		public override void OnEnter()
		{
			agent = Owner.GetComponent<NavMeshAgent> ();
			RaycastHit hit;
			int raycastCount = 0;
			var direction = Owner.transform.forward;
			float step = 0;
			coverTarget = Owner.transform.position;
			foundCover = false;
			// Keep firing a ray until too many rays have been fired
			while (raycastCount < maxRaycasts.Value) {
				var ray = new Ray(Owner.transform.position, direction);
				if (Physics.Raycast(ray, out hit, maxCoverDistance.Value, ActionHelpers.LayerArrayToLayerMask(availableLayerCovers, invertMask.Value))) {
					// A suitable agent has been found. Find the opposite side of that agent by shooting a ray in the opposite direction from a point far away
					if (hit.collider.Raycast(new Ray(hit.point - hit.normal * maxCoverDistance.Value, hit.normal), out hit, Mathf.Infinity)) {
						coverPoint = hit.point;
						coverTarget = hit.point + hit.normal * coverOffset.Value;
						foundCover = true;
						break;
					}
				}
				// Keep sweeiping along the y axis
				step += rayStep.Value;
				direction = Quaternion.Euler(0, Owner.transform.eulerAngles.y + step, 0) * Vector3.forward;
				raycastCount++;
			}

			if (foundCover) {
				agent.SetDestination(coverTarget);
			}


		}// on enter

		// Seek to the cover point. 
		public override void OnUpdate()
		{
			if (!foundCover) {
				Fsm.Event (NotFoundCover);
			}
			if (HasArrived()) {
				var rotation = Quaternion.LookRotation(coverPoint - Owner.transform.position);
			
				if (!lookAtCoverPoint.Value || Quaternion.Angle(Owner.transform.rotation, rotation) < rotationEpsilon.Value) {
					Fsm.Event (FoundCover);
				} else {
					// Still needs to rotate towards the target
					Owner.transform.rotation = Quaternion.RotateTowards(Owner.transform.rotation, rotation, maxLookAtRotationDelta.Value);
				}
			}


}// on update


}// class

} // namespace
