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
    [TaskDescription("Arrange the group in a circle.")]
    [TaskIcon("Assets/Behavior Designer Formations/Editor/Icons/{SkinColor}CircleIcon.png")]
    [HelpURL("http://www.opsive.com/assets/BehaviorDesigner/Formations/documentation.php?id=14")]
    public class Circle : GroupFormation
    {
        [Tooltip("The radius of the circle")]
        public SharedFloat radius = 5;

        private float theta;

        protected override void AddAgentToGroup(Transform agent)
        {
            base.AddAgentToGroup(agent);

            // 2 * PI = 360 degrees
            theta = 2 * Mathf.PI / agents.Count;
        }

        protected override int RemoveAgentFromGroup(Transform agent)
        {
            var index = base.RemoveAgentFromGroup(agent);

            // 2 * PI = 360 degrees
            theta = 2 * Mathf.PI / agents.Count;

            return index;
        }

        protected override Vector3 TargetPosition(int index, float zLookAhead)
        {
            // Subtract the radius from the z position to prevent the agents from getting ahead of the leader
            return transforms[0].TransformPoint(radius.Value * Mathf.Sin(theta * index), 0, radius.Value * Mathf.Cos(theta * index) - radius.Value + zLookAhead);
        }

        public override void OnReset()
        {
            base.OnReset();

            radius = 5;
        }
    }
}