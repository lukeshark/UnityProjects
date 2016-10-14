// (c) Copyright HutongGames, LLC 2010-2014. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.NavMeshAgent)]
	[Tooltip("Specifies which NavMesh areas are passable. \n" +
		"Changing AreaMask will make the path stale (see isPathStale). \n" +
		"NOTE: The Game Object must have a NavMeshAgent component attached.")]
	public class SetAgentAreaMask : FsmStateAction
	{
		[RequiredField]
		[Tooltip("The Game Object to work with. NOTE: The Game Object must have a NavMeshAgent component attached.")]
		[CheckForComponent(typeof(NavMeshAgent))]
		public FsmOwnerDefault gameObject;
		
		[Tooltip("Pick the walkable NavMesh areas")]
		public FsmInt NavMeshAreaMask;
		
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
			NavMeshAreaMask =  NavMesh.AllAreas;  // so that by default mask is "everything"
		}

		public override void OnEnter()
		{
			_getAgent();
			
			DoSetAreaMask();

			Finish();		
		}
		
		void DoSetAreaMask()
		{
			if ( _agent == null) 
			{
				return;
			}
			
			_agent.areaMask = NavMeshAreaMask.Value;

		}
	}
}

