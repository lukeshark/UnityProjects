using UnityEngine;
using UnityEditor;
using BehaviorDesigner.Editor;
using DG.Tweening;

namespace BehaviorDesigner.Runtime.Tasks.DOTween
{
    [CustomObjectDrawer(typeof(Tweener))]
    public class SharedTweenerDrawer : ObjectDrawer
    {
        public override void OnGUI(GUIContent label)
        {
            EditorGUILayout.LabelField(label);
        }
    }
}