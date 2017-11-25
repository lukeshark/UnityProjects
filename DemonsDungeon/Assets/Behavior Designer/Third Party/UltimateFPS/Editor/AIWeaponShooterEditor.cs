using UnityEngine;
using UnityEditor;

namespace BehaviorDesigner.Runtime.Tasks.UFPS
{
    [CustomEditor(typeof(AIWeaponShooter))]
    public class AIWeaponShooterEditor : vp_ShooterEditor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
        }
    }
}