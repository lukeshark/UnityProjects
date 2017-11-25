using UnityEngine;
using System.Collections.Generic;
using BehaviorDesigner.Runtime.Tasks;
using Tooltip = BehaviorDesigner.Runtime.Tasks.TooltipAttribute;
using HelpURL = BehaviorDesigner.Runtime.Tasks.HelpURLAttribute;

namespace BehaviorDesigner.Runtime.Tactical.Tasks
{
    [TaskCategory("Tactical")]
    [TaskDescription("Search for the target by forming two groups and leapfrogging each other. Both groups will start attacking as soon as the target is within sight")]
    [HelpURL("http://www.opsive.com/assets/BehaviorDesigner/Tactical/documentation.php?id=7")]
    [TaskIcon("Assets/Behavior Designer Tactical/Editor/Icons/{SkinColor}LeapfrogIcon.png")]
    public class Leapfrog : NavMeshTacticalGroup
    {
        [Tooltip("The horizontal separation between agents within the group")]
        public SharedFloat separation = 2;
        [Tooltip("The horizontal separation between the two groups")]
        public SharedFloat groupSeparation = 10;
        [Tooltip("The distance of one leap")]
        public SharedFloat leapDistance = 10;

        private Vector3 destinationOffset;
        private bool inPosition;
        private bool canMove;
        private bool firstMove;
        private bool moveFirstGroup;
        private List<bool> agentReady = new List<bool>();

        public override void OnAwake()
        {
            base.OnAwake();

            Owner.RegisterEvent<bool>("UpdateCanMove", UpdateCanMove);
            Owner.RegisterEvent<int, bool>("UpdateReadyState", UpdateReadyState);
        }

        public override void OnStart()
        {
            base.OnStart();

            moveFirstGroup = false;
            inPosition = false;
            firstMove = true;
        }

        protected override void FormationUpdated(int index)
        {
            base.FormationUpdated(index);

            var groupIndex = formationIndex % 2;
            destinationOffset.x = separation.Value * (groupIndex == 0 ? -1 : 1) * (int)(formationIndex / 2);
            if (groupIndex == 1) {
                destinationOffset.x += groupSeparation.Value; 
            }

            canMove = true;
        }

        protected override void AddAgentToGroup(Behavior agent, int index)
        {
            base.AddAgentToGroup(agent, index);

            if (leader.Value == null) {
                agentReady.Add(false);
            }
        }

        private void UpdateCanMove(bool move)
        {
            canMove = true;
        }

        private void UpdateReadyState(int index, bool ready)
        {
            agentReady[index] = ready;

            var allReady = true;
            for (int i = 0; i < agentReady.Count; ++i) {
                if (!agentReady[i]) {
                    allReady = false;
                    break;
                }
            }

            // If all of the agents are ready then send the can move event to the agnets that should move.
            if (allReady) {
                moveFirstGroup = !moveFirstGroup;
                for (int i = 0; i < formationTrees.Count; ++i) {
                    if ((moveFirstGroup && i % 2 == 0) || (!moveFirstGroup && i % 2 == 1)) {
                        continue;
                    }

                    formationTrees[i].SendEvent("UpdateCanMove", true);
                    agentReady[i] = false;
                }
            }
        }
        
        public override TaskStatus OnUpdate()
        {
            var baseStatus = base.OnUpdate();
            if (baseStatus != TaskStatus.Running || !started) {
                return baseStatus;
            }

            // Start attacking if a target is found.
            if (canAttack) {
                // An target has been detected. All units should attack.
                if (MoveToAttackPosition()) {
                    tacticalAgent.TryAttack();
                }
                return TaskStatus.Running;
            }

            // An target has been detected. All units should attack.
            FindAttackTarget();
            if (tacticalAgent.CanSeeTarget()) {
                if (leaderTree != null) {
                    leaderTree.SendEvent("SendAttackEvent", true);
                } else {
                    SendAttackEvent(true);
                }
                return TaskStatus.Running;
            }

            // No attacking unit has been found. Move into position and start leapfrogging.
            if (canMove) {
                // Move the agents into their starting position if they haven't been there already.
                if (!inPosition) {
                    var leaderTransform = leader.Value != null ? leader.Value.transform : transform;
                    tacticalAgent.SetDestination(TransformPoint(leaderTransform.position, destinationOffset, leaderTransform.rotation));
                    if (tacticalAgent.HasArrived()) {
                        // The agent is in position but it may not be facing the target.
                        if (tacticalAgent.RotateTowards(leaderTransform.rotation)) {
                            inPosition = true;
                            canMove = false;
                            if (leaderTree != null) {
                                leaderTree.SendEvent("UpdateReadyState", formationIndex, true);
                            } else {
                                UpdateReadyState(0, true);
                            }
                        }
                    }
                } else {
                    var leaderTransform = leader.Value != null ? leader.Value.transform : transform;
                    var moveOffset = Vector3.zero;
                    moveOffset.z = (leapDistance.Value * (formationIndex % 2 == 0 && !firstMove ? 2 : 1));
                    tacticalAgent.SetDestination(TransformPoint(leaderTransform.position, destinationOffset + moveOffset, leaderTransform.rotation));
                    canMove = false;
                    firstMove = false;
                }
            } else {
                // Notify the leader when the agent has arrived.
                if (tacticalAgent.HasArrived()) {
                    if (leaderTree != null) {
                        leaderTree.SendEvent("UpdateReadyState", formationIndex, true);
                    } else {
                        UpdateReadyState(0, true);
                    }
                }
            }
            return TaskStatus.Running;
        }

        protected override int RemoveAgentFromGroup(Behavior agent)
        {
            var index = base.RemoveAgentFromGroup(agent);
            if (leader.Value == null && index != -1) {
                agentReady.RemoveAt(index);
            }
            return index;
        }

        public override void OnBehaviorComplete()
        {
            base.OnBehaviorComplete();

            Owner.UnregisterEvent<bool>("UpdateCanMove", UpdateCanMove);
        }

        public override void OnReset()
        {
            base.OnReset();

            separation = 2;
            groupSeparation = 10;
            leapDistance = 10;
        }
    }
}