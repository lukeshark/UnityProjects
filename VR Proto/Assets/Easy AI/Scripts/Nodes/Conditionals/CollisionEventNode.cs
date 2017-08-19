using UnityEngine;
using System.Collections;
using NodeEditorFramework;
using NodeEditorFramework.Utilities;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System.Linq;

namespace AxlPlay{
[Node (false, "Conditionals/CollisionEvent")]
public class CollisionEventNode : BaseEasyAINode {

	public const string ID = "CollisionEventNode";
	public override string GetID { get { return ID; } }
	
	public GameObject targetObject;	
 	public bool CollisionEnter;
	public bool CollisionStay;
	public bool CollisionExit;
	
	public override Node Create (Vector2 pos) 
	{
		CollisionEventNode node = CreateInstance<CollisionEventNode> ();
		
		node.rect = new Rect (pos.x, pos.y, 260, 150);
		node.name = "Collision Event";
		
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
 		CollisionEnter = RTEditorGUI.Toggle(CollisionEnter ," => Collision Enter Event");
		GUILayout.Space(2);
		CollisionStay = RTEditorGUI.Toggle(CollisionStay ," => Collision Stay Event");
		GUILayout.Space(2);
		CollisionExit = RTEditorGUI.Toggle(CollisionExit ," => Collision Exit Event");
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
	public override BaseEasyAINode.Task OnCollisionEnter(Collision other)
	{
		
		if (CollisionEnter){
			
			if (useInternalVar[0]){
				self.GetComponent<EasyAIManager>().data.Add(internalVar[0],other.gameObject);
			}
			return Task.Success;
			
		}
		return Task.Running;
	}
	public override BaseEasyAINode.Task OnCollisionStay(Collision other)
	{
		if (CollisionStay){
			
			if (useInternalVar[0]){
				self.GetComponent<EasyAIManager>().data.Add(internalVar[0],other.gameObject);
			}
			return Task.Success;
			
		}
		return Task.Running;
	}
	public override BaseEasyAINode.Task OnCollisionExit(Collision other)
	{
		if (CollisionExit){
			
			if (useInternalVar[0]){
				self.GetComponent<EasyAIManager>().data.Add(internalVar[0],other.gameObject);
			}
			return Task.Success;
			
		}
		return Task.Running;
	}
	
}
}