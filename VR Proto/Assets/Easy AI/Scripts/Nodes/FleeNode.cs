using UnityEngine;
using System.Collections;
using NodeEditorFramework;
using NodeEditorFramework.Utilities;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System.Linq;
namespace AxlPlay{
[Node (false, "Actions/Flee")]
public class FleeNode : BaseEasyAINode {

	public const string ID = "FleeNode";
	public override string GetID { get { return ID; } }
	[Tooltip("The agent has fleed when the magnitude is greater than this value")]
	public float fleedDistance = 10;
	[Tooltip("The distance to look ahead when fleeing")]
	public float lookAheadDistance = 5;
	[Tooltip("The agent has arrived when they are less than the specified distance")]
	public float arrivedDistance = 2;
	
	[Tooltip("The Agent speed.")]
	public float AgentSpeed = 3.5f;
	[Tooltip("Stopping Distance.")]
	public float StoppingDistance = 0.1f;
	public UnityEngine.AI.NavMeshAgent _agent;
	
	public GameObject fleeGameObject;
	
	public bool hasArrived;
	
	public override Node Create (Vector2 pos) 
	{
		FleeNode node = CreateInstance<FleeNode> ();
		
		node.rect = new Rect (pos.x, pos.y, 280, 170);
		node.name = "Flee Node";
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
		
		fleedDistance = EditorGUILayout.FloatField("Flee Distance:",fleedDistance, EditorStyles.label);
		
		lookAheadDistance = EditorGUILayout.FloatField("Look ahead distance:",lookAheadDistance, EditorStyles.label);
		
		arrivedDistance = EditorGUILayout.FloatField("Arrived Distance:",arrivedDistance, EditorStyles.label);
	 EditorGUILayout.BeginHorizontal();
		if (!useInternalVar[0]){
			fleeGameObject = (GameObject)EditorGUILayout.ObjectField("GameObject to Evade:", fleeGameObject, typeof(GameObject), true);
		}else{
			
			 // to prevent is delete the variable from inspector
			if (choiceIndex[0] >= fsmVars.Count()) {
				
				choiceIndex[0]  =0 ;
			}
			EditorGUILayout.LabelField("GameObject to Evade:",GUILayout.Width(120));
			
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
			fleeGameObject = null;
			if (useInternalVar[0]){
				
				useInternalVar[0] = false;
			}else{
				useInternalVar[0] = true;
			}
			
		}
		GUI.color = Color.white;
		EditorGUILayout.EndHorizontal();
		StoppingDistance = EditorGUILayout.FloatField("Stopping Distance:",StoppingDistance, EditorStyles.label);
		
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
			fleeGameObject = (GameObject)data.Get(internalVar[0]);
		}
 
		SetDestination(Target());
	}
	public override BaseEasyAINode.Task OnUpdate(EasyAIBlackboard data)
	{
		base.OnUpdate(data);
		
		SetDestination(Target());
		if (Vector3.Magnitude(self.transform.position - fleeGameObject.transform.position) > fleedDistance)
		{
 
             // finish event
			return Task.Success;
		}else{
			
			return Task.Running;
			
		}
 
	}
	// Flee in the opposite direction
	private Vector3 Target()
	{
		if (fleeGameObject == null) return Vector3.zero;
		return self.transform.position + (self.transform.position - fleeGameObject.transform.position).normalized * lookAheadDistance;
	}
	private bool SetDestination(Vector3 target)
	{
		if (_agent.destination == target)
		{
			return true;
		}
		if (_agent.SetDestination(target))
		{
			return true;
		}
		return false;
	}
	
	private bool HasArrived()
	{
		return !_agent.pathPending && _agent.remainingDistance <= _agent.stoppingDistance + 0.001f;
	}
}
}
