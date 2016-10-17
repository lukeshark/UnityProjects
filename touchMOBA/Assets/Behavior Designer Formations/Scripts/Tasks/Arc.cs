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
    [TaskDescription("Arrange the group in an arc.")]
    [TaskIcon("Assets/Behavior Designer Formations/Editor/Icons/{SkinColor}ArcIcon.png")]
    [HelpURL("http://www.opsive.com/assets/BehaviorDesigner/Formations/documentation.php?id=8")]
    public class Arc : GroupFormation
    {
        [Tooltip("The radius of the arc")]
        public SharedFloat radius = 5;
        [Tooltip("Is the arc concave?")]
        public SharedBool concave = true;

        private float theta;

        protected override void AddAgentToGroup(Transform agent)
        {
            base.AddAgentToGroup(agent);

            // Multiply by 2 to form an arc up to 180 degrees.
            theta = Mathf.PI / (agents.Count * 2);
        }

        protected override int RemoveAgentFromGroup(Transform agent)
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
            return transforms[0].TransformPoint(radius.Value * Mathf.Sin(radians) * (index % 2 == 0 ? -1 : 1), 0,
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