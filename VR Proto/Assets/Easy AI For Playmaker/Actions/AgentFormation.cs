using UnityEngine;
#if UNITY_5_5_OR_NEWER
using UnityEngine.AI;
#endif
using AxlPlay;
 
namespace HutongGames.PlayMaker.Actions
{

	[ActionCategory("Agents AI")]
	public class AgentFormation : FsmStateAction
	{
		[RequiredField]
		[CheckForComponent(typeof(NavMeshAgent))]
		#region Public Variables
		public FsmOwnerDefault gameObject;
		
		public FsmGameObject FormationManager;
		
		public FsmFloat LeaderSpeed = 2f;
		public FsmBool isDead;
		public FsmBool isLeader;
		public FsmEvent inPosition;
		public FsmEvent ILeader;
		
		[UIHint(UIHint.AnimatorBool)]
		[Tooltip("The animator parameter")]
		public FsmString animBoolWalk;
		
		//[Tooltip("The Bool value to assign to the animator parameter")]
		//public FsmBool animBoolValue;
		
		private Animator _anim;
		
		#endregion
		
		#region Private Variables
    	private NavMeshAgent navMeshAgent;
		private GameObject go;
		private int indice;
		private Vector3 _target;
		private FormationManagerPM _formation;
		#endregion
		
		// Code that runs on entering the state.
		public override void OnEnter()
		{
			go = Fsm.GetOwnerDefaultTarget(gameObject);
			
			_formation = FormationManager.Value.GetComponent<FormationManagerPM>();
			
			navMeshAgent = go.GetComponent<NavMeshAgent>();
			_anim = go.GetComponent<Animator>();
			
			
			indice = _formation.getMyIndice(go);
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
				_target = _formation.GetPosition(indice);
				navMeshAgent.SetDestination(_target);
			}
			
			
		}
		
		// Code that runs every frame.
		public override void OnUpdate()
		{
			_target = _formation.GetPosition(indice);
			navMeshAgent.SetDestination(_target);
			
			if (_anim != null) {
				if (navMeshAgent.velocity.z <= 0.6f && navMeshAgent.velocity.x <= 0.6f) {
					if (!string.IsNullOrEmpty( animBoolWalk.Value)){
						_anim.SetBool (animBoolWalk.Value, false);
					}
				} else {
					if (!string.IsNullOrEmpty( animBoolWalk.Value))
						_anim.SetBool (animBoolWalk.Value, true);
				}
			}
			
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
