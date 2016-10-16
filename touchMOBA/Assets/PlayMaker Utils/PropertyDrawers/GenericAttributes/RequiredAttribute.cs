using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif 
public class Required : PropertyAttribute
{
	public Required()
	{
	}
}


#if UNITY_EDITOR
[CustomPropertyDrawer(typeof(Required))]
public class RequiredDrawer : PropertyDrawer
{
	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
	{

		if (property.objectReferenceValue == null) {
			GUI.backgroundColor = Color.red;			
		}

		EditorGUI.BeginChangeCheck();
		EditorGUI.PropertyField(position, property, label);
		EditorGUI.EndChangeCheck ();

		GUI.backgroundColor = Color.white;
	}
		
}
#endif