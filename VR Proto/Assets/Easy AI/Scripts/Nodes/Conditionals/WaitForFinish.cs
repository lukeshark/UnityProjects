using UnityEngine;
using System.Collections;
using NodeEditorFramework;
using NodeEditorFramework.Utilities;
namespace AxlPlay{
[Node (false, "Conditionals/WaitForFinish")]
public class WaitForFinish : BaseEasyAINode {
	
	public const string ID = "WaitForFinishNode";
	public override string GetID { get { return ID; } }
	public override Node Create (Vector2 pos) 
	{
		WaitForFinish node = CreateInstance<WaitForFinish> ();
		
		node.rect = new Rect (pos.x, pos.y, 180, 100);
		node.name = "Wait For Finish";
		
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
		
		
	}
	public override BaseEasyAINode.Task OnUpdate(EasyAIBlackboard data)
	{
 
		return Task.Running;
		
	}
	
}
}