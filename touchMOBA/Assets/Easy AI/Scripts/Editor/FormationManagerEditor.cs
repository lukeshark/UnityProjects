using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor (typeof(FormationManager))]
public class FormationManagerEditor : Editor
{
	int index = 0;
	string[] options = new string[] { "Cool", "Great", "Awesome" };
	string selectedTag = "";
	int selectedLayer = 0;

	public override void OnInspectorGUI ()
	{
		

		FormationManager _target = (FormationManager)target;


		EditorGUILayout.BeginVertical ("box");

		FormationManager.Formations newValue = (FormationManager.Formations)EditorGUILayout.EnumPopup ("Formation Type:", _target.formation);

		switch (newValue) {
		case FormationManager.Formations.SemiCircle:
			
			_target.radius = EditorGUILayout.FloatField (new GUIContent ("Radius", "The radius of the semicircle"), _target.radius);
			_target.concave = EditorGUILayout.Toggle (new GUIContent ("Concave", "If you want that the semicircle will be concave"), _target.concave);

			break;
		case FormationManager.Formations.Circle:

			_target.radius = EditorGUILayout.FloatField (new GUIContent ("Radius", "The radius of the semicircle"), _target.radius);

			break;
		case FormationManager.Formations.V:

			_target._separation = EditorGUILayout.FloatField (new GUIContent ("Separation", "The separation between agents in the formation."), _target._separation);


			break;
		case FormationManager.Formations.Triangle:
			_target.length = EditorGUILayout.FloatField (new GUIContent ("Length", "The separation between agents in the formation."), _target.length);


			break;
		case FormationManager.Formations.Wedge:
			_target._separation = EditorGUILayout.FloatField (new GUIContent ("Separation", "The separation between agents in the formation."), _target._separation);
			_target.fill = EditorGUILayout.Toggle (new GUIContent ("Fill", "If you want that the agents fill the formation"), _target.fill);

			break;
		default:
			break;
		}


		if (newValue != _target.formation) {
			_target.formation = newValue;

			EditorUtility.SetDirty (target);
		}
		EditorGUILayout.EndVertical ();
		EditorGUILayout.Space ();

		EditorGUILayout.BeginHorizontal ("box");
		EditorGUILayout.BeginVertical ();
		EditorGUILayout.Space ();
		EditorGUILayout.LabelField ("Select the tag of the Agents", EditorStyles.boldLabel);
		EditorGUILayout.Space ();
		EditorGUILayout.BeginHorizontal ();
		EditorGUILayout.Space ();
		_target.selectedTag = EditorGUILayout.TagField ("Tag of Agents:", _target.selectedTag);


		if (_target.agentes != null && _target.agentes.Length == 0) {
			GUI.color = Color.red;
		} else {
			GUI.color = Color.green;
			if (_target.leaderAgent == null) {
				
				_target.leaderAgent = _target.agentes [_target.agentsNum - 1];
				Debug.Log (_target.agentsNum - 1);
			}
		}
       

		if (GUILayout.Button ("Load Agents from tag")) {
			_target.loadFromTags ();
			_target.LoadAgentList ();
			EditorUtility.SetDirty (target);
		}
		GUI.color = Color.white;
		if (GUILayout.Button ("Test")) {
			_target.ListTest ();
		}

		EditorGUILayout.EndHorizontal ();
		EditorGUILayout.EndVertical ();
		EditorGUILayout.EndHorizontal ();
 
	}

	void OnSceneGUI ()
	{
		FormationManager _target = (FormationManager)target;

		switch (_target.formation) {
		case FormationManager.Formations.SemiCircle:
			_target.theta = Mathf.PI / (_target.agentsNum * 2);
			for (int i = 0; i < _target.agentsNum; i++) {
				var pos = _target.GetSemiCirclePos (i, _target.zLookAhead);
				if (_target.leaderAgent != null) {
					pos = pos + _target.leaderAgent.transform.position;
				}
				Handles.color = Color.red;
				Handles.SphereCap (i, pos, Quaternion.identity, 0.5f);
			}
			break;
		case FormationManager.Formations.Circle:
			_target.theta = Mathf.PI / (_target.agentsNum);
			for (int i = 0; i < _target.agentsNum; i++) {
				var pos = _target.GetCirclePos (i, _target.zLookAhead);
				if (_target.leaderAgent != null) {
					pos = pos + _target.leaderAgent.transform.position;
				}
				Handles.color = Color.red;
				Handles.SphereCap (i, pos, Quaternion.identity, 0.5f);
			}
			break;
		case FormationManager.Formations.V:
			_target.separation = new Vector2 (_target._separation, _target._separation);
			for (int i = 0; i < _target.agentsNum; i++) {
				var pos = _target.GetVPosition (i, _target.zLookAhead);
				if (_target.leaderAgent != null) {
					pos = pos + _target.leaderAgent.transform.position;
				}
				Handles.color = Color.red;
				Handles.SphereCap (i, pos, Quaternion.identity, 0.5f);
			}
			break;
		case FormationManager.Formations.Triangle:
			for (int i = 0; i < 3; ++i) {
				_target.agentsPerSide [i] = _target.agentsNum / 3 + (_target.agentsNum % 3 > i ? 1 : 0);
			}
			for (int i = 0; i < _target.agentsNum; i++) {
				var pos = _target.GetTrianglePos (i, _target.zLookAhead);
				if (_target.leaderAgent != null) {
					pos = pos + _target.leaderAgent.transform.position;
				}
				Handles.color = Color.red;
				Handles.SphereCap (i, pos, Quaternion.identity, 0.5f);
			}
			break;
		case FormationManager.Formations.Wedge:
			_target.separation = new Vector2 (_target._separation, _target._separation);
			for (int i = 0; i < _target.agentsNum; i++) {
				var pos = _target.GetWedgePos (i, _target.zLookAhead);
				if (_target.leaderAgent != null) {
					pos = pos + _target.leaderAgent.transform.position;
				}
				Handles.color = Color.red;
				Handles.SphereCap (i, pos, Quaternion.identity, 0.5f);
			}
			break;
		default:
			break;
		}
	}
}


