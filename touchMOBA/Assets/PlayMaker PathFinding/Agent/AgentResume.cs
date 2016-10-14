// (c) Copyright HutongGames, LLC 2010-2014. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.NavMeshAgent)]
	[Tooltip("Resumes the movement along the current path of a navMesh Agent. \nNOTE: The Game Object must have a NavMeshAgent component attached.")]
	public class AgentResume : FsmStateAction
	{
		[RequiredField]
		[Tooltip("The Game Object to work with. NOTE: The Game Object must have a NavMeshAgent component attached.")]
		[CheckForComponent(typeof(NavMeshAgent))]
		public FsmOwnerDefault gameObject;
		
		private NavMeshAgent _agent;
		
		private void _getAgent()
		{
			GameObject go = Fsm.GetOwnerDefaultTarget(gameObject);
			if (go == null) 
			{
				return;
			}
			
			_agent =  go.GetComponent<NavMeshAgent>();
		}	
		
		public override void Reset()
		{
			gameObject = null;
		}

		public override void OnEnter()
		{
			_getAgent();
			
			DoResume();

			Finish();		
		}
		
		void DoResume()
		{
			if (_agent == null) 
			{
				return;
			}
			
			_agent.Resume();
		}

	}
}