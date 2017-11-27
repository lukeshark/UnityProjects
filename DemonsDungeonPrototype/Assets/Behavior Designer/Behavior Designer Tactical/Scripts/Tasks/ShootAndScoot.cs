using UnityEngine;
using System.Collections.Generic;
using BehaviorDesigner.Runtime.Tasks;
using Tooltip = BehaviorDesigner.Runtime.Tasks.TooltipAttribute;
using HelpURL = BehaviorDesigner.Runtime.Tasks.HelpURLAttribute;

namespace BehaviorDesigner.Runtime.Tactical.Tasks
{
    [TaskCategory("Tactical")]
    [TaskDescription("Attacks the target and moves position after a short amount of time")]
    [HelpURL("http://www.opsive.com/assets/BehaviorDesigner/Tactical/documentation.php?id=6")]
    [TaskIcon("Assets/Behavior Designer Tactical/Editor/Icons/{SkinColor}ShootAndScootIcon.png")]
    public class ShootAndScoot : NavMeshTacticalGroup
    {
        [Tooltip("The number of agents that should be in a row")]
        public SharedInt agentsPerRow = 2;
        [Tooltip("The separation between agents")]
        public SharedVector2 separation = new Vector2(2, 2);
        [Tooltip("The amount of time that should elapse before moving to the next attack point")]
        public SharedFloat timeStationary = 2;
        [Tooltip("When moving positions the agents will move based on a new random angle. The mimium move angle specifies the minimum random angle")]
        public SharedFloat minMoveAngle = 10;
        [Tooltip("When moving positions the agents will move based on a new random angle. The maximum move angle specifies the maximum random angle")]
        public SharedFloat maxMoveAngle = 20;
        [Tooltip("When moving positions the agents will move based on a new random radius. The minimum attack radius specifies the minimum radius")]
        public SharedFloat minAttackRadius = 5;
        [Tooltip("When moving positions the agents will move based on a new random radius. The maximum attack radius specifies the maximum radius")]
        public SharedFloat maxAttackRadius = 10;

        private Vector3 offset;
        private float currentAngle;
        private float arrivalTime;
        private bool inPosition;
        private float attackRadius;
        private bool determinePosition;

        public override void OnAwake()
        {
            base.OnAwake();

            Owner.RegisterEvent<float, float>("UpdatePositionParameters", UpdatePositionParameters);
        }

        public override void OnStart()
        {
            base.OnStart();

            arrivalTime = -timeStationary.Value;

            // Determine a starting attack angle.
            if (leader.Value == null) {
                determinePosition = true;
                var attackCenter = CenterAttackPosition();
                var diff = transform.position - attackCenter;
                diff.y = 0;
                // Get an angle in the range of 0 - 360.
                currentAngle = Mathf.Sign(Vector3.Dot(diff, Vector3.right)) * Vector3.Angle(diff, Vector3.forward);
                attackRadius = Random.Range(minAttackRadius.Value, maxAttackRadius.Value);
                for (int i = 1; i < formationTrees.Count; ++i) {
                    formationTrees[i].SendEvent("UpdatePositionParameters", currentAngle, attackRadius);
                }
            }
            inPosition = false;
        }

        protected override void AddAgentToGroup(Behavior agent, int index)
        {
            base.AddAgentToGroup(agent, index);

            if (leader.Value == null) {
                agent.SendEvent("UpdatePositionParameters", currentAngle, attackRadius);
            }
        }

        private void UpdatePositionParameters(float angle, float radius)
        {
            currentAngle = angle;
            attackRadius = radius;
            inPosition = false;
            tacticalAgent.UpdateRotation(true);
        }

        protected override void FormationUpdated(int index)
        {
            base.FormationUpdated(index);

            var row = formationIndex / agentsPerRow.Value;
            var column = formationIndex % agentsPerRow.Value;

            // Each agent will always move to their respective offset when attacking.
            if (column == 0) {
                offset.Set(0, 0, -separation.Value.y * row);
            } else {
                offset.Set(separation.Value.x * (column % 2 == 0 ? -1 : 1) * (((column - 1) / 2) + 1), 0, -separation.Value.y * row);
            }
        }

        public override TaskStatus OnUpdate()
        {
            var baseStatus = base.OnUpdate();
            if (baseStatus != TaskStatus.Running || !started) {
                return baseStatus;
            }

            // Move to the attack position if the agents currently are not in position or if it's time to move to a new position.
            if (!inPosition || (leader.Value == null && arrivalTime + timeStationary.Value < Time.time)) {
                // Only determine a new angle and radius after moving into position. determinePosition will only be true for the leader.
                if (determinePosition) {
                    currentAngle += Random.Range(minMoveAngle.Value, maxMoveAngle.Value) * (Random.value > 0.5f ? 1 : -1);
                    attackRadius = Random.Range(minAttackRadius.Value, maxAttackRadius.Value);
                    determinePosition = false;

                    if (formationTrees != null) {
                        tacticalAgent.UpdateRotation(true);
                        UpdateInPosition(0, false);
                        inPosition = false;
                        for (int i = 1; i < formationTrees.Count; ++i) {
                            formationTrees[i].SendEvent("UpdatePositionParameters", currentAngle, attackRadius);
                        }
                    }
                }

                var attackCenter = CenterAttackPosition();
                var attackRotation = ReverseCenterAttackRotation(attackCenter);
                // Position on the circumference of the circle.
                attackCenter.x += Mathf.Sin(currentAngle * Mathf.Deg2Rad) * attackRadius;
                attackCenter.z += Mathf.Cos(currentAngle * Mathf.Deg2Rad) * attackRadius;

                tacticalAgent.SetDestination(TransformPoint(attackCenter, offset, attackRotation));

                if (tacticalAgent.HasArrived()) {
                    // The agents are not in position until they are looking at the target.
                    FindAttackTarget();
                    if (tacticalAgent.RotateTowardsPosition(tacticalAgent.TargetTransform.position)) {
                        inPosition = true;
                        // Notify the leader when the agent is in position.
                        if (leaderTree != null) {
                            leaderTree.SendEvent("UpdateInPosition", formationIndex, true);
                        } else {
                            UpdateInPosition(0, true);
                        }
                    }
                }

                if (inPosition && leader.Value == null) {
                    // The agnets are in position. Set the arrival time so they will move to a new position after timeStationary.
                    arrivalTime = Time.time;
                    determinePosition = true;
                }
            } else if (canAttack) {
                // The agents are in position and looking at their target. Attack.
                FindAttackTarget();
                tacticalAgent.TryAttack();
            }

            return TaskStatus.Running;
        }

        public override void OnBehaviorComplete()
        {
            base.OnBehaviorComplete();

            Owner.UnregisterEvent<float, float>("UpdatePositionParameters", UpdatePositionParameters);
        }

        public override void OnReset()
        {
            base.OnReset();

            agentsPerRow = 2;
            separation = new Vector2(2, 2);
            timeStationary = 2;
            minMoveAngle = 10;
            maxMoveAngle = 20;
            minAttackRadius = 5;
            maxAttackRadius = 10;
        }
    }
}