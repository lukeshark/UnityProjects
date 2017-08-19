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
	[Node (true, "Actions/Empty")]
public class SendEventToPlayMaker : BaseEasyAINode 
{
	public const string ID = "Send Event to PM";
	public override string GetID { get { return ID; } }
	
	public GameObject gameObjectFSM;
	
	public string eventName;
	
	
	public override Node Create (Vector2 pos) 
	{
		SendEventToPlayMaker node = CreateInstance<SendEventToPlayMaker> ();
		
		node.rect = new Rect (pos.x, pos.y, 260, 80);
		node.name = "Send Event to PlayMaker";
		node.titleColor = Color.green;
		
		node.CreateInput ("Value", "Action");
		node.CreateOutput ("Output val", "Conditional");
		node.myType = type.action;
			// dropdown index choice
		node.choiceIndex = new int[1];
	 
		// index from fsmVariables
		node.internalVar = new string[1];
		
		// bool for button selected
		node.useInternalVar = new bool[1];
		
		return node;
	}
	
	protected override void NodeGUI () 
	{
		#if UNITY_EDITOR
		
		GUILayout.Space(10);
		EditorGUILayout.BeginHorizontal();
		if (!useInternalVar[0]){
			gameObjectFSM = (GameObject)EditorGUILayout.ObjectField("PlayMaker FSM:", gameObjectFSM, typeof(GameObject), true);
		}else{
			 // to prevent is delete the variable from inspector
			if (choiceIndex[0] >= fsmVars.Count()) {
				
				choiceIndex[0]  =0 ;
			}
			EditorGUILayout.LabelField("PlayMaker FSM:",GUILayout.Width(120));
			
			if (fsmVars.Count() > 0){
				
				
			 var results = (from x in fsmVars
					where x.VariableType == "gameobject"
					select x.key).ToArray();
				
				choiceIndex[0]  = EditorGUILayout.Popup(choiceIndex[0],results,GUILayout.Height(10),GUILayout.Width(110) );
			 
				internalVar[0] = results[choiceIndex[0]];
			}else{
				
			}
			
			
		}
		if (useInternalVar[0]){
			GUI.color = Color.green;
		}else{
			GUI.color = Color.white;
		}
		
		if (GUILayout.Button ("S", GUILayout.Height(18), GUILayout.Width(18))){
			gameObjectFSM = null;
			if (useInternalVar[0]){
				
				useInternalVar[0] = false;
			}else{
				useInternalVar[0] = true;
			}
			
		}
		GUI.color = Color.white;
		EditorGUILayout.EndHorizontal();
	 
		EditorGUILayout.BeginVertical();
		
		eventName = EditorGUILayout.TextField("Event Name:" , eventName, GUILayout.Width(250));
  
		EditorGUILayout.EndVertical();
	 
		#endif
	}
	public override void OnStart(EasyAIBlackboard data)
	{
		PlayMakerFSM   playerFSM =  gameObjectFSM.GetComponent<PlayMakerFSM>();
		
		if (playerFSM != null)
			playerFSM.SendEvent(eventName);
		 
	}
	public override BaseEasyAINode.Task OnUpdate(EasyAIBlackboard data)
	{
		base.OnUpdate(data);
		 
		return Task.Success;
	}
	 
 
	
	
}
}