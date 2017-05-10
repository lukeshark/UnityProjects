using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor (typeof(Pursue))]

public class ArriveDistanceEditor : Editor
{

	Pursue _target;

	void OnEnable ()
	{
		if (_target == null) {
			_target = (Pursue)target;
			_target.agent = _target.gameObject.GetComponent<UnityEngine.AI.NavMeshAgent> ();

		}
	}

	public override void OnInspectorGUI ()
	{
		DrawDefaultInspector ();

		if (_target.arrivedDistance < _target.agent.stoppingDistance || _target.arrivedDistance == _target.agent.stoppingDistance) {
			_target.arrivedDistance = _target.agent.stoppingDistance + 0.1f;
			Debug.Log ("The arrivedDistance cannot be less or equal than Nav Mesh Agent Stopping Distance");

		}
		if (_target.agent.stoppingDistance <= 0) {
			_target.agent.stoppingDistance = 1;
			Debug.Log ("The Nav Mesh Agent Stopping Distance cannot be less or equal than 0");
		}
	}
}


public class CustomPursue : Editor
{

	Pursue _target;

	void OnEnable ()
	{
		if (_target == null) {
			_target = (Pursue)target;
			_target.agent = _target.gameObject.GetComponent<UnityEngine.AI.NavMeshAgent> ();

		}
	}

	public override void OnInspectorGUI ()
	{
		DrawDefaultInspector ();

		if (_target.arrivedDistance < _target.agent.stoppingDistance || _target.arrivedDistance == _target.agent.stoppingDistance) {
			_target.arrivedDistance = _target.agent.stoppingDistance + 0.1f;
			Debug.Log ("The arrivedDistance cannot be less or equal than Nav Mesh Agent Stopping Distance");

		}
		if (_target.agent.stoppingDistance <= 0) {
			_target.agent.stoppingDistance = 1;
			Debug.Log ("The Nav Mesh Agent Stopping Distance cannot be less or equal than 0");
		}
	}
}

public class CustomSetDestination : Editor
{

	SetDestination _target;

	void OnEnable ()
	{
		if (_target == null) {
			_target = (SetDestination)target;
			_target.agent = _target.gameObject.GetComponent<UnityEngine.AI.NavMeshAgent> ();

		}
	}

	public override void OnInspectorGUI ()
	{
		DrawDefaultInspector ();

		if (_target.arrivedDistance < _target.agent.stoppingDistance || _target.arrivedDistance == _target.agent.stoppingDistance) {
			_target.arrivedDistance = _target.agent.stoppingDistance + 0.1f;
			Debug.Log ("The arrivedDistance cannot be less or equal than Nav Mesh Agent Stopping Distance");

		}
		if (_target.agent.stoppingDistance <= 0) {
			_target.agent.stoppingDistance = 1;
			Debug.Log ("The Nav Mesh Agent Stopping Distance cannot be less or equal than 0");
		}
	}
}

public class CustomFlee : Editor
{

	Flee _target;

	void OnEnable ()
	{
		if (_target == null) {
			_target = (Flee)target;
			_target.agent = _target.gameObject.GetComponent<UnityEngine.AI.NavMeshAgent> ();

		}
	}

	public override void OnInspectorGUI ()
	{
		DrawDefaultInspector ();

		if (_target.arrivedDistance < _target.agent.stoppingDistance || _target.arrivedDistance == _target.agent.stoppingDistance) {
			_target.arrivedDistance = _target.agent.stoppingDistance + 0.1f;
			Debug.Log ("The arrivedDistance cannot be less or equal than Nav Mesh Agent Stopping Distance");

		}
		if (_target.agent.stoppingDistance <= 0) {
			_target.agent.stoppingDistance = 1;
			Debug.Log ("The Nav Mesh Agent Stopping Distance cannot be less or equal than 0");
		}
	}
}

