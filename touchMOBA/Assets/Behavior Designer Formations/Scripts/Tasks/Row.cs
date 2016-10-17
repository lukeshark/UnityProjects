using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
#if !(UNITY_4_3 || UNITY_4_4)
using Tooltip = BehaviorDesigner.Runtime.Tasks.TooltipAttribute;
#if !(UNITY_4_5 || UNITY_4_6 || UNITY_5_0)
using HelpURL = BehaviorDesigner.Runtime.Tasks.HelpURLAttribute;
#endif
#endif

namespace BehaviorDesigner.Runtime.Formations.Tasks
{
    [TaskCategory("Formations")]
    [TaskDescription("Arrange the group in one or more rows with the row significantly wider than the length of the column.")]
    [TaskIcon("Assets/Behavior Designer Formations/Editor/Icons/{SkinColor}RowIcon.png")]
    [HelpURL("http://www.opsive.com/assets/BehaviorDesigner/Formations/documentation.php?id=2")]
    public class Row : GroupFormation
    {
        [Tooltip("The separation between agents")]
        public SharedVector2 separation = new Vector2(2, 2);
        [Tooltip("The number of rows to form")]
        public SharedInt rows = 1;

        protected override Vector3 TargetPosition(int index, float zLookAhead)
        {
            var row = index % rows.Value;
            var column = index / rows.Value;

            Vector3 targetPos;
            if (column == 0) {
                // Position directly behind the leader
                targetPos = transforms[0].TransformPoint(0, 0, -separation.Value.y * row + zLookAhead);
            } else {
                // Alternate between the right and the left side of the center column
                targetPos = transforms[0].TransformPoint(separation.Value.x * (column % 2 == 0 ? -1 : 1) * (((column - 1) / 2) + 1), 0, -separation.Value.y * row + zLookAhead);
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