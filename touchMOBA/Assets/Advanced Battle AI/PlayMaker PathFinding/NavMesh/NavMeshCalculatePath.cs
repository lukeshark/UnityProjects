// (c) Copyright HutongGames, LLC 2010-2014. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.NavMesh)]
	[Tooltip("Calculate a path between two points and store the resulting path.")]
	public class NavMeshCalculatePath : FsmStateAction
	{
		[ActionSection("Set up")]
		
		[RequiredField]
		[Tooltip("The mask specifying which NavMesh layers can be passed when calculating the path.")]
		[UIHint(UIHint.FsmInt)]
		public FsmInt passableMask;

		[RequiredField]
		[Tooltip("The initial position of the path requested.")]
		[UIHint(UIHint.FsmVector3)]
		public FsmVector3 sourcePosition;

		[RequiredField]
		[Tooltip("The final position of the path requested.")]
		[UIHint(UIHint.FsmVector3)]
		public FsmVector3 targetPosition;
		
		[ActionSection("Result")]
		
		[RequiredField]
		[Tooltip("The Fsm NavMeshPath proxy component to hold the resulting path")]
		[UIHint(UIHint.Variable)]
		[CheckForComponent(typeof(FsmNavMeshPath))]
		public FsmOwnerDefault calculatedPath;

		
		[Tooltip("True If a resulting path is found.")]
		[UIHint(UIHint.Variable)]
		public FsmBool resultingPathFound;
		
		[Tooltip("Trigger event if resulting path found.")]
		public FsmEvent resultingPathFoundEvent;

		[Tooltip("Trigger event if no path could be found.")]
		public FsmEvent resultingPathNotFoundEvent;	
		
		
		private FsmNavMeshPath _NavMeshPathProxy;
		
		private void _getNavMeshPathProxy()
		{
			GameObject go = calculatedPath.OwnerOption == OwnerDefaultOption.UseOwner ? Owner : calculatedPath.GameObject.Value;
			if (go == null) 
			{
				return;
			}
			
			_NavMeshPathProxy =  go.GetComponent<FsmNavMeshPath>();
		}	
		
		
		public override void Reset()
		{
			calculatedPath = null;
			
			passableMask = -1; // so that by default mask is "everything"
			sourcePosition = null;
			targetPosition = null;
			resultingPathFound = null;
			resultingPathFoundEvent = null;
			resultingPathNotFoundEvent = null;
		}

		public override void OnEnter()
		{	
			DoCalculatePath();

			Finish();		
		}
		
		void DoCalculatePath()
		{
			
			_getNavMeshPathProxy();
			if (_NavMeshPathProxy ==null)
			{
				return;
			}
			 
			
			NavMeshPath _path = new NavMeshPath();
			
			bool _found = NavMesh.CalculatePath(sourcePosition.Value,targetPosition.Value,passableMask.Value,_path);
			
			_NavMeshPathProxy.path = _path;
	
			resultingPathFound.Value = _found;
			
			if (_found)
			{
				if ( ! FsmEvent.IsNullOrEmpty(resultingPathFoundEvent) ){
					Fsm.Event(resultingPathFoundEvent);
				}
			}else
			{
				if (! FsmEvent.IsNullOrEmpty(resultingPathNotFoundEvent) ){
					Fsm.Event(resultingPathNotFoundEvent);
				}
			}
			
			
		}

	}
}