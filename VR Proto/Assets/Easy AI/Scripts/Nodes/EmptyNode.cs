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
public class EmptyNode : BaseEasyAINode 
{
	public const string ID = "EmptyNode";
	public override string GetID { get { return ID; } }
	
	public GameObject fleeGameObject;
	public override Node Create (Vector2 pos) 
	{
		EmptyNode node = CreateInstance<EmptyNode> ();
 
		node.rect = new Rect (pos.x, pos.y, 260, 120);
		node.name = "Empty Node";
		
		node.CreateInput ("Value", "Action");
 
		node.CreateOutput ("Output val", "Conditional");
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
	
	protected override void NodeGUI () 
	{
		#if UNITY_EDITOR
		GUILayout.Label ("This is a custom Node!");
		
		//GUILayout.BeginHorizontal ();
		//GUILayout.BeginVertical ();
		
		//Inputs [0].DisplayLayout ();
		
		//GUILayout.EndVertical ();
		//GUILayout.BeginVertical ();
		
		//Outputs [0].DisplayLayout ();
		
		//GUILayout.EndVertical ();
		//GUILayout.EndHorizontal ();
		
		
		GUILayout.BeginHorizontal();
		  
 		if (!useInternalVar[0]){
	 
	 		fleeGameObject = (GameObject)EditorGUILayout.ObjectField("GameObject:", fleeGameObject, typeof(GameObject), true , GUILayout.Width(235) );
			
		}else{
			
			 // to prevent is delete the variable from inspector
			if (choiceIndex[0] >= fsmVars.Count) {
				
				choiceIndex[0]  =0 ;
			}
			EditorGUILayout.LabelField("GameObject:",GUILayout.Width(120));
			
			if (fsmVars.Count  > 0){
				// set to array with the variable type
			  
				var results = (from x in fsmVars
					where x.VariableType == "gameobject"
					select x.key).ToArray();
			 
			 
			 
			 
				choiceIndex[0]  = EditorGUILayout.Popup(choiceIndex[0],results ,GUILayout.Height(10),GUILayout.Width(60) );
				
				for (int i = 0; i < fsmVars.Count; i++) {
					if (fsmVars.Count  > 0 &&   fsmVars[i] == fsmVars[choiceIndex[0]] ){
						
						indexVarSelected[0] = i;
						
 					break;
					}
				}
				
				
				
			} 
		}
	 
		 
		if (useInternalVar[0]){
			GUI.color = Color.green;
		}else{
			GUI.color = Color.white;
		}
		
		if (GUILayout.Button ("S", GUILayout.Height(18), GUILayout.Width(18))){
		 
			if (useInternalVar[0]){
				
				useInternalVar[0] = false;
			}else{
				useInternalVar[0] = true;
			}
			
		}
		GUI.color = Color.white;
 		GUILayout.EndHorizontal();
	 #endif
		
	}
	public override void OnStart(EasyAIBlackboard data)
	{
	 
		base.OnStart(data);
		 
	}
	public override BaseEasyAINode.Task OnUpdate(EasyAIBlackboard data)
	{
		base.OnUpdate(data);
		 
		return Task.Running;
	}
	 
 
	
	
}
}