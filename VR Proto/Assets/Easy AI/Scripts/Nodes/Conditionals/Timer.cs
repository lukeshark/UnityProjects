using UnityEngine;
using System.Collections;
using NodeEditorFramework;
using NodeEditorFramework.Utilities;

namespace AxlPlay
{
    [Node(false, "Conditionals/Timer")]
    public class Timer : BaseEasyAINode
    {
        public const string ID = "TimerNode";
        public override string GetID { get { return ID; } }

        public float _timer;
        public float time;

        public override Node Create(Vector2 pos)
        {
            Timer node = CreateInstance<Timer>();

            node.rect = new Rect(pos.x, pos.y, 150, 60);
            node.name = "Timer Node";

            node.CreateInput("Value", "Conditional");
            node.CreateOutput("Output val", "Action");
            node.myType = type.conditional;
            node.titleColor = Color.cyan;



            return node;
        }

        protected override void NodeGUI()
	    {
		    #if UNITY_EDITOR
		    _timer = RTEditorGUI.FloatField("Time: ", _timer);
		    #endif

        }
        public override void OnStart(EasyAIBlackboard data)
        {

            time = _timer;
 
        }
        public override BaseEasyAINode.Task OnUpdate(EasyAIBlackboard data)
        {
            time = time - Time.deltaTime;

             if (time <= 0)
            {
                 return Task.Success;
            }

            return Task.Running;


        }



    }
}