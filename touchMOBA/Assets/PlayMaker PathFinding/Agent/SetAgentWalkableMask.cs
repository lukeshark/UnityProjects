// (c) Copyright HutongGames, LLC 2010-2014. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.NavMeshAgent)]
	[Tooltip("Specifies witch NavMesh layers are passable (bitfield). \n" +
		"Changing walkableMask will make the path stale (see isPathStale). \n" +
		"NOTE: The Game Object must have a NavMeshAgent component attached.")]
	public class SetAgentWalkableMask : FsmStateAction
	{
		[RequiredField]
		[Tooltip("The Game Object to work with. NOTE: The Game Object must have a NavMeshAgent component attached.")]
		[CheckForComponent(typeof(NavMeshAgent))]
		public FsmOwnerDefault gameObject;
		
		[Tooltip("Pick the walkable NavMesh layers")]
		public FsmInt NavMeshlayerMask;
		
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
			NavMeshlayerMask = -1;  // so that by default mask is "everything"
		}

		public override void OnEnter()
		{
			_getAgent();
			
			DoSetWalkableMask();

			Finish();		
		}
		
		void DoSetWalkableMask()
		{
			if ( _agent == null) 
			{
				return;
			}
			
			_agent.walkableMask = NavMeshlayerMask.Value;

		}
	}
}

