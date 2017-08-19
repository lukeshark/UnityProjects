using UnityEngine;
using System.Collections;
using NodeEditorFramework;
using NodeEditorFramework.Utilities;

namespace AxlPlay{
[Node (false, "Start")]
	public class StartNode : BaseEasyAINode 
	{
		public const string ID = "startNode";
		public override string GetID { get { return ID; } }
		
		public override Node Create (Vector2 pos) 
		{
			StartNode node = CreateInstance<StartNode> ();
			
			node.rect = new Rect (pos.x, pos.y, 150, 60);
			node.name = "Start";
			node.titleColor = Color.red;
			
			//node.CreateInput ("Value", "Float");
			node.CreateOutput ("Start Point", "Action");
			
			return node;
		}
		
		protected  override void NodeGUI () 
		{
			GUILayout.Label ("This is Start Node!");
			
			GUILayout.BeginHorizontal ();
		 
			GUILayout.BeginVertical ();
			
			Outputs [0].DisplayLayout ();
			
			GUILayout.EndVertical ();
			GUILayout.EndHorizontal ();
			
		}
		public override void OnStart(EasyAIBlackboard data)
		{
 	
 		}
		
		
	}
	
}