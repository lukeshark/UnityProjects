using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
using Tooltip = BehaviorDesigner.Runtime.Tasks.TooltipAttribute;
using HelpURL = BehaviorDesigner.Runtime.Tasks.HelpURLAttribute;

namespace BehaviorDesigner.Runtime.Tactical.Tasks
{
    [TaskCategory("Tactical")]
    [TaskDescription("Retreats in the opposite direction of the target")]
    [HelpURL("http://www.opsive.com/assets/BehaviorDesigner/Tactical/documentation.php?id=9")]
    [TaskIcon("Assets/Behavior Designer Tactical/Editor/Icons/{SkinColor}RetreatIcon.png")]
    public class Retreat : NavMeshTacticalGroup
    {
        [Tooltip("The distance away from the targets that is considered safe")]
        public SharedFloat safeDistance;

        private Quaternion direction;
        private Vector3 safeOffset;

        protected override void AddAgentToGroup(Behavior agent, int index)
        {
            base.AddAgentToGroup(agent, index);

            if (tacticalAgent != null) {
                // Prevent the agent from updating its rotation so the agent can attack while retreating.
                tacticalAgent.UpdateRotation(false);
            }
        }

        protected override void StartGroup()
        {
            base.StartGroup();

            direction = transform.rotation;
            safeOffset.z = -safeDistance.Value;
        }

        public override TaskStatus OnUpdate()
        {
            var baseStatus = base.OnUpdate();
            if (baseStatus != TaskStatus.Running || !started) {
                return baseStatus;
            }

            var attackCenter = CenterAttackPosition();
            var safe = true;
            // Try to attack the enemy while retreating.
            FindAttackTarget();
            if (tacticalAgent.CanSeeTarget()) {
                if (tacticalAgent.RotateTowardsPosition(tacticalAgent.TargetTransform.position)) {
                    tacticalAgent.TryAttack();
                }
            } else {
                // The agent can update its rotation when the agent is far enough away that it can't attack.
                tacticalAgent.UpdateRotation(true);
            }

            // The agents are only save once they have moved more than the safe distance away from the attack point.
            if ((attackCenter - transform.position).magnitude < safeDistance.Value) {
                safe = false;
                var targetPosition = TransformPoint(transform.position, safeOffset, direction);
                tacticalAgent.SetDestination(targetPosition);
            } else {
                tacticalAgent.Stop();
            }

            return safe ? TaskStatus.Success : TaskStatus.Running;
        }
    }
}