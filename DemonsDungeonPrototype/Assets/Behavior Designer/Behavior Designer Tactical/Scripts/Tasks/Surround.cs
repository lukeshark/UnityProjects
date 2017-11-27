using UnityEngine;
using System.Collections.Generic;
using BehaviorDesigner.Runtime.Tasks;
using Tooltip = BehaviorDesigner.Runtime.Tasks.TooltipAttribute;
using HelpURL = BehaviorDesigner.Runtime.Tasks.HelpURLAttribute;

namespace BehaviorDesigner.Runtime.Tactical.Tasks
{
    [TaskCategory("Tactical")]
    [TaskDescription("Surrounds the enemy and starts to attack after all agents are in position")]
    [HelpURL("http://www.opsive.com/assets/BehaviorDesigner/Tactical/documentation.php?id=8")]
    [TaskIcon("Assets/Behavior Designer Tactical/Editor/Icons/{SkinColor}SurroundIcon.png")]
    public class Surround : NavMeshTacticalGroup
    {
        [Tooltip("The radius of the agents that should surround the target")]
        public SharedFloat radius = 10;

        private float theta;
        private bool inPosition;

        public override void OnStart()
        {
            base.OnStart();

            inPosition = false;
        }

        protected override void AddAgentToGroup(Behavior agent, int index)
        {
            base.AddAgentToGroup(agent, index);

            // 2 * PI = 360 degrees
            theta = 2 * Mathf.PI / agents.Count;
        }

        protected override int RemoveAgentFromGroup(Behavior agent)
        {
            var index = base.RemoveAgentFromGroup(agent);

            // 2 * PI = 360 degrees
            theta = 2 * Mathf.PI / agents.Count;

            return index;
        }

        public override TaskStatus OnUpdate()
        {
            var baseStatus = base.OnUpdate();
            if (baseStatus != TaskStatus.Running || !started) {
                return baseStatus;
            }

            var attackCenter = CenterAttackPosition();
            var attackRotation = CenterAttackRotation(attackCenter);
            var offset = Vector3.zero;
            // Wait until all agents are in position before starting to attack.
            if (canAttack) {
                if (MoveToAttackPosition()) {
                    tacticalAgent.TryAttack();
                }
            } else if (!inPosition) {
                offset.Set(radius.Value * Mathf.Sin(theta * formationIndex), 0, radius.Value * Mathf.Cos(theta * formationIndex));
                var destination = TransformPoint(attackCenter, offset, attackRotation);
                var detour = false;
                // Don't go through the center when travelling to the other side of the circle
                if (offset.z < 0 && InverseTransformPoint(attackCenter, transform.position, attackRotation).z < -tacticalAgent.Radius()) {
                    offset.Set((radius.Value + tacticalAgent.Radius()) * Mathf.Sign(Mathf.Sin(theta * formationIndex)), 0, 0);
                    destination = TransformPoint(attackCenter, offset, attackRotation);
                    detour = true;
                }
                tacticalAgent.SetDestination(destination);
                // The agents can't be in position if they are taking a detour.
                if (!detour && tacticalAgent.HasArrived()) {
                    FindAttackTarget();
                    // The agents are not in position until they are looking at the target.
                    if (tacticalAgent.RotateTowardsPosition(tacticalAgent.TargetTransform.position)) {
                        // Notify the leader when the agent is in position.
                        if (leaderTree != null) {
                            leaderTree.SendEvent("UpdateInPosition", formationIndex, true);
                        } else {
                            UpdateInPosition(0, true);
                        }
                        inPosition = true;
                    }
                }
            }

            return TaskStatus.Running;
        }

        public override void OnReset()
        {
            base.OnReset();

            radius = 10;
        }
    }
}