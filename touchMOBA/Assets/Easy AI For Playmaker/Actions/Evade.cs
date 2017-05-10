using UnityEngine;
using System.Collections;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("Agents AI")]
	[Tooltip("Evade the target specified using the Unity NavMesh.")]
	public class Evade : FsmStateAction {

		[RequiredField]
		[CheckForComponent(typeof(UnityEngine.AI.NavMeshAgent))]

		[Tooltip("The agent has evaded when the magnitude is greater than this value")]
		public FsmFloat evadeDistance = 10;
		[Tooltip("The distance to look ahead when evading")]
		public FsmFloat lookAheadDistance = 5;
		[Tooltip("How far to predict the distance ahead of the target. Lower values indicate less distance should be predicated")]
		public FsmFloat targetDistPrediction = 20;
		[Tooltip("Multiplier for predicting the look ahead distance")]
		public FsmFloat targetDistPredictionMult = 20;
		[Tooltip("The GameObject that the agent is evading")]
		public FsmGameObject target;

		public FsmEvent FINISHED;

		// The position of the target at the last frame
		private Vector3 targetPosition;
		private UnityEngine.AI.NavMeshAgent agent;

		public override void Awake() {

			agent = Owner.GetComponent<UnityEngine.AI.NavMeshAgent> ();
		}

		public override void OnEnter()
		{
			

			targetPosition = target.Value.transform.position;
			agent.SetDestination(Target());
		}

		// Evade from the target. 
		public override void OnUpdate()
		{
			if (Vector3.Magnitude(Owner.transform.position - target.Value.transform.position) > evadeDistance.Value) {
				Fsm.Event (FINISHED);
			}

			agent.SetDestination(Target());

		}

		// Evade in the opposite direction
		private Vector3 Target()
		{
			// Calculate the current distance to the target and the current speed
			var distance = (target.Value.transform.position - Owner.transform.position).magnitude;
			var speed = agent.velocity.magnitude;

			float futurePrediction = 0;
			// Set the future prediction to max prediction if the speed is too small to give an accurate prediction
			if (speed <= distance / targetDistPrediction.Value) {
				futurePrediction = targetDistPrediction.Value;
			} else {
				futurePrediction = (distance / speed) * targetDistPredictionMult.Value; // the prediction should be accurate enough
			}

			// Predict the future by taking the velocity of the target and multiply it by the future prediction
			var prevTargetPosition = targetPosition;
			targetPosition = target.Value.transform.position;
			var position = targetPosition + (targetPosition - prevTargetPosition) * futurePrediction;

			return Owner.transform.position + (Owner.transform.position - position).normalized * lookAheadDistance.Value;
		}

	

}

}