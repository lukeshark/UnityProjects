using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
using Tooltip = BehaviorDesigner.Runtime.Tasks.TooltipAttribute;
using HelpURL = BehaviorDesigner.Runtime.Tasks.HelpURLAttribute;

namespace BehaviorDesigner.Runtime.Formations.Tasks
{
    [TaskCategory("Formations")]
    [TaskDescription("Arrange the group in a tactical diamond shape.")]
    [TaskIcon("Assets/Behavior Designer Formations/Editor/Icons/{SkinColor}DiamondIcon.png")]
    [HelpURL("http://www.opsive.com/assets/BehaviorDesigner/Formations/documentation.php?id=11")]
    public class Diamond : NavMeshFormationGroup
    {
        [Tooltip("The separation between agents")]
        public SharedVector2 separation = new Vector2(2, 2);
        [Tooltip("Should the back agents have a left and right offset?")]
        public SharedBool backPositionOffset = false;

        protected override Vector3 TargetPosition(int index, float zLookAhead)
        {
            Vector3 targetPos;
            var leaderTransform = leader.Value == null ? transform : leader.Value.transform;
            if (index < 3) { // form the diamond part
                targetPos = leaderTransform.TransformPoint(separation.Value.x * (index % 2 == 0 ? -1 : 1), 0, -separation.Value.y + zLookAhead);
            } else { // form the back of the diamond. This is a tactical diamond so it is made for agents to cover themselves down hallways.
                if (backPositionOffset.Value) {
                    targetPos = leaderTransform.TransformPoint(separation.Value.x * (index % 2 == 0 ? -0.5f : 0.5f), 0, -separation.Value.y * (((index - 1) / 2) + 1) + zLookAhead);
                } else {
                    targetPos = leaderTransform.TransformPoint(0, 0, -separation.Value.y * (index - 1) + zLookAhead);
                }
            }
            return targetPos;
        }

        public override void OnReset()
        {
            base.OnReset();

            separation = new Vector2(2, 2);
            backPositionOffset = false;
        }
    }
}