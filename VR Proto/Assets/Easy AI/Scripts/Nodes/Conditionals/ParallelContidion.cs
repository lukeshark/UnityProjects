using UnityEngine;
using System.Collections;
using NodeEditorFramework;
namespace AxlPlay{ 
	[Node (false, "Conditionals/Parallel Contidion")]
	public class ParallelContidion : BaseEasyAINode 
	{
		public const string ID = "ParallelContidion";
		public override string GetID { get { return ID; } }
		
		public override bool AllowRecursion { get { return true; } }
		public override bool ContinueCalculation { get { return true; } }
		
		public override Node Create (Vector2 pos) 
		{
			ParallelContidion node = CreateInstance<ParallelContidion> ();
			
			node.rect = new Rect (pos.x, pos.y, 110, 140);
			node.name = "Parallel Node";
			
			node.CreateInput ("Input Top", "Action", NodeSide.Left, 40);
			node.CreateInput ("Input Bottom", "Action", NodeSide.Left, 60);
			node.CreateInput ("Input Right", "Action", NodeSide.Left, 80);
			node.CreateInput ("Input Left", "Action", NodeSide.Left, 100);
			
			node.CreateOutput ("Output Top", "Action", NodeSide.Right, 40);
			
			node.myType = type.action;
 	
			node.titleColor = Color.red;
			return node;
		}
 
		protected override void NodeGUI () 
		{
		 
		}
		public override BaseEasyAINode.Task OnUpdate(EasyAIBlackboard data)
		{
			return Task.Success;
		}
		public override bool Calculate () 
		{
			Outputs [0].SetValue<float> (Inputs [0].GetValue<float> ());
			 
			
			return true;
		}
	}
 
}
