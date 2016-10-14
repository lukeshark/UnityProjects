// (c) Copyright HutongGames, LLC 2010-2014. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.NavMeshAgent)]
	[Tooltip("Sets the relative vertical displacement of the owning GameObject. \n" +
	         "NOTE: The Game Object must have a NavMeshAgent.")]
	public class SetAgentBaseOffset : FsmStateAction
	{
		[RequiredField]
		[Tooltip("The Game Object to work with. NOTE: The Game Object must have a NavMeshAgent component attached.")]
		[CheckForComponent(typeof(NavMeshAgent))]
		public FsmOwnerDefault gameObject;
		
		[RequiredField]
		[Tooltip("the relative vertical displacement of the owning GameObject")]
		public FsmFloat baseOffset;

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
			baseOffset = null;

		}

		public override void OnEnter()
		{
			_getAgent();
			
			DoSetBaseOffset();

			Finish();		
		}
		
		void DoSetBaseOffset()
		{
			if (baseOffset == null || _agent == null) 
			{
				return;
			}
			
			_agent.baseOffset = baseOffset.Value;
		}

	}
}