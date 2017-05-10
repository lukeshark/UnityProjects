using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
    [ActionCategory("Agents AI")]
	[Tooltip("Flee the target")]
    public class Flee : FsmStateAction
    {

        [RequiredField]
        [CheckForComponent(typeof(UnityEngine.AI.NavMeshAgent))]

        public FsmOwnerDefault gameObject;

        [Tooltip("The agent has fleed when the magnitude is greater than this value")]
        public FsmFloat fleedDistance;
        [Tooltip("The distance to look ahead when fleeing")]
        public FsmFloat lookAheadDistance;
        [Tooltip("The GameObject that the agent is fleeing from")]
        public FsmGameObject target;

        public FsmEvent finishEvent;

		public FsmInt LayerOfWalls;

        private GameObject go;
        private UnityEngine.AI.NavMeshAgent navMeshAgent;

        public override void Awake()
        {
			go = Fsm.GetOwnerDefaultTarget(gameObject);
            navMeshAgent = go.GetComponent<UnityEngine.AI.NavMeshAgent>();
        }
        public override void OnUpdate()
        {

		   if (Vector3.Magnitude(go.transform.position - target.Value.transform.position) > fleedDistance.Value)
            {
            
                Finish();
                if (finishEvent != null)
                {
                    Fsm.Event(finishEvent);
                }
            }
			SetDestination(Target());

        }
        // Flee in the opposite direction
        private Vector3 Target()
        {
            return go.transform.position + (go.transform.position - target.Value.transform.position).normalized * lookAheadDistance.Value;
        }
        private bool SetDestination(Vector3 target)
        {
            if (navMeshAgent.destination == target)
            {
                return true;
            }
            if (navMeshAgent.SetDestination(target))
            {
                return true;
            }
            return false;
        }

        private bool HasArrived()
        {
            return !navMeshAgent.pathPending && navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance + 0.001f;
        }

    }
}
