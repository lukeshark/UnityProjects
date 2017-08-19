using UnityEngine;
using System.Collections;
using NodeEditorFramework;
using NodeEditorFramework.Utilities;
namespace AxlPlay{
[Node (false, "Actions/Wander")]
public class WanderNode : BaseEasyAINode {
	
	public const string ID = "WanderNode";
	public override string GetID { get { return ID; } }
	
	[Tooltip("How far ahead of the current position to look ahead for a wander")]
	public float wanderDistance = 20;
	[Tooltip("The amount that the agent rotates direction")]
	public float wanderRate = 2;
	[Tooltip("The Agent speed.")]
	public float AgentSpeed = 3.5f;
 
	public override Node Create (Vector2 pos) 
	{
		WanderNode node = CreateInstance<WanderNode> ();
		
		node.rect = new Rect (pos.x, pos.y, 210, 90);
		node.name = "Wander Node";
		node.titleColor = Color.green;
		
		node.CreateInput ("Value", "Action");
		node.CreateOutput ("Output val", "Conditional");
		node.myType = type.action;
		
		return node;
	}
	
	protected override void NodeGUI () 
	{
		#if UNITY_EDITOR
		
		GUILayout.BeginVertical("box");
		
		wanderDistance = RTEditorGUI.FloatField( new GUIContent("Wander Distance:","How far ahead of the current position to look ahead for a wander"),wanderDistance);
		wanderRate = RTEditorGUI.FloatField("Wander Rate:",wanderRate);
		AgentSpeed = RTEditorGUI.FloatField("Agent Speed:",AgentSpeed);
		
		GUILayout.EndVertical();
		#endif
	}
	public override void OnStart(EasyAIBlackboard data)
	{
		 
		base.OnStart(data);
 
	
		if (!self.GetComponent<UnityEngine.AI.NavMeshAgent>()){
			
			self.AddComponent<UnityEngine.AI.NavMeshAgent>();
		}
 
		SetDestination(Target());
	 
	}
	public override BaseEasyAINode.Task OnUpdate(EasyAIBlackboard data)
	{ 
		base.OnUpdate(data);	
		
 		if (HasArrived())
		{
			SetDestination( Target());
		}
		return Task.Running;
		
	}
	private Vector3 Target()
	{
        // point in a new random direction and then multiply that by the wander distance
		var direction = self.transform.forward + Random.insideUnitSphere * wanderRate;
		return self.transform.position + direction.normalized * wanderDistance;
	}
	private bool SetDestination( Vector3 target)
	{
		if (self.GetComponent<UnityEngine.AI.NavMeshAgent>().destination == target)
		{
			return true;
		}
		if (self.GetComponent<UnityEngine.AI.NavMeshAgent>().SetDestination(target))
		{
			return true;
		}
		return false;
	}
	
	private bool HasArrived()
	{
	 	return !self.GetComponent<UnityEngine.AI.NavMeshAgent>().pathPending && self.GetComponent<UnityEngine.AI.NavMeshAgent>().remainingDistance <= self.GetComponent<UnityEngine.AI.NavMeshAgent>().stoppingDistance + 0.001f;
	}
	 
}
}