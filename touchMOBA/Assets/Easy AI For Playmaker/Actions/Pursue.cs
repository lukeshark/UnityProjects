using UnityEngine;
using System.Collections;
using AxlPlay;

namespace HutongGames.PlayMaker.Actions
{
    [ActionCategory("Agents AI")]
    [Tooltip("Pursue the target specified using the Unity NavMesh.")]
    public class Pursue : FsmStateAction
    {
        [RequiredField]
        [CheckForComponent(typeof(NavMeshAgent))]

        [Tooltip("How far to predict the distance ahead of the target. Lower values indicate less distance should be predicated")]
        public FsmFloat targetDistPrediction = 20;
        [Tooltip("Multiplier for predicting the look ahead distance")]
        public FsmFloat targetDistPredictionMult = 20;
        [Tooltip("The GameObject that the agent is pursuing")]
        public FsmGameObject target;

        [Tooltip("The agent has arrived when they are less than the specified distance")]
        public FsmFloat arrivedDistance = 2;

        // The position of the target at the last frame
        private Vector3 targetPosition;

        private NavMeshAgent agent;

        [Tooltip("The event will run when the object arrives")]
        public FsmEvent ArrivedEvent;


        public override void Awake()
        {
            if (Owner != null)
                agent = Owner.GetComponent<NavMeshAgent>();
        }

        // Reset the public variables
        public override void Reset()
        {
            targetDistPrediction = 20;
            targetDistPredictionMult = 20;
            target = null;

        }
        public override void OnEnter()
        {
            targetPosition = target.Value.transform.position;
            agent.Resume();
            agent.SetDestination(Target());

        }
        public override void OnUpdate()
        {
            if (HasArrived() && ArrivedEvent != null)
            {
                agent.Stop();
                Fsm.Event(ArrivedEvent);
                Finish();
            }

            // Target will return the predicated position
            agent.SetDestination(Target());

        }
		public override void OnExit ()
		{
			if (agent != null)
				agent.Stop();
		}

        private Vector3 Target()
        {
            // Calculate the current distance to the target and the current speed
            var distance = (target.Value.transform.position - Owner.transform.position).magnitude;
            var speed = agent.velocity.magnitude;

            float futurePrediction = 0;
            // Set the future prediction to max prediction if the speed is too small to give an accurate prediction
            if (speed <= distance / targetDistPrediction.Value)
            {
                futurePrediction = targetDistPrediction.Value;
            }
            else {
                futurePrediction = (distance / speed) * targetDistPredictionMult.Value; // the prediction should be accurate enough
            }

            // Predict the future by taking the velocity of the target and multiply it by the future prediction
            var prevTargetPosition = targetPosition;
            targetPosition = target.Value.transform.position;
            return targetPosition + (targetPosition - prevTargetPosition) * futurePrediction;
        }
        bool HasArrived()
        {
            var direction = (agent.destination - Owner.transform.position);
            // Do not account for the y difference if it is close to zero.
            if (Mathf.Abs(direction.y) < 0.1f)
            {
                direction.y = 0;
            }
            return !agent.pathPending && direction.magnitude <= arrivedDistance.Value;
        }


    }
}
