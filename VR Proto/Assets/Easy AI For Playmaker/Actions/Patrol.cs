using UnityEngine;
using System.Collections;
#if UNITY_5_5_OR_NEWER
using UnityEngine.AI;
#endif
namespace HutongGames.PlayMaker.Actions
{
	[RequireComponent(typeof( NavMeshAgent))]
	[ActionCategory ("Agents AI")]
	[Tooltip ("Patrol around the specified waypoints using the Unity NavMesh.")]
	public class Patrol : FsmStateAction
	{

		[RequiredField]
		[CheckForComponent (typeof(NavMeshAgent))]

		[Tooltip ("Should the agent patrol the waypoints randomly?")]
		public FsmBool randomPatrol = false;
		[Tooltip ("The length of time that the agent should pause when arriving at a waypoint")]
		public FsmFloat waypointPauseDuration = 0;
		
		
		[ArrayEditor(VariableType.GameObject)]
		public FsmArray waypoints;
		
		
		//[Tooltip ("The waypoints to move to")]
		//public GameObject[] waypoints;

		public FsmFloat arrivedDistance;

		// The current index that we are heading towards within the waypoints array
		private int waypointIndex;
		private float waypointReachedTime;

		private NavMeshAgent agent;
 
		public override void OnEnter ()
		{
			
			agent = Owner.GetComponent<NavMeshAgent> ();
			
			if (agent == null)
				Finish ();

			// initially move towards the closest waypoint
			float distance = Mathf.Infinity;
			float localDistance;
			for (int i = 0; i < waypoints.Length; ++i) {
				
			 var temp = (GameObject)waypoints.Get(i);
				 
				
				if ((localDistance = Vector3.Magnitude (Owner.transform.position - temp.transform.position)) < distance) {
					distance = localDistance;
					waypointIndex = i;
				}
			}
			waypointReachedTime = -1;
			agent.SetDestination (Target ());
		}

		// Patrol around the different waypoints specified in the waypoint array.
		public override void OnUpdate ()
		{
			if (waypoints.Length == 0) {
				Finish ();
			}
			if (HasArrived ()) {
				if (waypointReachedTime == -1) {
					waypointReachedTime = Time.time;
				}
				// wait the required duration before switching waypoints.
				if (waypointReachedTime + waypointPauseDuration.Value <= Time.time) {
					if (randomPatrol.Value) {
						if (waypoints.Length == 1) {
							waypointIndex = 0;
						} else {
							// prevent the same waypoint from being selected
							var newWaypointIndex = waypointIndex;
							while (newWaypointIndex == waypointIndex) {
								newWaypointIndex = Random.Range (0, waypoints.Length);
							}
							waypointIndex = newWaypointIndex;
						}
					} else {
						waypointIndex = (waypointIndex + 1) % waypoints.Length;
					}
					agent.SetDestination (Target ());
					waypointReachedTime = -1;
				}
			}


		}

		// Return the current waypoint index position
		private Vector3 Target ()
		{
			
			var temp =  (GameObject)waypoints.Get(waypointIndex);
			
			if (waypointIndex >= waypoints.Length) {
				return Owner.transform.position;
			}
			return temp.transform.position;
		}


		bool HasArrived ()
		{
			var direction = (agent.destination - Owner.transform.position);
			// Do not account for the y difference if it is close to zero.
			if (Mathf.Abs (direction.y) < 0.1f) {
				direction.y = 0;
			}
			return !agent.pathPending && direction.magnitude <= arrivedDistance.Value;
		}

		public override void OnExit ()
		{

			Finish ();


		}

		// Draw a gizmo indicating a patrol
		public override void OnDrawActionGizmos ()
		{
			#if UNITY_EDITOR
			if (waypoints == null) {
				return;
			}
			var oldColor = UnityEditor.Handles.color;
			UnityEditor.Handles.color = Color.yellow;
			for (int i = 0; i < waypoints.Length; ++i) {
				
				var temp =  (GameObject)waypoints.Get(waypointIndex);
				if (temp != null) {
					UnityEditor.Handles.SphereCap (0, temp.transform.position, temp.transform.rotation, 1);
				}
			}
			UnityEditor.Handles.color = oldColor;
			#endif
		}
		 
	
	}

}