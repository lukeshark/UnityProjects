using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using NodeEditorFramework;
namespace AxlPlay
{
    public class EasyAIManager : MonoBehaviour
    {

        public EasyAICanvas canvas;
        [HideInInspector]
        public BaseEasyAINode curNode;


         [SerializeField]
        public List<blackboard_go> listGO = new List<blackboard_go>();

        public bool showVars = true;
        public bool showDebugger = true;
        public bool debugConsole;
        [HideInInspector]
        public bool restartTree;

        // for show on inspector
        [HideInInspector]
        public List<string> listNodes = new List<string>();

        private bool deleteConditional;

        private float _timer;

        [HideInInspector]
        public EasyAIBlackboard data;

        void Awake()
        {

            //foreach (var item in listGO)
            //{
            //    Debug.Log(item.key + " >>>> " + item.value);
            //}
        }

        // Use this for initialization
        void Start()
        {

	        
	        if (canvas != null){
	            data = new EasyAIBlackboard(canvas.blackboard, listGO);
	
	            curNode = canvas.GetRootNode();
	
	
	            curNode.self = this.gameObject;
	 
	            curNode.debugConsole = debugConsole;
		        curNode.OnStart(data);
	        }

        }

        // Update is called once per frame
        void Update()
	    {
	    	if (curNode != null){
	            curNode.self = this.gameObject;
	
	            if (curNode.OnUpdate(data) == BaseEasyAINode.Task.Success)
	            {
	
	                // go to next node
	                if (curNode.Outputs[0].connections.Count > 0)
	                {
	
	                    var temp = (BaseEasyAINode)curNode.Outputs[0].connections[0].body;
	                    temp.self = this.gameObject;
		                
		                if (curNode is SetAnimBool || curNode is SetAnimFloat || curNode is SetAnimInt || curNode is SetAnimTrigger)
		                {
			                curNode.debugConsole = debugConsole;
			                curNode.nextState(temp, data);
		                	
		                }
		                else if (curNode is GetGameObjectNode)
	                    {
	
	                        curNode.debugConsole = debugConsole;
	                        curNode.nextState(temp, data);
	
	                    }
	                    else if (curNode is ParallelContidion)
	                    {
	
	                        curNode.debugConsole = debugConsole;
	                        curNode.nextState(temp, data);
	
	                    }
	                    else if (temp.Outputs.Count > 0)
	                    {
	
	                        var nextNode = (BaseEasyAINode)temp.Outputs[0].GetNodeAcrossConnection();
	                        nextNode.self = this.gameObject;
	                        curNode.debugConsole = debugConsole;
	                        curNode.nextState(nextNode, data);
	
	                    }
	
	                }
	
	            } // end if if (curNode.OnUpdate(data) == BaseEasyAINode.Task.Success)
	
	            if (restartTree)
	            {
	                curNode = canvas.GetRootNode();
	
	                curNode.self = this.gameObject;
	                curNode.debugConsole = debugConsole;
	                curNode.OnStart(data);
	
	                restartTree = false;
	            }
	    	}
        }
        void OnTriggerEnter(Collider other)
	    {
		    if (curNode != null){
		    	curNode.self = this.gameObject;
			    curNode.OnTriggerEnter(other);
		    	
		    }
        }
        void OnTriggerStay(Collider other)
	    {
		    if (curNode != null){
		    	curNode.self = this.gameObject;
			    curNode.OnTriggerStay(other);
		    }
        }
        void OnTriggerExit(Collider other)
	    {
		    if (curNode != null){
		    	curNode.self = this.gameObject;
			    curNode.OnTriggerExit(other);
		    }
        }
        void OnCollisionEnter(Collision other)
	    {
		    if (curNode != null){
		    	curNode.self = this.gameObject;
			    curNode.OnCollisionEnter(other);
		    }
        }
        void OnCollisionStay(Collision other)
	    {
		    if (curNode != null){
		    	curNode.self = this.gameObject;
			    curNode.OnCollisionStay(other);
		    }
        }
        void OnCollisionExit(Collision other)
	    {
		    if (curNode != null){
		    	curNode.self = this.gameObject;
			    curNode.OnCollisionExit(other);
		    }
        }
        void OnDrawGizmos()
        {

            if (canvas != null)
            {
                foreach (BaseEasyAINode item in canvas.nodes)
                {
                    item.self = this.gameObject;
                    item.OnDrawGizmos();
                }
            }
        }
    }
}
