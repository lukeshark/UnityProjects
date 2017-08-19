using UnityEngine;
using System.Collections;
using NodeEditorFramework;
using NodeEditorFramework.Utilities;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System.Linq;
namespace AxlPlay{
[Node (false, "Conditionals/SmoothLookAt")]
public class SmoothLookAtNode : BaseEasyAINode {

	public const string ID = "SmoothLookAtNode";
	public override string GetID { get { return ID; } }
	
	public GameObject _target;
	
	public override Node Create (Vector2 pos) 
	{
		SmoothLookAtNode node = CreateInstance<SmoothLookAtNode> ();
		
		node.rect = new Rect (pos.x, pos.y, 250, 90);
		node.name = "SmoothLookAt";
		
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
	protected override void NodeGUI()
	{
		#if UNITY_EDITOR
		GUILayout.Space(20);
  
 		EditorGUILayout.BeginHorizontal();
		
		if (!useInternalVar[0]){
			_target = (GameObject)EditorGUILayout.ObjectField("GameObject to look at:", _target, typeof(GameObject), true);
		}else{
				 // to prevent is delete the variable from inspector
			 // to prevent is delete the variable from inspector
			if (choiceIndex[0] >= fsmVars.Count) {
				
				choiceIndex[0]  =0 ;
			}
			
			EditorGUILayout.LabelField("GameObject to look at:",GUILayout.Width(120));
 
			if (fsmVars.Count  > 0){
				// set to array with the variable type
				
				var results = (from x in fsmVars
					where x.VariableType == "gameobject"
					select x.key).ToArray();
			 
			 
			 
			 
				choiceIndex[0]  = EditorGUILayout.Popup(choiceIndex[0],results ,GUILayout.Height(10),GUILayout.Width(90) );
				
				
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
			_target = null;
			if (useInternalVar[0]){
				
				useInternalVar[0] = false;
			}else{
				useInternalVar[0] = true;
			}
			
		}
		GUI.color = Color.white;
		
		EditorGUILayout.EndHorizontal();
		#endif
 	}
	public override void OnStart(EasyAIBlackboard data)
	{
		if (useInternalVar[0]) {
			_target = (GameObject)data.Get(internalVar[0]);
		}
		Debug.Log("OnStart => " + _target);
	}
	public override BaseEasyAINode.Task OnUpdate(EasyAIBlackboard data)
	{
		if (_target != null)
			RotateTowards(_target.transform);
		return Task.Running;
		
	}
	private void RotateTowards(Transform target)
	{
		Vector3 direction = (target.position - self.transform.position).normalized;
		Quaternion lookRotation = Quaternion.LookRotation(direction);
		self.transform.rotation = Quaternion.Slerp(self.transform.rotation, lookRotation, Time.deltaTime * 5f);
	}
}
}