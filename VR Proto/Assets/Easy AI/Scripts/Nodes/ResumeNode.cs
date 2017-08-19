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
    [Node(false, "Actions/Resume")]
    public class ResumeNode : BaseEasyAINode
    {
        public const string ID = "ResumeNode";
        public override string GetID { get { return ID; } }

        public GameObject fleeGameObject;
        public override Node Create(Vector2 pos)
        {
            ResumeNode node = CreateInstance<ResumeNode>();

            node.rect = new Rect(pos.x, pos.y, 120, 80);
            node.name = "Resume Node";

            node.CreateInput("Value", "Action");

            node.CreateOutput("Output val", "Conditional");
            node.myType = type.action;
            node.titleColor = Color.cyan;
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
            GUILayout.Label("Resume the Agent");
			#endif
        }
        public override void OnStart(EasyAIBlackboard data)
        {

            base.OnStart(data);
            if (!self.GetComponent<UnityEngine.AI.NavMeshAgent>())
            {

                self.AddComponent<UnityEngine.AI.NavMeshAgent>().Resume();
            }
            else
            {

                self.GetComponent<UnityEngine.AI.NavMeshAgent>().Resume();
            }
        }
        public override BaseEasyAINode.Task OnUpdate(EasyAIBlackboard data)
        {
            base.OnUpdate(data);

            return Task.Success;
        }




    }
}