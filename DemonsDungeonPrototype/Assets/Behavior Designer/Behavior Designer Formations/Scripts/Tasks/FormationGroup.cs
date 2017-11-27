using UnityEngine;
#if !(UNITY_5_1 || UNITY_5_2 || UNITY_5_3 || UNITY_5_4)
using UnityEngine.AI;
#endif
using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime.Tasks;
using Tooltip = BehaviorDesigner.Runtime.Tasks.TooltipAttribute;
using HelpURL = BehaviorDesigner.Runtime.Tasks.HelpURLAttribute;
#if DEATHMATCH_AI_KIT_PRESENT
using Opsive.DeathmatchAIKit;
#endif

namespace BehaviorDesigner.Runtime.Formations.Tasks
{
    public abstract class FormationGroup : Action
    {
        [Tooltip("The leader to follow. If null then the current agent will lead")]
        public SharedGameObject leader;
        [Tooltip("Specifies the group index of the leader behavior tree. This is not necessary if there is only one behavior tree on the leader")]
        public SharedInt leaderGroupIndex;
        [Tooltip("The target destination")]
        public SharedTransform targetTransform;
        [Tooltip("The target destination Vector3 position. Used if targetTransform is null")]
        public SharedVector3 targetPosition;
        [Tooltip("The distance to look ahead to the destination. The higher the value the better the agent will avoid obstacles and less keep formation")]
        public SharedFloat lookAhead = 1;
        [Tooltip("The agent move speed after the group has formed")]
        public SharedFloat fullSpeed = 3.5f;
        [Tooltip("The agent move speed as the group is forming")]
        public SharedFloat formationSpeed = 2f;
        [Tooltip("The agent move speed if waiting for another agent to join")]
        public SharedFloat slowdownSpeed = 1;
        [Tooltip("The agent move speed if falling behind the formation")]
        public SharedFloat catchupSpeed = 3f;
        [Tooltip("The amount of time to wait until the group starts forming")]
        public SharedFloat waitTime = 0;
        [Tooltip("Should the agent wait to move when new agents are added?")]
        public SharedBool waitToMove = true;

        public enum MoveStatus { Wait, Full, Formation, Slowdown, Catchup, Last }

        private List<Behavior> formationTrees;
        private List<bool> pathStarted;
        private List<MoveStatus> moveStatus;
        private int formationIndex = -1;
        private bool formationStarted;
        private bool inFormation;
        private Vector3 prevTargetPosition;
        private TaskStatus runStatus;
        private bool sendListenerEvent;
        private GameObject prevLeader;
        private MoveStatus prevMoveStatus = MoveStatus.Last;
        private MoveStatus leaderMoveStatus;
        private Transform prevTargetTransform;

        protected List<Transform> agents;
        protected List<FormationAgent> formationAgents;
        protected Behavior leaderTree;
        protected FormationAgent formationAgent;
        protected FormationAgent leaderAgent;

        /// <summary>
        /// Listen for any agents that want to join the group.
        /// </summary>
        public override void OnAwake()
        {
            Owner.RegisterEvent<Behavior>("StartListeningForOrders", StartListeningForOrders);
            Owner.RegisterEvent<Behavior>("StopListeningToOrders", StopListeningToOrders);
            Owner.RegisterEvent<int>("FormationUpdated", FormationUpdated);
            Owner.RegisterEvent<Behavior, int>("AddAgentToGroup", AddAgentToGroup);
            Owner.RegisterEvent<Vector3>("UpdateTargetPosition", UpdateTargetPosition);
            Owner.RegisterEvent<Transform>("UpdateTarget", UpdateTarget);
            Owner.RegisterEvent<int, MoveStatus>("UpdateMoveStatus", UpdateMoveStatus);
            Owner.RegisterEvent<TaskStatus>("OrdersFinished", OrdersFinished);
        }

        /// <summary>
        /// Start forming the group immediately on start or after a set amount of time.
        /// </summary>
        public override void OnStart()
        {
            UpdateLeader();
            runStatus = TaskStatus.Running;
        }

        /// <summary>
        /// The leader has changed. Update the leader.
        /// </summary>
        private void UpdateLeader()
        {
            // If the leader is null then the current agent is the leader.
            if (leader.Value == null) {
                AddAgentToGroup(Owner, 0);
#if DEATHMATCH_AI_KIT_PRESENT
                if (TeamManager.IsInstantiated) {
                    TeamManager.SetLeader(gameObject, true);
                }
#endif

                if (waitTime.Value == 0) {
                    StartFormation();
                } else {
                    StartCoroutine(WaitForGroupFormation());
                }
            } else {
                var leaderTrees = leader.Value.GetComponents<Behavior>();
                if (leaderTrees.Length > 1) {
                    for (int i = 0; i < leaderTrees.Length; ++i) {
                        if (leaderTrees[i].Group == leaderGroupIndex.Value) {
                            leaderTree = leaderTrees[i];
                            break;
                        }
                    }
                } else if (leaderTrees.Length == 1) {
                    leaderTree = leaderTrees[0];
                }
                if (leaderTree != null) {
                    sendListenerEvent = true;
                }
#if DEATHMATCH_AI_KIT_PRESENT
                if (TeamManager.IsInstantiated) {
                    // If the leader tree is null then the leader is a player-controlled character.
                    formationIndex = TeamManager.AddToFormation(leader.Value, Owner) + (leaderTree == null ? 1 : 0);
                }
#endif
            }
            prevLeader = leader.Value;
        }

        /// <summary>
        /// Wait a small amount of time before the group is formed.
        /// </summary>
        private IEnumerator WaitForGroupFormation()
        {
            yield return new WaitForSeconds(waitTime.Value);

            StartFormation();
        }

        /// <summary>
        /// An agent wants to join the formation.
        /// </summary>
        /// <param name="obj">The agent that wants to join the group.</param>
        private void StartListeningForOrders(Behavior agent)
        {
            // StartListeningForOrders is registered within OnAwake which could cause the callback to be executed when the task isn't active.
            if (runStatus != TaskStatus.Running) {
                return;
            }

            // Add the agent based on the distance to the closest position.
            var distance = float.MaxValue;
            var insertIndex = agents.Count;
            for (int i = 1; i < insertIndex + 1; ++i) {
                var targetPosition = TargetPosition(i, 0);
                var agentDistance = (agent.transform.position - targetPosition).sqrMagnitude;
                // The agent should occupy the slot if it's the closest slot and the slot is closer to the current agent compared to the existing agent occupying the slot.
                if (agentDistance < distance && (i == agents.Count || (agents[i] != null && (agents[i].position - targetPosition).sqrMagnitude > agentDistance))) {
                    insertIndex = i;
                    distance = agentDistance;
                }
            }

            AddAgentToGroup(agent, insertIndex);
        }

        /// <summary>
        /// The formation has changed. Update the formation index.
        /// </summary>
        /// <param name="index">The new formation index.</param>
        private void FormationUpdated(int index)
        {
            formationIndex = index;
        }

        /// <summary>
        /// Adds the agent to the formation group.
        /// </summary>
        /// <param name="agent">The agent to add.</param>
        /// <param name="index">The index of the agent within the group.</param>
        protected virtual void AddAgentToGroup(Behavior agent, int index)
        {
            if (leader.Value == null) {
                if (formationTrees == null) {
                    formationTrees = new List<Behavior>();
                    formationAgents = new List<FormationAgent>();
                    pathStarted = new List<bool>();
                    moveStatus = new List<MoveStatus>();
                }
                // Notify the current agent of the existing agents.
                for (int i = 0; i < formationTrees.Count; ++i) {
                    agent.SendEvent("AddAgentToGroup", formationTrees[i], i);
                }

                // Insert the agent in the lists.
                formationTrees.Insert(index, agent);
                pathStarted.Insert(index, false);
                moveStatus.Insert(index, index == 0 ? MoveStatus.Wait : MoveStatus.Full);

                // Notify the agent of the target.
                if (targetTransform.Value != null) {
                    prevTargetTransform = targetTransform.Value;
                    formationTrees[index].SendEvent("UpdateTarget", targetTransform.Value);
                } else {
                    prevTargetPosition = targetPosition.Value;
                    formationTrees[index].SendEvent("UpdateTargetPosition", targetPosition.Value);
                }

                // Notify other agents that the current agent has joined the formation.
                for (int i = 1; i < formationTrees.Count; ++i) {
                    formationTrees[i].SendEvent("FormationUpdated", i);
                    formationTrees[i].SendEvent("AddAgentToGroup", formationTrees[index], index);
                }
                formationIndex = index;
            } else {
                sendListenerEvent = false;
                formationAgent.Resume();
            }

            // The agents array is maintained on both the leader and follower.
            if (agents == null) {
                agents = new List<Transform>();
            }
            agents.Insert(index, agent.transform);
            if (waitToMove.Value) {
                inFormation = false;
                leaderMoveStatus = MoveStatus.Wait;
                prevMoveStatus = MoveStatus.Last;
            }
        }

        /// <summary>
        /// Updates the target position on the following agent.
        /// </summary>
        /// <param name="target">The new target position.</param>
        private void UpdateTargetPosition(Vector3 target)
        {
            // UpdateTargetPosition is registered within OnAwake which could cause the callback to be executed when the task isn't active.
            if (runStatus != TaskStatus.Running) {
                return;
            }

            targetPosition.Value = target;
        }

        /// <summary>
        /// Updates the target transform on the following agent.
        /// </summary>
        /// <param name="target">The new target transform.</param>
        private void UpdateTarget(Transform target)
        {
            // UpdateTarget is registered within OnAwake which could cause the callback to be executed when the task isn't active.
            if (runStatus != TaskStatus.Running) {
                return;
            }

            targetTransform.Value = target;
        }

        /// <summary>
        /// Update the leader or follower move status.
        /// </summary>
        /// <param name="index">The index of the agent to update.</param>
        /// <param name="status">The move status of the index </param>
        private void UpdateMoveStatus(int index, MoveStatus status)
        {
            // UpdateMoveStatus is registered within OnAwake which could cause the callback to be executed when the task isn't active.
            if (runStatus != TaskStatus.Running) {
                return;
            }

            if (leader.Value == null) {
                moveStatus[index] = status;
            } else {
                leaderMoveStatus = status;
            }
        }

        /// <summary>
        /// Start forming the group.
        /// </summary>
        private void StartFormation()
        {
            formationStarted = true;
        }

        /// <summary>
        /// Move the agents in a formation. The TargetPosition method will retrieve the target position for the individual group member.
        /// </summary>
        public override TaskStatus OnUpdate()
        {
            // If the leader has changed then reinitialize with the new leader.
            if (prevLeader != leader.Value) {
                EndFormation();
                UpdateLeader();
            }

            if (leader.Value == null) {
                if (formationStarted) {
                    // Notify following agents if the target position has updated.
                    if (targetTransform.Value != null) {
                        if (targetTransform.Value != prevTargetTransform) {
                            prevTargetTransform = targetTransform.Value;
                            for (int i = 1; i < formationTrees.Count; ++i) {
                                formationTrees[i].SendEvent("UpdateTarget", targetTransform.Value);
                            }
                        }
                    } else if (targetPosition.Value != prevTargetPosition) {
                        prevTargetPosition = targetPosition.Value;
                        for (int i = 1; i < formationTrees.Count; ++i) {
                            formationTrees[i].SendEvent("UpdateTargetPosition", targetPosition.Value);
                        }
                    }

                    // Wait until all of the agents are in position before moving to the target.
                    var waitForAgent = false;
                    for (int i = 1; i < formationAgents.Count; ++i) {
                        if (!pathStarted[i]) {
                            pathStarted[i] = formationAgents[i].HasPath;
                        }
                        if (!pathStarted[i] || (moveStatus[0] == MoveStatus.Wait && moveStatus[i] == MoveStatus.Full) || moveStatus[i] == MoveStatus.Catchup) {
                            waitForAgent = true;
                        }
                    }

                    // Send the updated move status to all of the followers.
                    if (waitForAgent) {
                        moveStatus[0] = inFormation ? MoveStatus.Formation : MoveStatus.Wait;
                    } else {
                        moveStatus[0] = MoveStatus.Full;
                    }
                    if (moveStatus[0] != prevMoveStatus) {
                        for (int i = 0; i < formationAgents.Count; ++i) {
                            formationTrees[i].SendEvent("UpdateMoveStatus", 0, moveStatus[0]);
                        }

                        prevMoveStatus = moveStatus[0];
                    }

                    var target = (targetTransform.Value != null ? targetTransform.Value.position : targetPosition.Value);
                    formationAgent.SetDestination(target);

                    // Determine if all of the agents have arrived.
                    var arrived = true;
                    for (int i = 0; i < formationAgents.Count; ++i) {
                        if (formationAgents[i].RemainingDistance > formationAgent.StoppingDistance || formationAgents[i].PathPending) {
                            arrived = false;
                            break;
                        } else {
                            formationAgents[i].Stop();
                        }
                    }
                    if (arrived) {
                        runStatus = TaskStatus.Success;
                        return runStatus;
                    }

                    // The leader can move if all agents are ready.
                    if (!waitForAgent) {
                        inFormation = true;
                    }
                    formationAgent.Speed = (!waitForAgent ? fullSpeed.Value : (inFormation ? formationSpeed.Value : 0));
                }
            } else {
                // Send within OnUpdate to ensure the at least one leader behavior tree is active. If registered within OnStart there is a chance that the behavior tree
                // isn't active yet and will never receive the event.
                if (sendListenerEvent) {
                    leaderTree.SendEvent("StartListeningForOrders", Owner);
                    return runStatus;
                }

                // A following agent should never have a formation index of -1. If the index is 0 then the agent hasn't been registered with the leader yet.
                if (formationIndex == -1) {
                    return runStatus;
                }

                // Move towards the starting position and look in the same direction as the leader when just getting started in the formation.
                var targetDistance = (transform.position - TargetPosition(formationIndex, 0)).magnitude;
                if (!inFormation) {
                    if (targetDistance <= formationAgent.StoppingDistance + 0.001f) {
                        inFormation = formationAgent.RotateTowards(leader.Value.transform.rotation) || leaderMoveStatus != MoveStatus.Wait;
                    }
                }

                // If the destination is immediately in front of the agent then that agents stopping distance will take over and slow down the agent. This will make the agent
                // lag behind the leader. Prevent this from happening by adding a small look ahead distance.
                var leaderTarget = targetTransform.Value != null ? targetTransform.Value.position : targetPosition.Value;
                var leaderDistance = (leader.Value.transform.position - leaderTarget).magnitude;
                var zLookAhead = inFormation ? Mathf.Min(leaderDistance, lookAhead.Value) : 0;
                var target = TargetPosition(formationIndex, zLookAhead);

                // TargetPosition will be overridden to return the target position for the individual agent.
                formationAgent.SetDestination(target);

#if UNITY_EDITOR
                Debug.DrawRay(TargetPosition(formationIndex, 0), Vector3.up);
#endif
                // Determine the current move status.
                MoveStatus currentMoveStatus;
                if (inFormation) {
                    if (formationAgent.RemainingDistance - zLookAhead < -formationAgent.Radius / 2) {
                        currentMoveStatus = leaderAgent != null && leaderMoveStatus == MoveStatus.Wait ? MoveStatus.Wait : MoveStatus.Slowdown;
                    } else {
                        if (targetDistance < (formationAgent.Radius * 2 + formationAgent.StoppingDistance) && leaderAgent != null) {
                            currentMoveStatus = leaderMoveStatus;
                        } else {
                            currentMoveStatus = MoveStatus.Catchup;
                        }
                    }
                } else {
                    currentMoveStatus = (leaderMoveStatus != MoveStatus.Wait ? MoveStatus.Catchup : MoveStatus.Full);
                }
                // Set the speed according to the move status, and notify the leader.
                if (currentMoveStatus != prevMoveStatus) {
                    switch (currentMoveStatus) {
                        case MoveStatus.Wait:
                            formationAgent.Speed = 0;
                            break;
                        case MoveStatus.Slowdown:
                            formationAgent.Speed = slowdownSpeed.Value;
                            break;
                        case MoveStatus.Formation:
                            formationAgent.Speed = formationSpeed.Value;
                            break;
                        case MoveStatus.Full:
                            formationAgent.Speed = fullSpeed.Value;
                            break;
                        case MoveStatus.Catchup:
                            formationAgent.Speed = catchupSpeed.Value;
                            break;
                    }
                    // Prevent auto breaking from slowing the agent down if they are trying to catch up.
                    formationAgent.AutoBreaking = (currentMoveStatus != MoveStatus.Catchup);

                    if (leaderTree != null) {
                        leaderTree.SendEvent("UpdateMoveStatus", formationIndex, currentMoveStatus);
                    }
                    prevMoveStatus = currentMoveStatus;
                }
            }
            return runStatus;
        }

        /// <summary>
        /// The task has completed its orders.
        /// </summary>
        /// <param name="status">The return status of the task.</param>
        private void OrdersFinished(TaskStatus status)
        {
            runStatus = status;
        }

        /// <summary>
        /// An agent has dropped out of the group so it should be removed.
        /// </summary>
        /// <param name="obj">The agent to remove.</param>
        private void StopListeningToOrders(Behavior agent)
        {
            // StopListeningToOrders is registered within OnAwake which could cause the callback to be executed when the task isn't active.
            if (runStatus != TaskStatus.Running) {
                return;
            }

            RemoveAgentFromGroup(agent);
        }

        /// <summary>
        /// Removes the agent from the group.
        /// </summary>
        /// <param name="agent">The agent to remove.</param>
        /// <returns>The index of the agent removed from the group.</returns>
        protected virtual int RemoveAgentFromGroup(Behavior agent)
        {
            var agentTransform = agent.transform;
            for (int i = agents.Count - 1; i >= 0; --i) {
                if (agents[i] == agentTransform) {
                    if (prevLeader == null) {
                        formationTrees.RemoveAt(i);
                        formationAgents.RemoveAt(i);
                        pathStarted.RemoveAt(i);
                        moveStatus.RemoveAt(i);
                        for (int j = 1; j < formationTrees.Count; ++j) {
                            formationTrees[j].SendEvent("StopListeningToOrders", agent);
                            formationTrees[j].SendEvent("FormationUpdated", j);
                        }
                    }
                    agents.RemoveAt(i);
                    if (waitToMove.Value) {
                        inFormation = false;
                        prevMoveStatus = MoveStatus.Last;
                    }
                    return i;
                }
            }
            return -1;
        }
        
        /// <summary>
        /// Virtual method to allow the formation tasks to specify a target position.
        /// </summary>
        /// <param name="index">The index of the group member.</param>
        /// <param name="zLookAhead">The z distance to look ahead of the target position.</param>
        /// <returns>The position to move to, in world space.</returns>
        protected virtual Vector3 TargetPosition(int index, float zLookAhead)
        {
            return Vector3.zero;
        }

        /// <summary>
        /// The task has ended.
        /// </summary>
        public override void OnEnd()
        {
            EndFormation();
        }

        /// <summary>
        /// Ends the current formation.
        /// </summary>
        private void EndFormation()
        {
            if (formationTrees != null) {
                // If the status is running then the leader task ended early. Send a status of failure to the group.
                if (runStatus == TaskStatus.Running) {
                    runStatus = TaskStatus.Failure;
                }
                for (int i = 0; i < formationTrees.Count; ++i) {
                    formationTrees[i].SendEvent("OrdersFinished", runStatus);
                }
                formationAgents.Clear();
                formationTrees.Clear();
                pathStarted.Clear();
                moveStatus.Clear();
                prevMoveStatus = MoveStatus.Full;
#if DEATHMATCH_AI_KIT_PRESENT
                if (TeamManager.IsInstantiated) {
                    TeamManager.SetLeader(gameObject, false);
                }
#endif
            } else {
                if (leaderTree != null) {
                    leaderTree.SendEvent("StopListeningToOrders", Owner);
                }
#if DEATHMATCH_AI_KIT_PRESENT
                if (TeamManager.IsInstantiated) {
                    TeamManager.RemoveFromFormation(prevLeader, Owner);
                }
#endif
            }
            formationIndex = -1;
            formationAgent.Stop();
            inFormation = false;
            if (agents != null) {
                agents.Clear();
            }
        }

        /// <summary>
        /// The behavior tree is complete so the task should stop listening for the events.
        /// </summary>
        public override void OnBehaviorComplete()
        {
            Owner.UnregisterEvent<Behavior>("StartListeningForOrders", StartListeningForOrders);
            Owner.UnregisterEvent<Behavior>("StopListeningToOrders", StopListeningToOrders);
            Owner.UnregisterEvent<int>("FormationUpdated", FormationUpdated);
            Owner.UnregisterEvent<Behavior, int>("AddAgentToGroup", AddAgentToGroup);
            Owner.UnregisterEvent<Vector3>("UpdateTargetPosition", UpdateTargetPosition);
            Owner.UnregisterEvent<Transform>("UpdateTarget", UpdateTarget);
            Owner.UnregisterEvent<int, MoveStatus>("UpdateMoveStatus", UpdateMoveStatus);
            Owner.UnregisterEvent<TaskStatus>("OrdersFinished", OrdersFinished);
        }

        /// <summary>
        /// Reset the public variables back to their defaults.
        /// </summary>
        public override void OnReset()
        {
            targetTransform = null;
            targetPosition = Vector3.zero;
            lookAhead = 1;
            fullSpeed = 3.5f;
            formationSpeed = 2f;
            slowdownSpeed = 1f;
            catchupSpeed = 3f;
            waitTime = 0;
            waitToMove = true;
        }
    }
}