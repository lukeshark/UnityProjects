using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
using Tooltip = BehaviorDesigner.Runtime.Tasks.TooltipAttribute;
using HelpURL = BehaviorDesigner.Runtime.Tasks.HelpURLAttribute;

namespace BehaviorDesigner.Runtime.Formations.Tasks
{
    [TaskCategory("Formations")]
    [TaskDescription("Arrange the group in a square.")]
    [TaskIcon("Assets/Behavior Designer Formations/Editor/Icons/{SkinColor}SquareIcon.png")]
    [HelpURL("http://www.opsive.com/assets/BehaviorDesigner/Formations/documentation.php?id=13")]
    public class Square : NavMeshFormationGroup
    {
        [Tooltip("The length of the square")]
        public SharedFloat length = 5;

        private int[] agentsPerSide = new int[4];

        protected override void AddAgentToGroup(Behavior agent, int index)
        {
            base.AddAgentToGroup(agent, index);

            for (int i = 0; i < 4; ++i) {
                agentsPerSide[i] = agents.Count / 4 + (agents.Count % 4 > i ? 1 : 0);
            }
        }

        protected override int RemoveAgentFromGroup(Behavior agent)
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
            var leaderTransform = leader.Value == null ? transform : leader.Value.transform;
            if (side == 0) { // top
                return leaderTransform.TransformPoint(length.Value * lengthMultiplier, 0, zLookAhead);
            } else if (side == 1) { // right
                return leaderTransform.TransformPoint(length.Value, 0, -length.Value * (1 - lengthMultiplier) + zLookAhead);
            } else if (side == 2) { // bottom
                return leaderTransform.TransformPoint(length.Value * lengthMultiplier, 0, -length.Value + zLookAhead);
            } else { // left
                return leaderTransform.TransformPoint(0, 0, -length.Value * lengthMultiplier + zLookAhead);
            }
        }

        public override void OnReset()
        {
            base.OnReset();

            length = 5;
        }
    }
}