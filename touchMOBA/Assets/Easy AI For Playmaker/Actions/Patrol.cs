using UnityEngine;
using System.Collections;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("Agents AI")]
	[Tooltip("Patrol around the specified waypoints using the Unity NavMesh.")]
	public class Patrol : FsmStateAction {

		[RequiredField]
		[CheckForComponent(typeof(NavMeshAgent))]

		[Tooltip("Should the agent patrol the waypoints randomly?")]
		public FsmBool randomPatrol = false;
		[Tooltip("The length of time that the agent should pause when arriving at a waypoint")]
		public FsmFloat waypointPauseDuration = 0;
		[Tooltip("The waypoints to move to")]
		public GameObject[] waypoints;

		public FsmFloat arrivedDistance;

		// The current index that we are heading towards within the waypoints array
		private int waypointIndex;
		private float waypointReachedTime;

		private NavMeshAgent agent;


		public override void Awake() {

			agent = Owner.GetComponent<NavMeshAgent> ();
		}

		public override void OnEnter()
		{
			if (agent == null)
				Finish ();

			// initially move towards the closest waypoint
			float distance = Mathf.Infinity;
			float localDistance;
			for (int i = 0; i < waypoints.Length; ++i) {
				if ((localDistance = Vector3.Magnitude(Owner.transform.position - waypoints[i].transform.position)) < distance) {
					distance = localDistance;
					waypointIndex = i;
				}
			}
			waypointReachedTime = -1;
			agent.SetDestination(Target());
		}

		// Patrol around the different waypoints specified in the waypoint array. 
		public override void OnUpdate()
		{
			if (waypoints.Length == 0) {
				Finish ();
			}
			if (HasArrived()) {
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
								newWaypointIndex = Random.Range(0, waypoints.Length);
							}
							waypointIndex = newWaypointIndex;
						}
					} else {
						waypointIndex = (waypointIndex + 1) % waypoints.Length;
					}
					agent.SetDestination(Target());
					waypointReachedTime = -1;
				}
			}


		}

		// Return the current waypoint index position
		private Vector3 Target()
		{
			if (waypointIndex >= waypoints.Length) {
				return Owner.transform.position;
			}
			return waypoints[waypointIndex].transform.position;
		}


		bool HasArrived()
		{
			var direction = (agent.destination - Owner.transform.position);
			// Do not account for the y difference if it is close to zero.
			if (Mathf.Abs(direction.y) < 0.1f) {
				direction.y = 0;
			}
			return !agent.pathPending && direction.magnitude <= arrivedDistance.Value;
		}


		// Draw a gizmo indicating a patrol 
		public override void OnDrawActionGizmos()
		{
			#if UNITY_EDITOR
			if (waypoints == null) {
				return;
			}
			var oldColor = UnityEditor.Handles.color;
			UnityEditor.Handles.color = Color.yellow;
			for (int i = 0; i < waypoints.Length; ++i) {
				if (waypoints[i] != null) {
					UnityEditor.Handles.SphereCap(0, waypoints[i].transform.position, waypoints[i].transform.rotation, 1);
				}
			}
			UnityEditor.Handles.color = oldColor;
			#endif
		}
		 
	
}

}