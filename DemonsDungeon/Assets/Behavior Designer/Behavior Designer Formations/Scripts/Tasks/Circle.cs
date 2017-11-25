using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
using Tooltip = BehaviorDesigner.Runtime.Tasks.TooltipAttribute;
using HelpURL = BehaviorDesigner.Runtime.Tasks.HelpURLAttribute;

namespace BehaviorDesigner.Runtime.Formations.Tasks
{
    [TaskCategory("Formations")]
    [TaskDescription("Arrange the group in a circle.")]
    [TaskIcon("Assets/Behavior Designer Formations/Editor/Icons/{SkinColor}CircleIcon.png")]
    [HelpURL("http://www.opsive.com/assets/BehaviorDesigner/Formations/documentation.php?id=14")]
    public class Circle : NavMeshFormationGroup
    {
        [Tooltip("The radius of the circle")]
        public SharedFloat radius = 5;

        private float theta;

        protected override void AddAgentToGroup(Behavior agent, int index)
        {
            base.AddAgentToGroup(agent, index);

            // 2 * PI = 360 degrees
            theta = 2 * Mathf.PI / agents.Count;
        }

        protected override int RemoveAgentFromGroup(Behavior agent)
        {
            var index = base.RemoveAgentFromGroup(agent);

            // 2 * PI = 360 degrees
            theta = 2 * Mathf.PI / agents.Count;

            return index;
        }

        protected override Vector3 TargetPosition(int index, float zLookAhead)
        {
            // Subtract the radius from the z position to prevent the agents from getting ahead of the leader
            var leaderTransform = leader.Value == null ? transform : leader.Value.transform;
            return leaderTransform.TransformPoint(radius.Value * Mathf.Sin(theta * index), 0, radius.Value * Mathf.Cos(theta * index) - radius.Value + zLookAhead);
        }

        public override void OnReset()
        {
            base.OnReset();

            radius = 5;
        }
    }
}