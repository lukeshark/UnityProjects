using UnityEngine;
using UnityEditor;

namespace BehaviorDesigner.Runtime.Tasks.UFPS
{
    [CustomEditor(typeof(AIWeapon))]
    public class AIWeaponEditor : vp_WeaponEditor
    {
        public override void OnInspectorGUI()
        {
            // intentionally left blank
        }
    }
}