using UnityEngine;
using UnityEditor;
using AxlPlay;

namespace AxlPlay {
[CustomEditor(typeof(CanHearObject))]
public class CanHearObjectEditor : Editor {

	public override void OnInspectorGUI()
	{
		CanHearObject _target = (CanHearObject)target;
		
		DrawDefaultInspector();
		EditorGUILayout.BeginVertical();
		_target.targetTag = EditorGUILayout.TagField( new GUIContent("Tag of Target:","The tag of the object that we are searching for"), _target.targetTag);
		EditorGUILayout.EndVertical();
		
		
		
	}
}
	
}
