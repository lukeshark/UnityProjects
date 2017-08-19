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
[Node (false, "Conditionals/CanSee")]
public class CanSeeNode : BaseEasyAINode {

	public const string ID = "CanSeeNode";
	public override string GetID { get { return ID; } }
	
	public bool usePhysics2D;
	//[Tooltip("The object that we are searching for. If this value is null then the objectLayerMask will be used")]
	//public GameObject targetObject;
	[Tooltip("The LayerMask of the objects that we are searching for")]
	public LayerMask objectLayerMask;
	[Tooltip("The LayerMask of the objects to ignore when performing the line of sight check")]
	public LayerMask ignoreLayerMask;
	[Tooltip("The field of view angle of the agent (in degrees)")]
	public float fieldOfViewAngle = 90;
	[Tooltip("The distance that the agent can see")]
	public float viewDistance = 25;
	[Tooltip("The offset relative to the pivot position")]
	public Vector3 offset;
	[Tooltip("The target offset relative to the pivot position")]
	public Vector3 targetOffset;
 
	[Tooltip("The angle offset relative to the pivot position 2D")]
	public float angleOffset2D;
	
	public GameObject targetObject;	
	
	public GameObject returnedObject;
	
	public LayerMask _objectLayerMask;
	public LayerMask _ignoreLayerMask;
	public bool useInverted;
	
	public override Node Create (Vector2 pos) 
	{
		CanSeeNode node = CreateInstance<CanSeeNode> ();
		
		node.rect = new Rect (pos.x, pos.y, 280, 280);
		node.name = "Can See";
		
		node.CreateInput ("Value", "Conditional");
		node.CreateOutput ("Output val", "Action");
		
		node.myType = type.conditional;
		node.titleColor = Color.cyan;
		
		node.myType = type.conditional;
		
		// dropdown index choice
		node.choiceIndex = new int[2];
	 
		// index from fsmVariables
		node.internalVar = new string[2];
		
		// bool for button selected
		node.useInternalVar = new bool[2];
		
		
		return node;
	}
	
	protected override void NodeGUI () 
	{
		
		#if UNITY_EDITOR
		EditorGUILayout.BeginVertical("box");
		usePhysics2D = EditorGUILayout.Toggle("Use 2D Physics:" , usePhysics2D);
		
		angleOffset2D = EditorGUILayout.FloatField("The angle offset 2D: ",angleOffset2D, EditorStyles.label);
	 
		
		EditorGUILayout.BeginHorizontal();
 	
		if (!useInternalVar[0]){
	 
			targetObject = (GameObject)EditorGUILayout.ObjectField("We are searching for:", targetObject, typeof(GameObject), true,GUILayout.Width(235) );
			
			internalVar[0] = "";
			
		}else{
			
			 // to prevent is delete the variable from inspector
			if (choiceIndex[0] >= fsmVars.Count) {
				
				choiceIndex[0]  =0 ;
			}
			EditorGUILayout.LabelField("We are searching for:",GUILayout.Width(120));
			
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
	 
		 _objectLayerMask = EditorGUILayout.LayerField("The Layer searching for", _objectLayerMask);
		
		_ignoreLayerMask =  EditorGUILayout.LayerField("The Layer to ignore", _ignoreLayerMask);
		
		fieldOfViewAngle = EditorGUILayout.FloatField("The field of view angle: ",fieldOfViewAngle, EditorStyles.label);
		
		viewDistance = EditorGUILayout.FloatField("The distance that the agent can see: ",viewDistance, EditorStyles.label);
		
		
		offset = EditorGUILayout.Vector3Field("The offset relative to the pivot position: ",offset);
		
		targetOffset = EditorGUILayout.Vector3Field("The target offset relative to the pivot position: ",targetOffset);
		
		EditorGUILayout.BeginHorizontal();
		
		
		if (!useInternalVar[1]){
			returnedObject = (GameObject)EditorGUILayout.ObjectField("Returned Object:", returnedObject, typeof(GameObject), true);
		
			internalVar[1] = null;
		
		}else{
		 
		 // to prevent is delete the variable from inspector
			if (choiceIndex[1] >= fsmVars.Count()) {
				
				choiceIndex[1]  =0 ;
			}
		 
			EditorGUILayout.LabelField("Returned Object:",GUILayout.Width(120));
			if (fsmVars.Count() > 0){
				
				var results = (from x in fsmVars
					where x.VariableType == "gameobject"
					select x.key).ToArray();
				
				choiceIndex[1]  = EditorGUILayout.Popup(choiceIndex[1],results,GUILayout.Height(10),GUILayout.Width(110) );
			 
				internalVar[1] = results[choiceIndex[1]];
					 
				 
			 
			}else{
				
				GUILayout.Space(120);
			}

		} // end else
		
		
		if (useInternalVar[1]){
			GUI.color = Color.green;
		}else{
			GUI.color = Color.white;
		}
		
		if (GUILayout.Button ("S", GUILayout.Height(18), GUILayout.Width(18))){
			targetObject = null;
			if (useInternalVar[1]){
				
				useInternalVar[1] = false;
			}else{
				useInternalVar[1] = true;
			}
			
		}
		GUI.color = Color.white;
		EditorGUILayout.EndHorizontal();
		GUILayout.BeginHorizontal();
		GUILayout.Label("Use Inverted => NOT See Object:");
		GUILayout.Space(10);
		useInverted = EditorGUILayout.Toggle(useInverted);
		EditorGUILayout.EndVertical();
		//GUILayout.BeginArea (new Rect(140, 290, 20, 20));
	
		//GUILayout.EndArea ();
		EditorGUILayout.EndVertical();
	 
		#endif
	}
	public override void OnStart(EasyAIBlackboard data)
	{
	 
		//Debug.Log(useInternalVar[0]);
		//Debug.Log(internalVar[0]);
		//Debug.Log("------------");
		//Debug.Log(useInternalVar[1]);
		//Debug.Log(internalVar[1]);
		objectLayerMask =  _objectLayerMask;
		ignoreLayerMask =  _ignoreLayerMask;

		//ignoreLayerMask = LayerMask.NameToLayer("Ignore Raycast");
		
		if (useInternalVar[0]) {
 			targetObject = (GameObject)data.Get(internalVar[0]);
		}
 
	}
	public override BaseEasyAINode.Task OnUpdate(EasyAIBlackboard data)
	{
		GameObject _target;
		// only for test purpose must be comment for release
		//if (useInternalVar[0]) {
		//	targetObject = (GameObject)data.Get(internalVar[0]);
		//}
		_target = CanSee();
		
		if (_target == null) {
			if (useInverted) {
				return Task.Success;
			} else {
				
				return Task.Running;
			}
			
		}else{
			
			if (useInverted){
				return Task.Running;
				
			}else{
				if (useInternalVar[1]){
	
					data.Add(internalVar[1],_target);
				}
				return Task.Success;
				
			}
			
			
		}
	}
	public GameObject CanSee()
	{
		if (usePhysics2D)
		{
            // If the target object is null then determine if there are any objects within sight based on the layer mask
			if (targetObject == null)
			{
				returnedObject = AxlPlay.MovementUtility.WithinSight2D(self.transform, offset, fieldOfViewAngle, viewDistance, objectLayerMask, targetOffset, angleOffset2D, ignoreLayerMask);
			}
			else { // If the target is not null then determine if that object is within sight
				
				returnedObject = AxlPlay.MovementUtility.WithinSight2D(self.transform, offset, fieldOfViewAngle, viewDistance, targetObject, targetOffset, angleOffset2D, ignoreLayerMask);
			}
		}
		else {
            // If the target object is null then determine if there are any objects within sight based on the layer mask
			if (targetObject == null)
			{
				returnedObject = AxlPlay.MovementUtility.WithinSight(self.transform, offset, fieldOfViewAngle, viewDistance, objectLayerMask, targetOffset, ignoreLayerMask);
			}
			else { // If the target is not null then determine if that object is within sight
				returnedObject = AxlPlay.MovementUtility.WithinSight(self.transform, offset, fieldOfViewAngle, viewDistance, targetObject, targetOffset, ignoreLayerMask);
			}
		}
		if (returnedObject != null)
		{
            // Return success if an object was found
			return returnedObject;
		}
        // An object is not within sight so return failure
		return null;
		
		
	}
	public override void OnDrawGizmos()
	{
		DrawLineOfSight(self.transform, offset, fieldOfViewAngle, viewDistance, usePhysics2D);
	}
	public static void DrawLineOfSight(Transform transform, Vector3 positionOffset, float fieldOfViewAngle, float viewDistance, bool usePhysics2D)
	{
#if UNITY_EDITOR
		var oldColor = UnityEditor.Handles.color;
		var color = Color.yellow;
		color.a = 0.1f;
		UnityEditor.Handles.color = color;
		
		var halfFOV = fieldOfViewAngle * 0.5f;
		var beginDirection = Quaternion.AngleAxis(-halfFOV, (usePhysics2D ? Vector3.forward : Vector3.up)) * (usePhysics2D ? transform.up : transform.forward);
		UnityEditor.Handles.DrawSolidArc(transform.TransformPoint(positionOffset), (usePhysics2D ? transform.forward : transform.up), beginDirection, fieldOfViewAngle, viewDistance);
		
		UnityEditor.Handles.color = oldColor;
#endif
	}
	
}
}





