using UnityEngine;
using System.Collections;
#if UNITY_5_5_OR_NEWER
using UnityEngine.AI;
#endif
namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("Agents AI")]
	public class SetDestination : FsmStateAction
	{
		
		[RequiredField]
		[CheckForComponent (typeof(NavMeshAgent))]

		public FsmOwnerDefault gameObject;

		public FsmGameObject target;


		public FsmEvent finishEvent;
		private NavMeshAgent navMeshAgent;
		private Vector3 localitation;

		private GameObject _gameObject;

	

		public override void OnEnter ()
		{
			navMeshAgent = Owner.GetComponent<NavMeshAgent> ();
		}

		public override void OnUpdate ()
		{
			if (target.Value != null && navMeshAgent != null)
				navMeshAgent.SetDestination (target.Value.transform.position);

			if (HasArrived ()) {
					
				if (finishEvent != null) {
					Fsm.Event (finishEvent);
				}
			}
				

		}

		private bool HasArrived ()
		{

			return !navMeshAgent.pathPending && navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance + 0.001f;
		}
	}
}