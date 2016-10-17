using UnityEngine;
using System.Collections;
using AxlPlay;


namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("Agents AI")]
	[Tooltip("Search for a target by combining the wander, within hearing range, and the within seeing range tasks using the Unity NavMesh.")]
	public class Search : FsmStateAction {
		
		[RequiredField]
		[CheckForComponent(typeof(NavMeshAgent))]

		[Tooltip("How far ahead of the current position to look ahead for a wander")]
		public FsmFloat wanderDistance = 10;
		[Tooltip("The amount that the agent rotates direction")]
		public FsmFloat wanderRate = 1;
		[Tooltip("The field of view angle of the agent (in degrees)")]
		public FsmFloat fieldOfViewAngle = 90;
		[Tooltip("The distance that the agent can see")]
		public FsmFloat viewDistance = 30;
	
		public FsmBool senseAudio = true;
		[Tooltip("How far away the unit can hear")]
		public FsmFloat hearingRadius = 30;
		[Tooltip("The offset relative to the pivot position")]
		public FsmVector3 offset;
		[Tooltip("The target offset relative to the pivot position")]
		public FsmVector3 targetOffset;
		[Tooltip("The LayerMask of the objects that we are searching for")]
		[UIHint(UIHint.Layer)]
		public FsmInt[] objectLayerMask;
		[Tooltip("Invert the mask, so you pick from all layers except those defined above.")]

		public FsmBool invertMask;


		[Tooltip("The further away a sound source is the less likely the agent will be able to hear it. " +
			"Set a threshold for the the minimum audibility level that the agent can hear")]
		public FsmFloat audibilityThreshold = 0.05f;
		[Tooltip("The object that is found")]
		public FsmGameObject returnedObject;

		public FsmEvent FINISHED;

		private NavMeshAgent agent;
		private LayerMask Ignore;

		public override void Awake()
		{
			agent = Owner.GetComponent<NavMeshAgent> ();

		}

		public override void OnEnter()
		{
			
				agent.SetDestination (Target ());

		}

		// Keep searching until an object is seen or heard (if senseAudio is enabled)
		public override  void OnUpdate()
		{
			if (HasArrived ()) {
				agent.SetDestination (Target ());

			}
			// Detect if any objects are within sight
			returnedObject.Value = MovementUtility.WithinSight(Owner.transform, offset.Value, fieldOfViewAngle.Value, viewDistance.Value, ActionHelpers.LayerArrayToLayerMask(objectLayerMask, invertMask.Value), targetOffset.Value,Ignore);

			if (returnedObject.Value != null) {
				Fsm.Event(FINISHED);
			}
			// Detect if any object are within audio range (if enabled)
			if (senseAudio.Value) {
				returnedObject.Value = MovementUtility.WithinHearingRange(Owner.transform, offset.Value, audibilityThreshold.Value, hearingRadius.Value, ActionHelpers.LayerArrayToLayerMask(objectLayerMask, invertMask.Value));
			
				if (returnedObject.Value != null) {
					Fsm.Event(FINISHED);
				}
			}

		
		}

		private Vector3 Target()
		{

			var direction = Owner.transform.forward + Random.insideUnitSphere * wanderRate.Value;
			return Owner.transform.position + direction.normalized * wanderDistance.Value;
		}

		private  bool SetDestination(Vector3 target)
		{
			if (agent.destination == target)
			{
				return true;
			}
			if (agent.SetDestination(target))
			{
				return true;
			}
			return false;
		}

		private bool HasArrived()
		{
			return !agent.pathPending && agent.remainingDistance <= agent.stoppingDistance + 0.001f;
		}


}

}
