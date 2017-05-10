using UnityEngine;
using AxlPlay;



namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("Agents AI")]
    [Tooltip("Wander nav agent.")]
    public class Wander : FsmStateAction
    {
        [RequiredField]
        [CheckForComponent(typeof(UnityEngine.AI.NavMeshAgent))]

        public FsmOwnerDefault gameObject;
        [Tooltip("How far ahead of the current position to look ahead for a wander")]
        public FsmFloat wanderDistance = 20;
        [Tooltip("The amount that the agent rotates direction")]
        public FsmFloat wanderRate = 2;
        public FsmVector3 vector;
        private GameObject go;
        private UnityEngine.AI.NavMeshAgent navMeshAgent;

        // Reset the public variables
        public override void Reset()
        {
            wanderDistance = 20;
            wanderRate = 2;
        }
        public override void Awake()
        {
            go = Fsm.GetOwnerDefaultTarget(gameObject);
            navMeshAgent = go.GetComponent<UnityEngine.AI.NavMeshAgent>();
        }
        public override void OnEnter()
        {
            SetDestination(Target());
        }
       
        public override void OnUpdate()
        {

            if (HasArrived())
            {
                SetDestination(Target());
            }
           


        }
        private Vector3 Target()
        {
            // point in a new random direction and then multiply that by the wander distance
            var direction = go.transform.forward + Random.insideUnitSphere * wanderRate.Value;
            return go.transform.position + direction.normalized * wanderDistance.Value;
        }
        private  bool SetDestination(Vector3 target)
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
