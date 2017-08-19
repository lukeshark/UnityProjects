using UnityEngine;
using System.Collections;
using NodeEditorFramework;
using NodeEditorFramework.Utilities;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System.Linq;
namespace AxlPlay
{
    [Node(false, "Actions/Stop")]
    public class StopNode : BaseEasyAINode
    {

        public const string ID = "StopNode";
        public override string GetID { get { return ID; } }
        public override Node Create(Vector2 pos)
        {
            StopNode node = CreateInstance<StopNode>();

            node.rect = new Rect(pos.x, pos.y, 120, 80);
            node.name = "Stop Node";
            node.titleColor = Color.green;

            node.CreateInput("Value", "Conditional", NodeSide.Left, 20);
            node.CreateInput("Value", "Action", NodeSide.Left, 40);
            node.CreateOutput("Output val", "Conditional");
            node.myType = type.action;


            return node;
        }
        protected override void NodeGUI()
	    {
		    #if UNITY_EDITOR
            EditorGUILayout.BeginVertical("box");

            GUILayout.Label("Stop the Agent");
			#endif
        }
        public override void OnStart(EasyAIBlackboard data)
        {
            base.OnStart(data);
            if (!self.GetComponent<UnityEngine.AI.NavMeshAgent>())
            {
            	

                self.AddComponent<UnityEngine.AI.NavMeshAgent>().Stop();
            }
            else
            {

                self.GetComponent<UnityEngine.AI.NavMeshAgent>().Stop();
            }
	       

        }
        public override Task OnUpdate(EasyAIBlackboard data)
        {
            base.OnUpdate(data);

            return Task.Success;
        }
    }
}
