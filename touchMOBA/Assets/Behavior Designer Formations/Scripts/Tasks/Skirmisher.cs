using UnityEngine;
using System.Collections.Generic;
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
    [TaskDescription("Arrange the group in a randomly spread out line.")]
    [TaskIcon("Assets/Behavior Designer Formations/Editor/Icons/{SkinColor}SkirmisherIcon.png")]
    [HelpURL("http://www.opsive.com/assets/BehaviorDesigner/Formations/documentation.php?id=9")]
    public class Skirmisher : GroupFormation
    {
        [Tooltip("The minimum separation between two agents")]
        public SharedVector2 minSeparation = new Vector2(1, 1);
        [Tooltip("The maximum separation between two agents")]
        public SharedVector2 maxSeparation = new Vector2(3, 3);

        private List<Vector3> offsets = new List<Vector3>();

        protected override void AddAgentToGroup(Transform agent)
        {
            base.AddAgentToGroup(agent);

            // Randomly select a new offset. This offset will be relative to the last transform so no overlap checks need to be done.
            var offset = Vector3.zero;
            if (agents.Count > 1) {
                offset.Set(Random.Range(minSeparation.Value.x, maxSeparation.Value.x), 0, Random.Range(minSeparation.Value.y, maxSeparation.Value.y));
            }
            offsets.Add(offset);
        }

        protected override int RemoveAgentFromGroup(Transform agent)
        {
            var index = base.RemoveAgentFromGroup(agent);

            offsets.RemoveAt(index);

            return index;
        }

        protected override Vector3 TargetPosition(int index, float zLookAhead)
        {
            // Alternate which side the offset is relative to.
            Transform relativeTransform;
            if (index < 3) {
                relativeTransform = transforms[0];
            } else {
                relativeTransform = transforms[index - 2];
            }

            return relativeTransform.TransformPoint(offsets[index].x * (index % 2 == 0 ? -1 : 1), 0, offsets[index].z + zLookAhead);
        }

        public override void OnReset()
        {
            base.OnReset();

            minSeparation = new Vector2(1, 1);
            maxSeparation = new Vector2(3, 3);
        }
    }
}