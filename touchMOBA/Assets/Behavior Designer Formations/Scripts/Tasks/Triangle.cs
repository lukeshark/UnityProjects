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
    [TaskDescription("Arrange the group in a triangle.")]
    [TaskIcon("Assets/Behavior Designer Formations/Editor/Icons/{SkinColor}TriangleIcon.png")]
    [HelpURL("http://www.opsive.com/assets/BehaviorDesigner/Formations/documentation.php?id=12")]
    public class Triangle : GroupFormation
    {
        [Tooltip("The length of the triangle")]
        public SharedFloat length = 5;

        private int[] agentsPerSide = new int[3];

        protected override void AddAgentToGroup(Transform agent)
        {
            base.AddAgentToGroup(agent);

            for (int i = 0; i < 3; ++i) {
                agentsPerSide[i] = agents.Count / 3 + (agents.Count % 3 > i ? 1 : 0);
            }
        }

        protected override int RemoveAgentFromGroup(Transform agent)
        {
            var index = base.RemoveAgentFromGroup(agent);

            for (int i = 0; i < 3; ++i) {
                agentsPerSide[i] = agents.Count / 3 + (agents.Count % 3 > i ? 1 : 0);
            }

            return index;
        }

        protected override Vector3 TargetPosition(int index, float zLookAhead)
        {
            var side = index % 3;
            var lengthMultiplier = (index / 3) / (float)agentsPerSide[side];
            lengthMultiplier = 1 - (lengthMultiplier - (int)lengthMultiplier);
            var height = length.Value / 2 * Mathf.Sqrt(3); // Equilaterial triangle height
            if (side == 0) { // Right
                return transforms[0].TransformPoint(length.Value / 2 * lengthMultiplier, 0, -height * lengthMultiplier + zLookAhead);
            } else if (side == 1) { // Bottom
                return transforms[0].TransformPoint(Mathf.Lerp(-length.Value / 2, length.Value / 2, lengthMultiplier), 0, -height + zLookAhead);
            } else { // Left
                return transforms[0].TransformPoint(-length.Value / 2 * lengthMultiplier, 0, -height * lengthMultiplier + zLookAhead);
            }
        }

        public override void OnReset()
        {
            base.OnReset();

            length = 5;
        }
    }
}