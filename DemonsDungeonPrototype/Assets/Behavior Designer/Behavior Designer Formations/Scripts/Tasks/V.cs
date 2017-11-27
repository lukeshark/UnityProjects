using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
using Tooltip = BehaviorDesigner.Runtime.Tasks.TooltipAttribute;
using HelpURL = BehaviorDesigner.Runtime.Tasks.HelpURLAttribute;

namespace BehaviorDesigner.Runtime.Formations.Tasks
{
    [TaskCategory("Formations")]
    [TaskDescription("Arrange the group in a V shape where the leader is in the back.")]
    [TaskIcon("Assets/Behavior Designer Formations/Editor/Icons/{SkinColor}VIcon.png")]
    [HelpURL("http://www.opsive.com/assets/BehaviorDesigner/Formations/documentation.php?id=7")]
    public class V : NavMeshFormationGroup
    {
        [Tooltip("The separation between agents")]
        public SharedVector2 separation = new Vector2(2, 2);

        protected override Vector3 TargetPosition(int index, float zLookAhead)
        {
            // Alternate between the left and right sides of the v
            var leaderTransform = leader.Value == null ? transform : leader.Value.transform;
            return leaderTransform.TransformPoint(separation.Value.x * (index % 2 == 0 ? -1 : 1) * (((index - 1) / 2) + 1), 0, separation.Value.y * (((index - 1) / 2) + 1) + zLookAhead);
        }

        public override void OnReset()
        {
            base.OnReset();

            separation = new Vector2(2, 2);
        }
    }
}