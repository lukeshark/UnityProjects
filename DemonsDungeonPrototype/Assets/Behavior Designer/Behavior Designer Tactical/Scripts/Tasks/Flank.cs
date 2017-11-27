using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
using Tooltip = BehaviorDesigner.Runtime.Tasks.TooltipAttribute;
using HelpURL = BehaviorDesigner.Runtime.Tasks.HelpURLAttribute;

namespace BehaviorDesigner.Runtime.Tactical.Tasks
{
    [TaskCategory("Tactical")]
    [TaskDescription("Flanks the target from the left and right")]
    [HelpURL("http://www.opsive.com/assets/BehaviorDesigner/Tactical/documentation.php?id=4")]
    [TaskIcon("Assets/Behavior Designer Tactical/Editor/Icons/{SkinColor}FlankIcon.png")]
    public class Flank : NavMeshTacticalGroup
    {
        [Tooltip("Should the agents flank from both the left and right side?")]
        public SharedBool dualFlank = false;
        [Tooltip("Should the agents wait to attack until all of the agents have moved into position?")]
        public SharedBool waitForAttack = true;
        [Tooltip("Optionally set an extra distance that the agents should first move towards. This will prevent the agents from crossing in front of the enemies")]
        public SharedFloat approachDistance = 2;
        [Tooltip("The distance that the agents should be separated while attacking")]
        public SharedFloat separation = 2;

        private float attackStartTime;
        private Vector3 destinationOffset;
        private bool inPosition;

        protected override void FormationUpdated(int index)
        {
            base.FormationUpdated(index);

            // Determine the initial move to offset. This allows the agents to sneak up on the target without crossing directly in front of the target's field of view.
            var groupCount = dualFlank.Value ? 3 : 2;
            var groupIndex = formationIndex % groupCount;
            if (groupIndex == 0) { // center
                destinationOffset.Set(0, 0, tacticalAgent.AttackAgent.AttackDistance());
            } else if (groupIndex == 1) { // right
                destinationOffset.Set(-tacticalAgent.AttackAgent.AttackDistance() - approachDistance.Value, 0, 0);
            } else { // left
                destinationOffset.Set(tacticalAgent.AttackAgent.AttackDistance() + approachDistance.Value, 0, 0);
            }
            inPosition = false;
        }
        
        public override TaskStatus OnUpdate()
        {
            var baseStatus = base.OnUpdate();
            if (baseStatus != TaskStatus.Running || !started) {
                return baseStatus;
            }

            var attackCenter = CenterAttackPosition();
            var centerRotation = CenterAttackRotation(attackCenter);
            var groupCount = dualFlank.Value ? 3 : 2;

            if (!tacticalAgent.AttackPosition) {
                var groupIndex = formationIndex % groupCount;
                var destination = TransformPoint(attackCenter, destinationOffset, centerRotation);
                // Arrange the agents in a row to prevent two agents from trying to move to the same destination.
                if (formationIndex + 1 > groupCount) {
                    var offset = Vector3.zero;
                    offset.x += separation.Value * ((formationIndex / groupCount) % 2 == 0 ? -1 : 1) * (((groupIndex / 2) + 1));
                    destination = TransformPoint(destination, offset, Quaternion.LookRotation(attackCenter - destination));
                }
                tacticalAgent.SetDestination(destination);
                // Set AttackPosition to true when the agent arrived at the destination. This will put the agent in attack mode and start to rotate towards
                // the target.
                if (tacticalAgent.HasArrived()) {
                    tacticalAgent.AttackPosition = true;
                }
            } else if (MoveToAttackPosition()) {
                // The agent isn't in position yet. One case of MoveToAttackPosition returning false is when the agent still needs to rotate to face the target.
                inPosition = true;
                // Notify the leader when the agent is in position.
                if (leaderTree != null) {
                    leaderTree.SendEvent("UpdateInPosition", formationIndex, true);
                } else {
                    UpdateInPosition(formationIndex, true);
                }
            }

            if (inPosition && (canAttack || !waitForAttack.Value)) {
                tacticalAgent.TryAttack();
            }

            return TaskStatus.Running;
        }

        public override void OnReset()
        {
            base.OnReset();

            dualFlank = false;
            waitForAttack = true;
            approachDistance = 2;
            separation = 2;
        }
    }
}