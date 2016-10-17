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
    [TaskDescription("Arrange the group in a square.")]
    [TaskIcon("Assets/Behavior Designer Formations/Editor/Icons/{SkinColor}SquareIcon.png")]
    [HelpURL("http://www.opsive.com/assets/BehaviorDesigner/Formations/documentation.php?id=13")]
    public class Square : GroupFormation
    {
        [Tooltip("The length of the square")]
        public SharedFloat length = 5;

        private int[] agentsPerSide = new int[4];

        protected override void AddAgentToGroup(Transform agent)
        {
            base.AddAgentToGroup(agent);

            for (int i = 0; i < 4; ++i) {
                agentsPerSide[i] = agents.Count / 4 + (agents.Count % 4 > i ? 1 : 0);
            }
        }

        protected override int RemoveAgentFromGroup(Transform agent)
        {
            var index = base.RemoveAgentFromGroup(agent);

            for (int i = 0; i < 4; ++i) {
                agentsPerSide[i] = agents.Count / 4 + (agents.Count % 4 > i ? 1 : 0);
            }

            return index;
        }

        protected override Vector3 TargetPosition(int index, float zLookAhead)
        {
            var side = index % 4;
            var lengthMultiplier = (index / 4) / (float)agentsPerSide[side];
            lengthMultiplier = 1 - (lengthMultiplier - (int)lengthMultiplier);
            if (side == 0) { // top
                return transforms[0].TransformPoint(length.Value * lengthMultiplier, 0, zLookAhead);
            } else if (side == 1) { // right
                return transforms[0].TransformPoint(length.Value, 0, -length.Value * (1 - lengthMultiplier) + zLookAhead);
            } else if (side == 2) { // bottom
                return transforms[0].TransformPoint(length.Value * lengthMultiplier, 0, -length.Value + zLookAhead);
            } else { // left
                return transforms[0].TransformPoint(0, 0, -length.Value * lengthMultiplier + zLookAhead);
            }
        }

        public override void OnReset()
        {
            base.OnReset();

            length = 5;
        }
    }
}