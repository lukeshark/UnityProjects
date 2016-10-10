// (c) Copyright HutongGames, LLC 2010-2014. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.NavMeshAgent)]
	[Tooltip("Set flag to automate movement onto and off of OffMeshLinks. \n" +
		"NOTE: The Game Object must have a NavMeshAgent component attached.")]
	public class SetAgentAutoTraverseOffMeshLink : FsmStateAction
	{
		[RequiredField]
		[Tooltip("The Game Object to work with. NOTE: The Game Object must have a NavMeshAgent component attached.")]
		[CheckForComponent(typeof(NavMeshAgent))]
		public FsmOwnerDefault gameObject;
		
		[RequiredField]
		[Tooltip("flag for automatic movement onto and off of OffMeshLinks")]
		public FsmBool autoTraverseOffMeshLink;

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
			autoTraverseOffMeshLink = null;

		}

		public override void OnEnter()
		{
			_getAgent();
			
			DoSetAutoTraverseOffMeshLink();

			Finish();		
		}
		
		void DoSetAutoTraverseOffMeshLink()
		{
			if (autoTraverseOffMeshLink == null || _agent == null) 
			{
				return;
			}
			
			_agent.autoTraverseOffMeshLink = autoTraverseOffMeshLink.Value;
		}

	}
}