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
    [TaskDescription("Arrange the group in a tight circle that can move together.")]
    [TaskIcon("Assets/Behavior Designer Formations/Editor/Icons/{SkinColor}SwarmIcon.png")]
    [HelpURL("http://www.opsive.com/assets/BehaviorDesigner/Formations/documentation.php?id=10")]
    public class Swarm : GroupFormation
    {
        [Tooltip("The radius of the group")]
        public SharedFloat radius = 5;
        [Tooltip("The agent positions in the swarm is randomly determined. These positions will keep being regenerated until they are not overlapping any other position. " +
                 "This value allows the random placement to be capped so it doesn't result in an infinite loop")]
        public SharedInt maxPlacementAttempts = 10;

        private List<Vector3> offsets = new List<Vector3>();

        protected override void AddAgentToGroup(Transform agent)
        {
            base.AddAgentToGroup(agent);

            // Prevent the agents from overlapping by generating a new offset until there are no overlaps. The maxAttempts value will prevent an infinite loop
            // from being generated if there are no non-overlap positions because the radius is too small.
            var overlapping = false;
            var offset = Random.insideUnitCircle * radius.Value;
            var maxAttempts = offsets.Count * maxPlacementAttempts.Value;
            var attempts = 0;
            do {
                for (int i = 0; i < offsets.Count; ++i) {
                    if (Vector2.Distance(offset, offsets[i]) < agents[i].radius * 3) {
                        overlapping = true;
                    }
                }
                attempts++;
                
            } while (overlapping && attempts < maxAttempts);

            offsets.Add(new Vector3(offset.x, 0, offset.y));
        }

        protected override int RemoveAgentFromGroup(Transform agent)
        {
            var index = base.RemoveAgentFromGroup(agent);

            offsets.RemoveAt(index);

            return index;
        }

        protected override Vector3 TargetPosition(int index, float zLookAhead)
        {
            return transforms[0].TransformPoint(offsets[index].x, 0, offsets[index].z + zLookAhead);
        }

        public override void OnEnd()
        {
            base.OnEnd();

            offsets.Clear();
        }

        public override void OnReset()
        {
            base.OnReset();

            radius = 5;
            maxPlacementAttempts = 10;
        }
    }
}