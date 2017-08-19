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
[Node (false, "Actions/Patrol")]
public class PatrolNode : BaseEasyAINode 
{
	public const string ID = "PatrolNode";
	public override string GetID { get { return ID; } }
	
	
	[Tooltip("Should the agent patrol the waypoints randomly?")]
	public bool randomPatrol = false;
	[Tooltip("The length of time that the agent should pause when arriving at a waypoint")]
	public float waypointPauseDuration = 0f;
	[Tooltip("The Agent speed.")]
	public float AgentSpeed = 3.5f;
	[Tooltip("The waypoints to move to")]
	
	public int numberOfWaypoints;
	
	 // The current index that we are heading towards within the waypoints array
	private int waypointIndex;
	
	private float waypointReachedTime;
	
	private float arrivedDistance = 1f;
	
	private UnityEngine.AI.NavMeshAgent _agent;
	
 
	public List<GameObject> waypoints;
	
	public Rect newRect;
	
 	public int count;
	
	public override Node Create (Vector2 pos) 
	{
		PatrolNode node = CreateInstance<PatrolNode> ();
 
		node.rect = new Rect (pos.x, pos.y, 260, 120);
		node.name = "Patrol Node";
		
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
		
		node.waypoints = new List<GameObject>();
		node.count = 0;
 
		return node;
	}
	
	protected override void NodeGUI () 
	{
	  #if UNITY_EDITOR
		EditorGUILayout.BeginVertical("box");
		
		randomPatrol = EditorGUILayout.Toggle ("Random Patrol", randomPatrol);
		
		waypointPauseDuration = EditorGUILayout.FloatField("Waypoint Pause Duration:",waypointPauseDuration, EditorStyles.label);
		
		AgentSpeed = EditorGUILayout.FloatField("Agent Speed:",AgentSpeed, EditorStyles.label);
		
		EditorGUILayout.EndVertical();
		
		GUILayout.Space(10);
		
		EditorGUILayout.BeginVertical ();
		
		count = EditorGUILayout.IntField("Waypoints: " ,count);
		
		
		if (count < waypoints.Count){
			
			var temp = waypoints.Count -1;
			waypoints.RemoveAt(temp);
		}
		
		
		for (int i = 0; i < count; i++) {
			if (count > waypoints.Count){
				
				waypoints.Add(null);
			}else if (count == waypoints.Count) {
				
				waypoints[i] = (GameObject)EditorGUILayout.ObjectField("Waypoint : ", waypoints[i], typeof(GameObject), true);
			}
			
		}
	 
		newRect = EditorGUILayout.GetControlRect();
		
		if (newRect.y  > 0){
			rect.height  =  50f + newRect.y;
			
		}
		EditorGUILayout.EndVertical (); 
		#endif
	}
	public override void OnStart(EasyAIBlackboard data)
	{
		
		base.OnStart(data);
	 if (!self.GetComponent<UnityEngine.AI.NavMeshAgent>()){
		 
		 self.AddComponent<UnityEngine.AI.NavMeshAgent>();
	 }
		
		_agent = self.GetComponent<UnityEngine.AI.NavMeshAgent>(); 
		
		_agent.Resume();
        	// initially move towards the closest waypoint
		float distance = Mathf.Infinity;
		float localDistance;
		for (int i = 0; i < waypoints.Count; ++i)
		{
			if ((localDistance = Vector3.Magnitude(self.transform.position - waypoints[i].transform.position)) < distance)
			{
				distance = localDistance;
				waypointIndex = i;
			}
		}
		waypointReachedTime = -1;
		_agent.SetDestination(Target());
 		 
	}
	public override BaseEasyAINode.Task OnUpdate(EasyAIBlackboard data)
	{
		base.OnUpdate(data);
		 
		
		if (HasArrived())
		{
			if (waypointReachedTime == -1)
			{
				waypointReachedTime = Time.time;
			}
			
			
            // wait the required duration before switching waypoints.
			if (waypointReachedTime + waypointPauseDuration <= Time.time)
			{
				if (randomPatrol)
				{
					if (waypoints.Count == 1)
					{
						waypointIndex = 0;
					}
					else
					{
                        // prevent the same waypoint from being selected
						var newWaypointIndex = waypointIndex;
						while (newWaypointIndex == waypointIndex)
						{
							newWaypointIndex = Random.Range(0, waypoints.Count);
						}
						waypointIndex = newWaypointIndex;
					}
				}
				else
				{
					waypointIndex = (waypointIndex + 1) % waypoints.Count;
				}
				_agent.SetDestination(Target());
				waypointReachedTime = -1;
			}
		}
		return Task.Running;
	}
		// Return the current waypoint index position
	private Vector3 Target()
	{
		if (waypointIndex >= waypoints.Count)
		{
			return self.transform.position;
		}
		return waypoints[waypointIndex].transform.position;
	}
	
	bool HasArrived()
	{
		if (_agent == null) {
			_agent = self.GetComponent<UnityEngine.AI.NavMeshAgent>(); 
			return false;
		}
		var direction = (_agent.destination - self.transform.position);
			// Do not account for the y difference if it is close to zero.
		if (Mathf.Abs(direction.y) < 0.1f)
		{
			direction.y = 0;
		}
		return !_agent.pathPending && direction.magnitude <= arrivedDistance;
	}
 
    // Draw a gizmo indicating a patrol 
	public override void OnDrawGizmos()
	{
#if UNITY_EDITOR
		if (waypoints == null)
		{
			return;
		}
		var oldColor = UnityEditor.Handles.color;
		UnityEditor.Handles.color = Color.yellow;
		for (int i = 0; i < waypoints.Count; ++i)
		{
			if (waypoints[i] != null)
			{
				UnityEditor.Handles.SphereCap(0, waypoints[i].transform.position, waypoints[i].transform.rotation, 1);
			}
		}
		UnityEditor.Handles.color = oldColor;
#endif
	}
	 
 
	
	
}
}