using UnityEngine;
using System.Collections;

namespace HutongGames.PlayMaker.Actions
{
	public class SetDestination : FsmStateAction
	{
		
		[RequiredField]
		[CheckForComponent (typeof(NavMeshAgent))]

		public FsmOwnerDefault gameObject;

		public FsmGameObject target;


		public FsmEvent finishEvent;
		private GameObject go;
		private NavMeshAgent navMeshAgent;
		private Vector3 localitation;

		private GameObject _gameObject;

		public override void Reset ()
		{
			
			if (gameObject != null)
				go = target.Value;
		}

		public override void Awake ()
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