// (c) Copyright HutongGames, LLC 2010-2014. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.NavMeshAgent)]
	[Tooltip("Set the height a NavMesh Agent. \n" +
		"NOTE: The Game Object must have a NavMeshAgent component attached.")]
	public class SetAgentHeight : FsmStateAction
	{
		[RequiredField]
		[Tooltip("The Game Object to work with. NOTE: The Game Object must have a NavMeshAgent component attached.")]
		[CheckForComponent(typeof(NavMeshAgent))]
		public FsmOwnerDefault gameObject;
		
		[RequiredField]
		[Tooltip("The height of the navMesh Agent.")]
		public FsmFloat height;

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
			height = null;

		}

		public override void OnEnter()
		{
			_getAgent();
			
			DoSetHeight();

			Finish();		
		}
		
		void DoSetHeight()
		{
			if (height == null || _agent == null) 
			{
				return;
			}
			
			_agent.height = height.Value;
		}

	}
}