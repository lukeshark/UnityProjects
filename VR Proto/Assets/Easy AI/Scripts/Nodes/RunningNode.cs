using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using NodeEditorFramework;
using NodeEditorFramework.Utilities;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace AxlPlay
{
	[Node(false, "Utils/Running")]
    public class RunningNode : BaseEasyAINode
    {
        public const string ID = "RunningNode";
        public override string GetID { get { return ID; } }

        public GameObject fleeGameObject;
        public override Node Create(Vector2 pos)
        {
            RunningNode node = CreateInstance<RunningNode>();

            node.rect = new Rect(pos.x, pos.y, 150, 50);
            node.name = "Running";

            node.CreateInput("Value", "Action");

            node.CreateOutput("Output val", "Conditional");
            node.myType = type.action;
            node.titleColor = Color.red;
            // dropdown index choice
            node.choiceIndex = new int[1];

            // index from fsmVariables
            node.indexVarSelected = new int[1];

            // bool for button selected
            node.useInternalVar = new bool[1];


            return node;
        }

        protected override void NodeGUI()
	    {
		    #if UNITY_EDITOR
            GUILayout.Label("This is a empty Node");
			#endif
  
        }
        public override void OnStart(EasyAIBlackboard data)
        {
            base.OnStart(data);


        }
        public override BaseEasyAINode.Task OnUpdate(EasyAIBlackboard data)
        {
            base.OnUpdate(data);

            return Task.Running;
        }




    }
}