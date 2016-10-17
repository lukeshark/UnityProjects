using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{

	[ActionCategory("Agents AI")]
	public class AgentFormation : FsmStateAction
	{
		[RequiredField]
		[CheckForComponent(typeof(NavMeshAgent))]
		#region Public Variables
		public FsmOwnerDefault gameObject;
		public FsmFloat LeaderSpeed = 2f;
		public FsmBool isDead;
		public FsmBool isLeader;
		public FsmEvent inPosition;
		public FsmEvent ILeader;
		
		#endregion
		
		#region Private Variables
    	private NavMeshAgent navMeshAgent;
		private GameObject go;
		private int indice;
		private Vector3 _target;
		#endregion
		
		// Code that runs on entering the state.
		public override void OnEnter()
		{
			go = Fsm.GetOwnerDefaultTarget(gameObject);
			navMeshAgent = go.GetComponent<NavMeshAgent>();
			
			indice = FormationManagerPM.current.getMyIndice(go);
			if (indice == -1){
				Finish();
			}
			if (indice == 0){
				
				isLeader.Value = true;
				navMeshAgent.speed = LeaderSpeed.Value;
				Fsm.Event(ILeader);
				Finish();
			}
			else
			{
				_target = FormationManagerPM.current.GetPosition(indice);
				navMeshAgent.SetDestination(_target);
			}
			
			
		}
		
		// Code that runs every frame.
		public override void OnUpdate()
		{
			_target = FormationManagerPM.current.GetPosition(indice);
			navMeshAgent.SetDestination(_target);
			
			if (HasArrived()){
				
				Fsm.Event(inPosition);
                Finish();

			}
			
		}
		private bool HasArrived()
		{
			return !navMeshAgent.pathPending && navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance + 0.001f;
		}
	
	
	}

}
