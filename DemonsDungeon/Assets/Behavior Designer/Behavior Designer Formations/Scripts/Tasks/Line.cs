using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
using Tooltip = BehaviorDesigner.Runtime.Tasks.TooltipAttribute;
using HelpURL = BehaviorDesigner.Runtime.Tasks.HelpURLAttribute;

namespace BehaviorDesigner.Runtime.Formations.Tasks
{
    [TaskCategory("Formations")]
    [TaskDescription("Arrange the group in a straight horizontal line.")]
    [TaskIcon("Assets/Behavior Designer Formations/Editor/Icons/{SkinColor}LineIcon.png")]
    [HelpURL("http://www.opsive.com/assets/BehaviorDesigner/Formations/documentation.php?id=4")]
    public class Line : NavMeshFormationGroup
    {
        [Tooltip("The separation between agents")]
        public SharedFloat separation = 2;
        [Tooltip("Should the formation be to the right of the leader?")]
        public SharedBool right;

        protected override Vector3 TargetPosition(int index, float zLookAhead)
        {
            var leaderTransform = leader.Value == null ? transform : leader.Value.transform;
            return leaderTransform.TransformPoint(separation.Value * index * (right.Value ? 1 : -1), 0, zLookAhead);
        }

        public override void OnReset()
        {
            base.OnReset();

            separation = 2;
            right = false;
        }
    }
}