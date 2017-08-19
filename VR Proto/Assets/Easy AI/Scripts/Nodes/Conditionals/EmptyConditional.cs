using UnityEngine;
using System.Collections;
using NodeEditorFramework;
using NodeEditorFramework.Utilities;
#if UNITY_EDITOR
using UnityEditor;
#endif
namespace AxlPlay{
	[Node (true, "Conditionals/Empty")]
public class EmptyConditional : BaseEasyAINode {

	public const string ID = "EmptyConditional";
	public override string GetID { get { return ID; } }
	public override Node Create (Vector2 pos) 
	{
		EmptyConditional node = CreateInstance<EmptyConditional> ();
		
		node.rect = new Rect (pos.x, pos.y, 180, 150);
		node.name = "Empty";
		
		node.CreateInput ("Value", "Conditional");
		node.CreateOutput ("Output val", "Action");
		
		node.myType = type.conditional;
		node.titleColor = Color.cyan;
 
		return node;
	}
	protected override void NodeGUI()
	{
		 
	}
	
	public override void OnStart(EasyAIBlackboard data)
	{
		
		Debug.Log("x aqui");
		
	}
}
}