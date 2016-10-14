// (c) Copyright HutongGames, LLC 2010-2014. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.NavMesh)]
	[Tooltip("Gets the layer index for a named layer.")]
	public class NavMeshGetLayerFromName : FsmStateAction
	{	
		[Tooltip("The layer Name")]
		public FsmString layerName;
		
		[ActionSection("Result")]
		
		[Tooltip("Store the layer Index for this layer Name")]
		[UIHint(UIHint.Variable)]
		public FsmInt layerIndex;

		
		public override void Reset()
		{
			layerName = null;
			layerIndex = null;
		}
		
		public override void OnEnter()
		{	
			DoGetLayerFromName();
			
			Finish();		
		}
		
		void DoGetLayerFromName()
		{
			layerIndex.Value = NavMesh.GetNavMeshLayerFromName(layerName.Value);
		}
		

	}
}