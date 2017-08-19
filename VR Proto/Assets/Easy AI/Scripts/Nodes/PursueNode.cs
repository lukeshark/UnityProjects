using UnityEngine;
using System.Collections;
using System.Linq;
using NodeEditorFramework;
using NodeEditorFramework.Utilities;
#if UNITY_EDITOR
using UnityEditor;
#endif


 namespace AxlPlay{
[Node (false, "Actions/Pursue")]
public class PursueNode : BaseEasyAINode {
 public const string ID = "PursueNode";
	public override string GetID { get { return ID; } }
	
	[Tooltip("How far to predict the distance ahead of the target. Lower values indicate less distance should be predicated")]
	public float targetDistPrediction = 20;
	[Tooltip("Multiplier for predicting the look ahead distance")]
	public float targetDistPredictionMult = 20;
	
	[Tooltip("The agent has arrived when they are less than the specified distance")]
	public float arrivedDistance = 0.1f;
	[Tooltip("The Agent speed.")]
	public float AgentSpeed = 3.5f;
	
	public GameObject pursueGameObject;
	
	// The position of the target at the last frame
	public Vector3 targetPosition;
	
	public UnityEngine.AI.NavMeshAgent _agent;
	
	public override Node Create (Vector2 pos) 
	{
		PursueNode node = CreateInstance<PursueNode> ();
		
		node.rect = new Rect (pos.x, pos.y, 260, 120);
		node.name = "Pursue Node";
		
		node.CreateInput ("Value", "Action");
		
		node.CreateOutput ("Output val", "Conditional");
		node.myType = type.action;
		node.titleColor = Color.green;
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
		EditorGUILayout.BeginVertical("box");
		 
		
		targetDistPrediction = EditorGUILayout.FloatField("Target Distance Prediction:",targetDistPrediction, EditorStyles.label);
		targetDistPredictionMult = EditorGUILayout.FloatField("Predicting the look ahead distance:",targetDistPredictionMult, EditorStyles.label);
		EditorGUILayout.BeginHorizontal();
		if (!useInternalVar[0]){
			pursueGameObject = (GameObject)EditorGUILayout.ObjectField("GameObject to Pursue:", pursueGameObject, typeof(GameObject), true);
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
			pursueGameObject = null;
			if (useInternalVar[0]){
				
				useInternalVar[0] = false;
			}else{
				useInternalVar[0] = true;
			}
			
		}
		GUI.color = Color.white;
		EditorGUILayout.EndHorizontal();
		arrivedDistance = EditorGUILayout.FloatField("Arrived Distance:",arrivedDistance, EditorStyles.label);
		
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
			pursueGameObject = (GameObject)data.Get(internalVar[0]);
		}
		
		if (pursueGameObject == null)
		{
			
			Debug.Log("Please set the target to Pursue");
		}
		else {
			_agent.SetDestination(Target());
		}


            //Debug.Log(self);
	}
	public override BaseEasyAINode.Task OnUpdate(EasyAIBlackboard data)
	{
		base.OnUpdate(data);
		
		 // prevent if target == null 
		if(pursueGameObject == null) return Task.Failure;
		
		_agent.SetDestination(Target());
		if (HasArrived())
		{
			return Task.Success;
		}
		
		return Task.Running;
	}
	public bool HasArrived()
	{
		
		var direction = (_agent.destination - self.transform.position);
        // Do not account for the y difference if it is close to zero.
		if (Mathf.Abs(direction.y) < 0.1f)
		{
			direction.y = 0;
		}
		return !_agent.pathPending && direction.magnitude <= arrivedDistance;
	}
    // Predict the position of the target
	private Vector3 Target()
	{
        // Calculate the current distance to the target and the current speed
		var distance = (pursueGameObject.transform.position - self.transform.position).magnitude;
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
		targetPosition = pursueGameObject.transform.position;
		return targetPosition + (targetPosition - prevTargetPosition) * futurePrediction;
	}
}
 }