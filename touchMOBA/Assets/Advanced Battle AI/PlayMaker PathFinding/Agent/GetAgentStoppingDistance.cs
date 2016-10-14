// (c) Copyright HutongGames, LLC 2010-2014. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.NavMeshAgent)]
	[Tooltip("Gets the stopping distance from the destination position of a NavMesh Agente. \nNOTE: The Game Object must have a NavMeshAgentcomponent attached.")]
	public class GetAgentStoppingDistance : FsmStateAction
	{
		[RequiredField]
		[Tooltip("The Game Object to work with. NOTE: The Game Object must have a NavMeshAgent component attached.")]
		[CheckForComponent(typeof(NavMeshAgent))]
		public FsmOwnerDefault gameObject;
		
		[RequiredField]
		[Tooltip("Store the stopping distance from the target position")]
		[UIHint(UIHint.Variable)]
		public FsmFloat storeResult;
		
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
			storeResult = null;
		}

		public override void OnEnter()
		{
			_getAgent();

			DoGetStoppingDistance();

			Finish();		
		}

		void DoGetStoppingDistance()
		{
			if (storeResult == null || _agent ==null) 
			{
				return;
			}

			storeResult.Value = _agent.stoppingDistance;
		}

	}
}