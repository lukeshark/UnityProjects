// (c) Copyright HutongGames, LLC 2010-2014. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.NavMeshAgent)]
	[Tooltip("Sets the cost for traversing over geometry of the layer type. \n " +
		"Cost should be between 1 and infinite. A cost of 3 means that walking 1 meter feels as walking 3 meter when cost is 1. So a higher value means 'more expensive'." +
		 "\nNOTE: The Game Object must have a NavMeshAgent component attached.")]
	public class SetAgentLayerCost : FsmStateAction
	{
		[RequiredField]
		[Tooltip("The Game Object to work with. NOTE: The Game Object must have a NavMeshAgent component attached.")]
		[CheckForComponent(typeof(NavMeshAgent))]
		public FsmOwnerDefault gameObject;
		
		[Tooltip("The layer index.")]
		public FsmInt layer;

		[Tooltip("OR The layer name.")]
		public FsmString ORlayerName;

		[Tooltip("The Layer Cost. A cost of 3 means that walking 1 meter feels as walking 3 meter when cost is 1")]
		public FsmFloat cost;
		
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
			layer = null;
			ORlayerName = null;
			cost = null;
		}

		public override void OnEnter()
		{
			_getAgent();
			
			DoSetLayerCost();

			Finish();		
		}
		
		void DoSetLayerCost()
		{
			if (_agent == null) 
			{
				return;
			}
			
			int layerId = layer.Value;
			if (ORlayerName.Value!=""){
				layerId = NavMesh.GetNavMeshLayerFromName(ORlayerName.Value);
			}
			
			_agent.SetLayerCost(layerId,cost.Value);
			
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