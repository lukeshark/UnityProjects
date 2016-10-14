// (c) Copyright HutongGames, LLC 2010-2014. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.NavMeshAgent)]
	[Tooltip("Gets the cost for traversing over geometry of the layer type. \n" +
		"If you enable or disable the Agent, the cost will be reset to the default layer cost. \n" +
		"NOTE: The Game Object must have a NavMeshAgent component attached.")]
	public class GetAgentLayerCost : FsmStateAction
	{
		[ActionSection("Set up")]
		
		[RequiredField]
		[Tooltip("The Game Object to work with. NOTE: The Game Object must have a NavMeshAgent component attached.")]
		[CheckForComponent(typeof(NavMeshAgent))]
		public FsmOwnerDefault gameObject;
		
		[Tooltip("The layer index.")]
		public FsmInt layer;

		[Tooltip("OR The layer name.")]
		public FsmString ORlayerName;
		
		[ActionSection("Result")]
		
		[Tooltip("Store the Layer Cost")]
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
			layer = null;
			ORlayerName = null;
			storeResult = null;
		}

		public override void OnEnter()
		{
			_getAgent();
			
			DoGetLayerCost();

			Finish();		
		}
		
		void DoGetLayerCost()
		{
			if (_agent == null) 
			{
				return;
			}
			
			int layerId = layer.Value;
			if (ORlayerName.Value!=""){
				
				layerId = NavMesh.GetNavMeshLayerFromName(ORlayerName.Value);
			}
			
			storeResult.Value =	_agent.GetLayerCost(layerId);
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