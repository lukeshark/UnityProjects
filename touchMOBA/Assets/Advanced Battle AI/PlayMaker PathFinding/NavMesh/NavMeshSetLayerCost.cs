// (c) Copyright HutongGames, LLC 2010-2014. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.NavMesh)]
	[Tooltip("Sets the cost for traversing over geometry of the layer type on all agents.\n" +
		"Cost should be between 1 and infinite. A cost of 3 means that walking 1 meter feels as walking 3 meter when cost is 1. So a higher value means 'more expensive'.")]
	public class NavMeshSetLayerCost : FsmStateAction
	{

		[Tooltip("The layer index.")]
		public FsmInt layer;

		[Tooltip("OR The layer name.")]
		public FsmString ORlayerName;
		
		[Tooltip("The Layer Cost")]
		public FsmFloat cost;
		
		
		public override void Reset()
		{
			layer = null;
			ORlayerName = null;
			cost = null;
		}

		public override void OnEnter()
		{
			DoSetLayerCost();

			Finish();		
		}
		
		void DoSetLayerCost()
		{
			
			int layerId = layer.Value;
			if (ORlayerName.Value!=""){
				layerId = NavMesh.GetNavMeshLayerFromName(ORlayerName.Value);
			}
			
			NavMesh.SetLayerCost(layerId,cost.Value);
			
		}
		
		public override string ErrorCheck()
		{
			
			if (ORlayerName.Value!="")
			{
				int layerID = NavMesh.GetNavMeshLayerFromName(ORlayerName.Value);
				if (layerID==-1){
					return "Layer Name '"+ORlayerName.Value+"' doesn't exists";
				}else if(layer.Value != 0){
					if (layerID == layer.Value){
						return "Layer reference redundancy. Use Layer OR Layer Name.";
					}else{
						return "Layer conflict, layer name '"+ORlayerName.Value+"' will be used";
					}
					
				}
			}
			
			return "";
		}

		
	}
}