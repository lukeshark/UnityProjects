using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor (typeof(FormationManager))]
public class FormationManagerEditor : Editor
{


	public override void OnInspectorGUI ()
	{
		FormationManager _target = (FormationManager)target;

		EditorGUILayout.BeginVertical ("box");
		_target.target = (GameObject)EditorGUILayout.ObjectField ("Target", _target.target, typeof(GameObject), true);
		EditorGUILayout.EndVertical ();



		EditorGUILayout.BeginVertical ("box");

		// EditorGUILayout.FloatField("Agents In Formation", _target.agentsNum);
		_target.zLookAhead = EditorGUILayout.FloatField ("Look Ahead", _target.zLookAhead);
		_target.formationSpeed = EditorGUILayout.FloatField ("Formation Speed", _target.formationSpeed);
        
		EditorGUILayout.EndVertical ();



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
		case FormationManager.Formations.Column:
			_target._separation = EditorGUILayout.FloatField (new GUIContent ("Separation", "The separation between agents in the formation."), _target._separation);
			_target.columns = EditorGUILayout.IntSlider (new GUIContent ("Columns", "The number of columns to form"), _target.columns, 1, 100);
			break;

		case FormationManager.Formations.Diamond:
			_target.backPositionOffset = EditorGUILayout.Toggle (new GUIContent ("backPositionOffset", "Should the back agents have a left and right offset?"), _target.backPositionOffset);
			_target._separation = EditorGUILayout.FloatField (new GUIContent ("Separation", "The separation between agents in the formation."), _target._separation);

			break;
		case FormationManager.Formations.Diagonal:
			_target.right = EditorGUILayout.Toggle (new GUIContent ("Right", "Should the formation be to the right of the leader?"), _target.right);
			_target._separation = EditorGUILayout.FloatField (new GUIContent ("Separation", "The separation between agents in the formation."), _target._separation);

			break;
		case FormationManager.Formations.Grid:
			_target.columns = EditorGUILayout.IntSlider (new GUIContent ("Columns", "The number of columns to form"), _target.columns, 1, 100);
			_target._separation = EditorGUILayout.FloatField (new GUIContent ("Separation", "The separation between agents in the formation."), _target._separation);

			break;

		case FormationManager.Formations.Line:
			_target._separation = EditorGUILayout.FloatField (new GUIContent ("Separation", "The separation between agents in the formation."), _target._separation);
			_target.right = EditorGUILayout.Toggle (new GUIContent ("Right", "Should the formation be to the right of the leader?"), _target.right);

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
			if (_target.leaderAgent == null && _target.agentsNum > 1) {

				_target.leaderAgent = _target.agentes [_target.agentsNum - 1];

			}
		}


		if (GUILayout.Button ("Load Agents from tag")) {
			_target.loadFromTags ();
			_target.LoadAgentList ();
			Debug.Log (_target.agentsNum);
			GUI.color = Color.white;
		}
      
		//if (GUILayout.Button("Test"))
		//{
          
        
		//}

		EditorGUILayout.EndHorizontal ();
		EditorGUILayout.EndVertical ();
		EditorGUILayout.EndHorizontal ();

		EditorUtility.SetDirty (target);
	}

	void OnSceneGUI ()
	{


		FormationManager _target = (FormationManager)target;

		if (_target.leaderAgent != null) {
			Handles.color = Color.red;
			Handles.ArrowCap (0, _target.leaderAgent.transform.position, _target.leaderAgent.transform.rotation, 4f);
		}


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

		case FormationManager.Formations.Column:
			_target.separation = new Vector2 (_target._separation, _target._separation);
			for (int i = 0; i < _target.agentsNum; i++) {
				var pos = _target.GetColumnPos (i, _target.zLookAhead);
				if (_target.leaderAgent != null) {
					pos = pos + _target.leaderAgent.transform.position;
				}
				Handles.color = Color.red;
				Handles.SphereCap (i, pos, Quaternion.identity, 0.5f);
			}

			break;


		case FormationManager.Formations.Diamond:

			_target.separation = new Vector2 (_target._separation, _target._separation);
			for (int i = 0; i < _target.agentsNum; i++) {
				var pos = _target.GetDiamondPos (i, _target.zLookAhead);
				if (_target.leaderAgent != null) {
					pos = pos + _target.leaderAgent.transform.position;
				}
				Handles.color = Color.red;
				Handles.SphereCap (i, pos, Quaternion.identity, 0.5f);
			}
			break;

		case FormationManager.Formations.Diagonal:

			_target.separation = new Vector2 (_target._separation, _target._separation);
			for (int i = 0; i < _target.agentsNum; i++) {
				var pos = _target.GetDiagonalPos (i, _target.zLookAhead);
				if (_target.leaderAgent != null) {
					pos = pos + _target.leaderAgent.transform.position;
				}
				Handles.color = Color.red;
				Handles.SphereCap (i, pos, Quaternion.identity, 0.5f);
			}
			break;

		case FormationManager.Formations.Grid:

			_target.separation = new Vector2 (_target._separation, _target._separation);
			for (int i = 0; i < _target.agentsNum; i++) {
				var pos = _target.GetGridPos (i, _target.zLookAhead);
				if (_target.leaderAgent != null) {
					pos = pos + _target.leaderAgent.transform.position;
				}
				Handles.color = Color.red;
				Handles.SphereCap (i, pos, Quaternion.identity, 0.5f);
			}
			break;

		case FormationManager.Formations.Line:

			_target.separation = new Vector2 (_target._separation, _target._separation);
			for (int i = 0; i < _target.agentsNum; i++) {
				var pos = _target.GetLinePos (i, _target.zLookAhead);
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


