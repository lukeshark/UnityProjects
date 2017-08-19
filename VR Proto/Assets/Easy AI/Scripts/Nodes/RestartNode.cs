using UnityEngine;
using System.Collections;
using NodeEditorFramework;
using NodeEditorFramework.Utilities;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System.Linq;
namespace AxlPlay{
[Node (false, "Utils/Restart")]
public class RestartNode : BaseEasyAINode {

	public const string ID = "RestartNode";
	public override string GetID { get { return ID; } }
	public override Node Create (Vector2 pos) 
	{
		RestartNode node = CreateInstance<RestartNode> ();
		
		node.rect = new Rect (pos.x, pos.y, 120, 80);
		node.name = "Restart Node";
		node.titleColor = Color.green;
		
		node.CreateInput ("Value", "Action");
		node.CreateOutput ("Output val", "Conditional");
		node.myType = type.action;
 
		
		return node;
	}
	protected override void NodeGUI()
	{
		 #if UNITY_EDITOR
		GUILayout.Space(10f);
		EditorGUILayout.LabelField("Restart the tree.");
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
            self.GetComponent<EasyAIManager>().restartTree = true;
		
	}
}
}