using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
using Tooltip = BehaviorDesigner.Runtime.Tasks.TooltipAttribute;
using HelpURL = BehaviorDesigner.Runtime.Tasks.HelpURLAttribute;

namespace BehaviorDesigner.Runtime.Formations.Tasks
{
    [TaskCategory("Formations")]
    [TaskDescription("Arrange the group in a diagonal formation.")]
    [TaskIcon("Assets/Behavior Designer Formations/Editor/Icons/{SkinColor}EchelonIcon.png")]
    [HelpURL("http://www.opsive.com/assets/BehaviorDesigner/Formations/documentation.php?id=5")]
    public class Echelon : NavMeshFormationGroup
    {
        [Tooltip("The separation between agents")]
        public SharedVector2 separation = new Vector2(2, 2);
        [Tooltip("Should the formation be to the right of the leader?")]
        public SharedBool right;

        protected override Vector3 TargetPosition(int index, float zLookAhead)
        {
            // Position at a diagonal relative to the leader
            var leaderTransform = leader.Value == null ? transform : leader.Value.transform;
            return leaderTransform.TransformPoint(separation.Value.x * index * (right.Value ? 1 : -1), 0, -separation.Value.y * index + zLookAhead);
        }

        public override void OnReset()
        {
            separation = new Vector2(2, 2);
            right = false;
        }
    }
}