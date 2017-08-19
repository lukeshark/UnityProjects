using UnityEngine;
using System.Collections;
using NodeEditorFramework;
using NodeEditorFramework.Utilities;
#if UNITY_EDITOR
using UnityEditor;
#endif
namespace AxlPlay{
	[Node (false, "Conditionals/To Success")]
public class ToSuccess : BaseEasyAINode {

	public const string ID = "ToSuccess";
	public override string GetID { get { return ID; } }
	public override Node Create (Vector2 pos) 
	{
            ToSuccess node = CreateInstance<ToSuccess> ();
		
		node.rect = new Rect (pos.x, pos.y, 120, 80);
		node.name = "To Success";
		
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

            //base.OnStart(data);
		
	}
    public override Task OnUpdate(EasyAIBlackboard data)
    {
        return Task.Success;

    }
    }
}