using UnityEngine;
using System;
using System.Collections.Generic;
using NodeEditorFramework;
using System.Reflection;

namespace AxlPlay{
[Node(true, "Easy AI/Base Node")]
public abstract class BaseEasyAINode : Node
{
	[SerializeField]
	protected string Id;
	[SerializeField]
	protected Texture tex;
	[SerializeField]
	protected Guid guid;

	
	public int [] choiceIndex;
	
	public int [] indexVarSelected;
	
	public bool []  useInternalVar;
	
	public string [] internalVar;
	
	public bool debugConsole;
	
	
	[SerializeField]
	protected bool goToNextState;
	
	public bool hasFinished;
 
	//public bool isTrigger;

	public List<blackboard> fsmVars = new System.Collections.Generic.List<blackboard>();
	
	
	public virtual Task Tick(EasyAIBlackboard data){
		return Task.Running;
	}

	public virtual void Init(EasyAIBlackboard data)
	{
 
		List<NodeOutput> nodes = this.Outputs;
		
		foreach(NodeOutput node in nodes)
		{
			if(node.connections.Count == 0)
				continue;
			
			((BaseEasyAINode)(node.connections[0].body)).Init(data);
		}
		guid = Guid.NewGuid();
		
	 
	}
	
	protected void Init(BaseEasyAINode node)
	{
		node.name = this.GetType().Name;
		node.Id = node.name;
		//node.tex = (Texture)Resources.Load("BehavorIcons/" + node.Id);
		
	 
	}
	#if UNITY_EDITOR
	protected override void NodeGUI()
	{
		GUILayout.BeginHorizontal();
		GUILayout.FlexibleSpace();
		GUILayout.Label(tex);
		GUILayout.FlexibleSpace();
		GUILayout.EndHorizontal();
	}
	#endif
	public override string GetID{get { return Id; } }
	public virtual void  nextState(BaseEasyAINode node,EasyAIBlackboard data){
		
		self.GetComponent<EasyAIManager>().curNode = node;
 
		node.OnStart(data);
		if (debugConsole)
			Debug.Log("nextState=> " + node);
		
	}
	 
	public virtual void OnStart(EasyAIBlackboard data){

		var curNode = self.GetComponent<EasyAIManager>().curNode;
		if (debugConsole)
			Debug.Log("curNode=> " +curNode );
		
		if (curNode.Outputs[0].connections.Count > 0 ){
 
			for (int x = 0; x < curNode.Outputs[0].connections.Count ; x++) {
				
				var temp = (BaseEasyAINode)curNode.Outputs[0].connections [x].body;
				
				if (temp.myType == BaseEasyAINode.type.conditional) {
					temp.self = self;
					temp.OnStart(data);
				}
				
			} 
	 
			
		}
		
	}
	public virtual Task OnUpdate(EasyAIBlackboard data){ 
		
		var _curNode =  (BaseEasyAINode)self.GetComponent<EasyAIManager>().curNode;

		if (_curNode.Outputs[0].connections.Count > 0 ){
			
			for (int x = 0; x < _curNode.Outputs[0].connections.Count ; x++) {
				
				var temp = (BaseEasyAINode)_curNode.Outputs[0].connections [x].body;
				
				if (temp.myType == BaseEasyAINode.type.conditional) {
					
					temp.self = self;
					
					if(temp.OnUpdate(data) == BaseEasyAINode.Task.Success){
						
						if(temp.Outputs.Count >  0 ) {
							
							var nextNode = (BaseEasyAINode) temp.Outputs[0].GetNodeAcrossConnection();

							nextState(nextNode, data);
						} 
						
					} 
				} 
				 
				
			}
			
			 
		
			
		}
		
			
			
		return Task.Running;
	}
	
   
	 
	#region Easy AI
	public enum type {
		
		action,
		conditional
		
	}
	
	public type myType;
	public enum Task {
		Success,
		Failure,
		Running,
		Error
	}
 
	[SerializeField]
	public GameObject self;
	 
	public virtual Task OnTriggerEnter(Collider other){

  
            var _curNode = (BaseEasyAINode)self.GetComponent<EasyAIManager>().curNode;

            if (_curNode.Outputs[0].connections.Count > 0)
            {

                for (int x = 0; x < _curNode.Outputs[0].connections.Count; x++)
                {

                    var temp = (BaseEasyAINode)_curNode.Outputs[0].connections[x].body;
                 
                 
	                if (temp is TriggerEventNode)
                    {
  
	                    temp.self = self;
 
	                    if (temp.OnTriggerEnter(other) == Task.Success) {
		                    
		                    if(temp.Outputs.Count >  0 ) {
			                    
			                    var nextNode = (BaseEasyAINode) temp.Outputs[0].GetNodeAcrossConnection();
			                    
			                    var data =  (EasyAIBlackboard)self.GetComponent<EasyAIManager>().data;
			                    
			                    
			                    nextState(nextNode,data);
		                    }  
	                    }
                    }
 
                } // end for

            }
    

		
		
		return Task.Running;
		
	}
	public virtual Task OnTriggerStay(Collider other){
		
		
		var _curNode = (BaseEasyAINode)self.GetComponent<EasyAIManager>().curNode;
		
		if (_curNode.Outputs[0].connections.Count > 0)
		{
			
			for (int x = 0; x < _curNode.Outputs[0].connections.Count; x++)
			{
				
				var temp = (BaseEasyAINode)_curNode.Outputs[0].connections[x].body;
				
				
				if (temp is TriggerEventNode)
				{
					
					temp.self = self;
					
					if (temp.OnTriggerEnter(other) == Task.Success) {
						
						if(temp.Outputs.Count >  0 ) {
							
							var nextNode = (BaseEasyAINode) temp.Outputs[0].GetNodeAcrossConnection();
							
							var data =  (EasyAIBlackboard)self.GetComponent<EasyAIManager>().data;
							
							
							nextState(nextNode,data);
						}  
					}
				}
				
			} // end for
			
		}
		return Task.Running;
		
	}
	public virtual Task OnTriggerExit(Collider other){
		
		
		var _curNode = (BaseEasyAINode)self.GetComponent<EasyAIManager>().curNode;
		
		if (_curNode.Outputs[0].connections.Count > 0)
		{
			
			for (int x = 0; x < _curNode.Outputs[0].connections.Count; x++)
			{
				
				var temp = (BaseEasyAINode)_curNode.Outputs[0].connections[x].body;
				
				
				if (temp is TriggerEventNode)
				{
					
					temp.self = self;
					
					if (temp.OnTriggerEnter(other) == Task.Success) {
						
						if(temp.Outputs.Count >  0 ) {
							
							var nextNode = (BaseEasyAINode) temp.Outputs[0].GetNodeAcrossConnection();
							
							var data =  (EasyAIBlackboard)self.GetComponent<EasyAIManager>().data;
							
							
							nextState(nextNode,data);
						}  
					}
				}
				
			} // end for
			
		}
		
		return Task.Running;
		
	}
	public virtual Task OnCollisionEnter(Collision other){
		
		
		var _curNode = (BaseEasyAINode)self.GetComponent<EasyAIManager>().curNode;
		
		if (_curNode.Outputs[0].connections.Count > 0)
		{
			
			for (int x = 0; x < _curNode.Outputs[0].connections.Count; x++)
			{
				
				var temp = (BaseEasyAINode)_curNode.Outputs[0].connections[x].body;
				
				
				if (temp is CollisionEventNode)
				{
					
					temp.self = self;
					
					if (temp.OnCollisionEnter(other) == Task.Success) {
						
						if(temp.Outputs.Count >  0 ) {
							
							var nextNode = (BaseEasyAINode) temp.Outputs[0].GetNodeAcrossConnection();
							
							var data =  (EasyAIBlackboard)self.GetComponent<EasyAIManager>().data;
							
							
							nextState(nextNode,data);
						}  
					}
				}
				
			} // end for
			
		}
		
		return Task.Running;
		
	}
	public virtual Task OnCollisionStay(Collision other){
		
		var _curNode = (BaseEasyAINode)self.GetComponent<EasyAIManager>().curNode;
		
		if (_curNode.Outputs[0].connections.Count > 0)
		{
			
			for (int x = 0; x < _curNode.Outputs[0].connections.Count; x++)
			{
				
				var temp = (BaseEasyAINode)_curNode.Outputs[0].connections[x].body;
				
				
				if (temp is CollisionEventNode)
				{
					
					temp.self = self;
					
					if (temp.OnCollisionStay(other) == Task.Success) {
						
						if(temp.Outputs.Count >  0 ) {
							
							var nextNode = (BaseEasyAINode) temp.Outputs[0].GetNodeAcrossConnection();
							
							var data =  (EasyAIBlackboard)self.GetComponent<EasyAIManager>().data;
							
							
							nextState(nextNode,data);
						}  
					}
				}
				
			} // end for
			
		}
		
		return Task.Running;
	}
	public virtual Task OnCollisionExit(Collision other){
		var _curNode = (BaseEasyAINode)self.GetComponent<EasyAIManager>().curNode;
		
		if (_curNode.Outputs[0].connections.Count > 0)
		{
			
			for (int x = 0; x < _curNode.Outputs[0].connections.Count; x++)
			{
				
				var temp = (BaseEasyAINode)_curNode.Outputs[0].connections[x].body;
				
				
				if (temp is CollisionEventNode)
				{
					
					temp.self = self;
					
					if (temp.OnCollisionExit(other) == Task.Success) {
						
						if(temp.Outputs.Count >  0 ) {
							
							var nextNode = (BaseEasyAINode) temp.Outputs[0].GetNodeAcrossConnection();
							
							var data =  (EasyAIBlackboard)self.GetComponent<EasyAIManager>().data;
							
							
							nextState(nextNode,data);
						}  
					}
				}
				
			} // end for
			
		}
		return Task.Running;
	}
	public virtual void OnDrawGizmos(){
		
	}

    public class ActionType : IConnectionTypeDeclaration
    {
	    public string Identifier { get { return "Action"; } }
	    public Type Type { get { return typeof(float); } }  // type to pass
	    public Color Color { get { return Color.red; } }
	    public string InKnobTex { get { return "Textures/In_Knob.png"; } }
	    public string OutKnobTex { get { return "Textures/Out_Knob.png"; } }
    }
	public class ConditionalType : IConnectionTypeDeclaration
    {
	    public string Identifier { get { return "Conditional"; } }
	    public Type Type { get { return typeof(int); } }  // type to pass
	    public Color Color { get { return Color.cyan; } }
	    public string InKnobTex { get { return "Textures/In_Knob.png"; } }
	    public string OutKnobTex { get { return "Textures/Out_Knob.png"; } }
    }
	#endregion
	 
}
}
