using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using NodeEditorFramework;
using NodeEditorFramework.Utilities;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace AxlPlay{
	[Node (false, "Animator/Set Float")]
public class SetAnimFloat : BaseEasyAINode 
{
	public const string ID = "SetAnimFloat";
	public override string GetID { get { return ID; } }
	
	public string Parameter;		
	public float Value;

    public override Node Create (Vector2 pos) 
	{
        SetAnimFloat node = CreateInstance<SetAnimFloat> ();
 
		node.rect = new Rect (pos.x, pos.y, 260, 120);
		node.name = "Set Anim Float";
		
		node.CreateInput ("Value", "Action");
 
		node.CreateOutput ("Output val", "Action");
		node.myType = type.action;
		node.titleColor = Color.red;

		return node;
	}
	
	protected override void NodeGUI () 
	{
		#if UNITY_EDITOR
		GUILayout.Space(10f);
		Parameter = EditorGUILayout.TextField("Animator Parameter",Parameter);
		Value = EditorGUILayout.FloatField("Value",Value);
		
		#endif


        }
	public override void OnStart(EasyAIBlackboard data)
	{
		base.OnStart(data);
            	 
	}
	public override BaseEasyAINode.Task OnUpdate(EasyAIBlackboard data)
	{
		base.OnUpdate(data);
		
		var _anim = self.GetComponent<Animator>();
		if(_anim != null)
			_anim.SetFloat(Parameter,Value);
		
		return  BaseEasyAINode.Task.Success;
		
	}
	 
 
	
	
}
}