using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
using Tooltip = BehaviorDesigner.Runtime.Tasks.TooltipAttribute;
using HelpURL = BehaviorDesigner.Runtime.Tasks.HelpURLAttribute;

namespace BehaviorDesigner.Runtime.Formations.Tasks
{
    [TaskCategory("Formations")]
    [TaskDescription("Arrange the group in an upside down V shape where the leader is in the front.")]
    [TaskIcon("Assets/Behavior Designer Formations/Editor/Icons/{SkinColor}WedgeIcon.png")]
    [HelpURL("http://www.opsive.com/assets/BehaviorDesigner/Formations/documentation.php?id=6")]
    public class Wedge : NavMeshFormationGroup
    {
        [Tooltip("The separation between agents")]
        public SharedVector2 separation = new Vector2(2, 2);
        [Tooltip("Should agents fill the wedge?")]
        public SharedBool fill;

        private int currentRow = 1;
        private int currentAgentsPerRow = 0;
        private int lastIndex;

        protected override Vector3 TargetPosition(int index, float zLookAhead)
        {
            var leaderTransform = leader.Value == null ? transform : leader.Value.transform;
            if (fill.Value) {
                // The wedge can optionally be filled in. I don't know of a nice formula which computes which row the agent should be in relative to its index so use the number of agents
                // already placed to determine the next position. If anybody knows of an easy formula to compute a filled in wedge please send an email to support@opsive.com.
                if (index <= lastIndex) {
                    currentRow = 1;
                    currentAgentsPerRow = 0;
                }
                lastIndex = index;

                var targetPosition = leaderTransform.TransformPoint(Mathf.Lerp(-currentRow * separation.Value.x, currentRow * separation.Value.x, currentAgentsPerRow / (float)currentRow), 0, -separation.Value.y * currentRow + zLookAhead);

                currentAgentsPerRow++;
                if (currentAgentsPerRow > currentRow) {
                    currentAgentsPerRow = 0;
                    currentRow++;
                }

                return targetPosition;
            } else {
                // The wedge is not filled in so the math is much easier.
                return leaderTransform.TransformPoint(separation.Value.x * (index % 2 == 0 ? -1 : 1) * (((index - 1) / 2) + 1), 0, -separation.Value.y * (((index - 1) / 2) + 1) + zLookAhead);
            }
        }

        public override void OnReset()
        {
            base.OnReset();

            separation = new Vector2(2, 2);
            fill = false;
        }
    }
}