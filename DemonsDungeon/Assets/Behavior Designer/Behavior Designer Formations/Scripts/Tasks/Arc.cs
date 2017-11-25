using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
using Tooltip = BehaviorDesigner.Runtime.Tasks.TooltipAttribute;
using HelpURL = BehaviorDesigner.Runtime.Tasks.HelpURLAttribute;

namespace BehaviorDesigner.Runtime.Formations.Tasks
{
    [TaskCategory("Formations")]
    [TaskDescription("Arrange the group in an arc.")]
    [TaskIcon("Assets/Behavior Designer Formations/Editor/Icons/{SkinColor}ArcIcon.png")]
    [HelpURL("http://www.opsive.com/assets/BehaviorDesigner/Formations/documentation.php?id=8")]
    public class Arc : NavMeshFormationGroup
    {
        [Tooltip("The radius of the arc")]
        public SharedFloat radius = 5;
        [Tooltip("Is the arc concave?")]
        public SharedBool concave = true;

        private float theta;

        protected override void AddAgentToGroup(Behavior agent, int index)
        {
            base.AddAgentToGroup(agent, index);

            // Multiply by 2 to form an arc up to 180 degrees.
            theta = Mathf.PI / (agents.Count * 2);
        }

        protected override int RemoveAgentFromGroup(Behavior agent)
        {
            var index = base.RemoveAgentFromGroup(agent);

            // Multiply by 2 to form an arc up to 180 degrees.
            theta = Mathf.PI / (agents.Count * 2);

            return index;
        }

        protected override Vector3 TargetPosition(int index, float zLookAhead)
        {
            // Alternate between the left and right sides of the leader. If convex then add PI so the agents form the back side of the circle.
            // Convex arcs will also increase their relative z position so the agents are ahead of the leader.
            var radians = theta * (((index - 1) / 2) + 1) + (concave.Value ? 0 : Mathf.PI);
            var leaderTransform = leader.Value == null ? transform : leader.Value.transform;
            return leaderTransform.TransformPoint(radius.Value * Mathf.Sin(radians) * (index % 2 == 0 ? -1 : 1), 0,
                                                    radius.Value * Mathf.Cos(radians) + radius.Value * (concave.Value ? -1 : 1) + zLookAhead);
        }

        public override void OnReset()
        {
            base.OnReset();

            radius = 5;
            concave = true;
        }
    }
}