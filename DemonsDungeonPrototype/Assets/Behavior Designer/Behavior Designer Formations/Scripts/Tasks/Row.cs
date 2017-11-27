using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
using Tooltip = BehaviorDesigner.Runtime.Tasks.TooltipAttribute;
using HelpURL = BehaviorDesigner.Runtime.Tasks.HelpURLAttribute;

namespace BehaviorDesigner.Runtime.Formations.Tasks
{
    [TaskCategory("Formations")]
    [TaskDescription("Arrange the group in one or more rows with the row significantly wider than the length of the column.")]
    [TaskIcon("Assets/Behavior Designer Formations/Editor/Icons/{SkinColor}RowIcon.png")]
    [HelpURL("http://www.opsive.com/assets/BehaviorDesigner/Formations/documentation.php?id=2")]
    public class Row : NavMeshFormationGroup
    {
        [Tooltip("The separation between agents")]
        public SharedVector2 separation = new Vector2(2, 2);
        [Tooltip("The number of rows to form")]
        public SharedInt rows = 1;

        protected override Vector3 TargetPosition(int index, float zLookAhead)
        {
            var row = index % rows.Value;
            var column = index / rows.Value;

            var leaderTransform = leader.Value == null ? transform : leader.Value.transform;
            Vector3 targetPos;
            if (column == 0) {
                // Position directly behind the leader
                targetPos = leaderTransform.TransformPoint(0, 0, -separation.Value.y * row + zLookAhead);
            } else {
                // Alternate between the right and the left side of the center column
                targetPos = leaderTransform.TransformPoint(separation.Value.x * (column % 2 == 0 ? -1 : 1) * (((column - 1) / 2) + 1), 0, -separation.Value.y * row + zLookAhead);
            }

            return targetPos;
        }

        public override void OnReset()
        {
            base.OnReset();

            separation = new Vector2(2, 2);
            rows = 1;
        }
    }
}