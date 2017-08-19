using UnityEngine;
using System.Collections;
using NodeEditorFramework;
using NodeEditorFramework.Utilities;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System.Linq;
namespace AxlPlay{
[Node (false, "Actions/Evade")]
public class EvadeNode : BaseEasyAINode {

	public const string ID = "EvadeNode";
	public override string GetID { get { return ID; } }
	[Tooltip("The agent has evaded when the magnitude is greater than this value")]
	public float evadeDistance = 10;
	[Tooltip("The distance to look ahead when evading")]
	public float lookAheadDistance = 5;
	[Tooltip("How far to predict the distance ahead of the target. Lower values indicate less distance should be predicated")]
	public float targetDistPrediction = 20;
	[Tooltip("Multiplier for predicting the look ahead distance")]
	public float targetDistPredictionMult = 20;
	[Tooltip("The Agent speed.")]
	public float AgentSpeed = 3.5f;
	[Tooltip("Stopping Distance.")]
	public float StopDistance = 0.1f;
	
	public GameObject evadeGameObject;
	// The position of the target at the last frame
	private Vector3 targetPosition;
	private UnityEngine.AI.NavMeshAgent _agent;
 
 
	public override Node Create (Vector2 pos) 
	{
		EvadeNode node = CreateInstance<EvadeNode> ();
		
		node.rect = new Rect (pos.x, pos.y, 280, 170);
		node.name = "Evader Node";
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
		
		EditorGUILayout.BeginVertical();
		
		evadeDistance = EditorGUILayout.FloatField("Evade Distance magnitude:",evadeDistance, EditorStyles.label);
		
		lookAheadDistance = EditorGUILayout.FloatField("Look ahead distance:",lookAheadDistance, EditorStyles.label);
		
		targetDistPrediction = EditorGUILayout.FloatField("target Distance Prediction:",targetDistPrediction, EditorStyles.label);
		
		targetDistPredictionMult = EditorGUILayout.FloatField("Multiplier for predicting:",targetDistPredictionMult, EditorStyles.label);
		EditorGUILayout.BeginHorizontal();
		if (!useInternalVar[0]){
			evadeGameObject = (GameObject)EditorGUILayout.ObjectField("Object to Evade:", evadeGameObject, typeof(GameObject), true);
		}else{
			 // to prevent is delete the variable from inspector
			if (choiceIndex[0] >= fsmVars.Count()) {
				
				choiceIndex[0]  =0 ;
			}
			
			EditorGUILayout.LabelField("Object to Evade:",GUILayout.Width(100));
			
			if (fsmVars.Count() > 0){
				
				var results = (from x in fsmVars
					where x.VariableType == "gameobject"
					select x.key).ToArray();
				
				choiceIndex[0]  = EditorGUILayout.Popup(choiceIndex[0],results,GUILayout.Height(10),GUILayout.Width(110) );
			 
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
			evadeGameObject = null;
			if (useInternalVar[0]){
				
				useInternalVar[0] = false;
			}else{
				useInternalVar[0] = true;
			}
			
		}
		GUI.color = Color.white;
		
		EditorGUILayout.EndHorizontal();
		
		StopDistance = EditorGUILayout.FloatField("Stopping Distance:",StopDistance, EditorStyles.label);
		
		AgentSpeed = EditorGUILayout.FloatField("Agent Speed:",AgentSpeed, EditorStyles.label);
		
		EditorGUILayout.EndVertical();
		#endif
		
	}
	public override void OnStart(EasyAIBlackboard data)
	{
            base.OnStart(data);

            if (!self.GetComponent<UnityEngine.AI.NavMeshAgent>()){
			 
			 self.AddComponent<UnityEngine.AI.NavMeshAgent>();
		 }
		
		_agent = self.GetComponent<UnityEngine.AI.NavMeshAgent>(); 
		if (useInternalVar[0]) {
			evadeGameObject = (GameObject)data.Get(internalVar[0]);
		}
 
	}
	public override BaseEasyAINode.Task OnUpdate(EasyAIBlackboard data)
	{
            base.OnUpdate(data);

		_agent.SetDestination(Target()); 
		
		if (HasArrived()){
			
 			return Task.Success;
		}
		
		return Task.Running;
		
	}
		  // Evade in the opposite direction
	private Vector3 Target()
	{
        // Calculate the current distance to the target and the current speed
		var distance = (evadeGameObject.transform.position - self.transform.position).magnitude;
		var speed = _agent.velocity.magnitude;
		
		float futurePrediction = 0;
        // Set the future prediction to max prediction if the speed is too small to give an accurate prediction
		if (speed <= distance / targetDistPrediction)
		{
			futurePrediction = targetDistPrediction;
		}
		else
		{
			futurePrediction = (distance / speed) * targetDistPredictionMult; // the prediction should be accurate enough
		}
		
        // Predict the future by taking the velocity of the target and multiply it by the future prediction
		var prevTargetPosition = targetPosition;
		targetPosition = evadeGameObject.transform.position;
		var position = targetPosition + (targetPosition - prevTargetPosition) * futurePrediction;
		
		return self.transform.position + (self.transform.position - position).normalized * lookAheadDistance;
	}
	private bool HasArrived()
	{
		return !_agent.pathPending && _agent.remainingDistance <= _agent.stoppingDistance + 0.001f;
	}
	
	
}
}
