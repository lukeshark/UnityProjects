using UnityEngine;
using System.Collections.Generic;
using BehaviorDesigner.Runtime.Tasks;
using Tooltip = BehaviorDesigner.Runtime.Tasks.TooltipAttribute;
using HelpURL = BehaviorDesigner.Runtime.Tasks.HelpURLAttribute;

namespace BehaviorDesigner.Runtime.Formations.Tasks
{
    [TaskCategory("Formations")]
    [TaskDescription("Arrange the group in a randomly spread out line.")]
    [TaskIcon("Assets/Behavior Designer Formations/Editor/Icons/{SkinColor}SkirmisherIcon.png")]
    [HelpURL("http://www.opsive.com/assets/BehaviorDesigner/Formations/documentation.php?id=9")]
    public class Skirmisher : NavMeshFormationGroup
    {
        [Tooltip("The minimum separation between two agents")]
        public SharedVector2 minSeparation = new Vector2(1, 1);
        [Tooltip("The maximum separation between two agents")]
        public SharedVector2 maxSeparation = new Vector2(3, 3);

        private List<Vector3> offsets = new List<Vector3>();

        protected override void AddAgentToGroup(Behavior agent, int index)
        {
            base.AddAgentToGroup(agent, index);

            // Randomly select a new offset. This offset will be relative to the last transform so no overlap checks need to be done.
            var offset = Vector3.zero;
            if (agents.Count > 1) {
                offset.Set(Random.Range(minSeparation.Value.x, maxSeparation.Value.x), 0, Random.Range(minSeparation.Value.y, maxSeparation.Value.y));
            }
            offsets.Add(offset);
        }

        protected override int RemoveAgentFromGroup(Behavior agent)
        {
            var index = base.RemoveAgentFromGroup(agent);

            if (index != -1) {
                offsets.RemoveAt(index);
            }

            return index;
        }

        protected override Vector3 TargetPosition(int index, float zLookAhead)
        {
            if (offsets.Count <= index) {
                return Vector3.zero;
            }

            var leaderTransform = leader.Value == null ? transform : leader.Value.transform;
            float agentOffset;
            if (index < 3) {
                agentOffset = 0;
            } else {
                // Alternate which side the offset is relative to.
                agentOffset = Mathf.Abs(index / 2 * maxSeparation.Value.x);
            }

            return leaderTransform.TransformPoint(offsets[index].x * (index % 2 == 0 ? -1 : 1) + agentOffset, 0, offsets[index].z + zLookAhead);
        }

        public override void OnReset()
        {
            base.OnReset();

            minSeparation = new Vector2(1, 1);
            maxSeparation = new Vector2(3, 3);
        }
    }
}