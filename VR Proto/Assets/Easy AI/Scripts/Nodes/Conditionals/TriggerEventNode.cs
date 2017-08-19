using UnityEngine;
using System.Collections;
using NodeEditorFramework;
using NodeEditorFramework.Utilities;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System.Linq;

namespace AxlPlay{
[Node (false, "Conditionals/TriggerEvent")]
public class TriggerEventNode : BaseEasyAINode {

	public const string ID = "TriggerEventNode";
	public override string GetID { get { return ID; } }
	
	public GameObject targetObject;	
	public string tagStr = "";
	public bool triggerEnter;
	public bool triggerStay;
	public bool triggerExit;
	
	public override Node Create (Vector2 pos) 
	{
		TriggerEventNode node = CreateInstance<TriggerEventNode> ();
		
		node.rect = new Rect (pos.x, pos.y, 260, 150);
		node.name = "Trigger Event";
		
		node.CreateInput ("Value", "Conditional");
		node.CreateOutput ("Output val", "Action");
		
		node.myType = type.conditional;
		node.titleColor = Color.cyan;
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
		tagStr = EditorGUILayout.TagField("Tag for Objects:", tagStr);
		triggerEnter = RTEditorGUI.Toggle(triggerEnter ," => Trigger Enter Event");
		GUILayout.Space(2);
		triggerStay = RTEditorGUI.Toggle(triggerStay ," => Trigger Stay Event");
		GUILayout.Space(2);
		triggerExit = RTEditorGUI.Toggle(triggerExit ," => Trigger Exit Event");
		EditorGUILayout.BeginHorizontal();
 	
		if (!useInternalVar[0]){
			
			targetObject = (GameObject)EditorGUILayout.ObjectField("Collider object:", targetObject, typeof(GameObject), true,GUILayout.Width(235) );
			
			internalVar[0] = "";
			
		}else{
			
			 // to prevent is delete the variable from inspector
			if (choiceIndex[0] >= fsmVars.Count) {
				
				choiceIndex[0]  =0 ;
			}
			EditorGUILayout.LabelField("Collider object:",GUILayout.Width(120));
			
			if (fsmVars.Count  > 0){
				// set to array with the variable type
				
				var results = (from x in fsmVars
					where x.VariableType == "gameobject"
					select x.key).ToArray();
			 
			 
			 
			 
				choiceIndex[0]  = EditorGUILayout.Popup(choiceIndex[0],results ,GUILayout.Height(10),GUILayout.Width(110) );
				
				
				internalVar[0] = results[choiceIndex[0]];
				
				
			}else{
				
				GUILayout.Space(120);
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
		
	}
	public override BaseEasyAINode.Task OnUpdate(EasyAIBlackboard data)
	{
		return Task.Running;
	}
	
	public override Task OnTriggerEnter(Collider other)
	{
 		if (triggerEnter && other.tag == tagStr){
	
	 		if (useInternalVar[0]){
		 			self.GetComponent<EasyAIManager>().data.Add(internalVar[0],other.gameObject);
			}
			return Task.Success;
			
		}
		
	 
		return Task.Running;
	}
	public override BaseEasyAINode.Task OnTriggerStay(Collider other)
	{  
		if (triggerStay && other.tag == tagStr){
			
			if (useInternalVar[0]){
				self.GetComponent<EasyAIManager>().data.Add(internalVar[0],other.gameObject);
			}
			return Task.Success;
			
		}
		return Task.Running;
	}
	public override BaseEasyAINode.Task OnTriggerExit(Collider other)
	{
		if (triggerExit && other.tag == tagStr){
			
			if (useInternalVar[0]){
				self.GetComponent<EasyAIManager>().data.Add(internalVar[0],other.gameObject);
			}
			return Task.Success;
			
		}
		return Task.Running;
	}
}
}
