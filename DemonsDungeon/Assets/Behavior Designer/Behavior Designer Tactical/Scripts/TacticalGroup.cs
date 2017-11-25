using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime.Tasks;
using Tooltip = BehaviorDesigner.Runtime.Tasks.TooltipAttribute;
using HelpURL = BehaviorDesigner.Runtime.Tasks.HelpURLAttribute;

namespace BehaviorDesigner.Runtime.Tactical.Tasks
{
    /// <summary>
    /// Base class for all Tactical tasks. This class knows about all of the agents that can attack and the attack targets.
    /// </summary>
    public abstract class TacticalGroup : Action
    {
        [Tooltip("The leader to follow. If null then the current agent will lead")]
        public SharedGameObject leader;
        [Tooltip("Specifies the group index of the leader behavior tree. This is not necessary if there is only one behavior tree on the leader")]
        public SharedInt leaderGroupIndex;
        [Tooltip("The objects to attack. If blank the targetTag will be used")]
        public SharedGameObjectList targetGroup;
        [Tooltip("The tag of the objects to attack. Will be used if targetGroup has no elements")]
        public SharedString targetTag;
        [Tooltip("The amount of time to wait until the group starts to form")]
        public SharedFloat waitTime = 0;
        [Tooltip("The raycast offset relative to the pivot position")]
        public SharedVector3 attackOffset;
        [Tooltip("The target raycast offset relative to the pivot position")]
        public SharedVector3 targetOffset;
        [Tooltip("Should the target bone be used?")]
        public SharedBool useTargetBone;
        [Tooltip("The target's bone if the target is a humanoid")]
        public HumanBodyBones targetBone;

        private bool sendListenerEvent;
        private TaskStatus runStatus;
        private GameObject prevLeader;

        protected List<Behavior> formationTrees;
        protected TacticalAgent tacticalAgent;
        protected List<Transform> agents;
        protected int formationIndex = -1;
        protected bool started;
        protected bool canAttack;
        private List<bool> agentsReady = new List<bool>();
        protected Behavior leaderTree;
        protected List<IDamageable> targets = new List<IDamageable>();
        protected List<Transform> targetTransforms = new List<Transform>();

        /// <summary>
        /// Listen for any agents that want to join the group.
        /// </summary>
        public override void OnAwake()
        {
            Owner.RegisterEvent<Behavior>("StartListeningForOrders", StartListeningForOrders);
            Owner.RegisterEvent<Behavior>("StopListeningToOrders", StopListeningToOrders);
            Owner.RegisterEvent<int>("FormationUpdated", FormationUpdated);
            Owner.RegisterEvent<Behavior, int>("AddAgentToGroup", AddAgentToGroup);
            Owner.RegisterEvent<int, bool>("UpdateInPosition", UpdateInPosition);
            Owner.RegisterEvent<Transform, IDamageable>("AddTarget", AddTarget);
            Owner.RegisterEvent<bool>("Attack", Attack);
            Owner.RegisterEvent<bool>("SendAttackEvent", SendAttackEvent);
            Owner.RegisterEvent<TaskStatus>("OrdersFinished", OrdersFinished);
        }

        /// <summary>
        /// Start forming the group immediately on start or after a set amount of time.
        /// </summary>
        public override void OnStart()
        {
            UpdateLeader();

            canAttack = false;
            runStatus = TaskStatus.Running;
        }

        /// <summary>
        /// The leader has changed. Update the leader tree.
        /// </summary>
        private void UpdateLeader()
        {
            if (leader.Value == null) {
                formationIndex = 0;
                AddAgentToGroup(Owner, 0);
                FormationUpdated(0);
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
            }
            canAttack = false;
            prevLeader = leader.Value;

            if (waitTime.Value == 0) {
                StartGroup();
            } else {
                StartCoroutine(WaitForGroup());
            }
        }

        /// <summary>
        /// Wait a small amount of time before the group is formed.
        /// </summary>
        private IEnumerator WaitForGroup()
        {
            yield return new WaitForSeconds(waitTime.Value);

            StartGroup();
        }

        /// <summary>
        /// Start forming the group.
        /// </summary>
        protected virtual void StartGroup()
        {
            started = true;

            // Clear the old group.
            targets.Clear();
            targetTransforms.Clear();
            if (leader.Value == null) {
                if (targetGroup.Value.Count > 0) {
                    for (int i = 0; i < targetGroup.Value.Count; ++i) {
                        var damageable = (targetGroup.Value[i].GetComponentInParent(typeof(IDamageable)) as IDamageable);
                        if (damageable != null) {
                            AddTarget(targetGroup.Value[i].transform, damageable);
                        }
                    }
                } else {
                    var foundAttackGroup = GameObject.FindGameObjectsWithTag(targetTag.Value);
                    for (int i = 0; i < foundAttackGroup.Length; ++i) {
                        var damageable = (foundAttackGroup[i].GetComponentInParent(typeof(IDamageable)) as IDamageable);
                        if (damageable != null) {
                            AddTarget(foundAttackGroup[i].transform, damageable);
                        }
                    }
                }

                if (targets.Count == 0) {
                    Debug.LogError("Error: no target GameObjects have been found. Ensure your targets implement the IDamageable interface.");
                }
            }
        }

        /// <summary>
        /// Adds the target to the target list.
        /// </summary>
        /// <param name="target">The target Transform.</param>
        /// <param name="damageable">The target Damageable.</param>
        private void AddTarget(Transform target, IDamageable damageable)
        {
            targets.Add(damageable);
            targetTransforms.Add(target);
        }

        /// <summary>
        /// An agent wants to join the formation. Add them to the pending group placement list if the group hasn't formed yet, otherwise directly add them to the existing formation.
        /// </summary>
        /// <param name="agent">The agent that wants to join the group.</param>
        protected void StartListeningForOrders(Behavior agent)
        {
            // StartListeningForOrders is registered within OnAwake which could cause the callback to be executed when the task isn't active.
            if (runStatus != TaskStatus.Running) {
                return;
            }

            AddAgentToGroup(agent, agents.Count);
        }

        /// <summary>
        /// Adds the agent to the agent list.
        /// </summary>
        /// <param name="agent">The agent to add.</param>
        /// <param name="index">The index of the agent within the group.</param>
        protected virtual void AddAgentToGroup(Behavior agent, int index)
        {
            if (leader.Value == null) {
                if (formationTrees == null) {
                    formationTrees = new List<Behavior>();
                    agentsReady = new List<bool>();
                }

                // Notify the current agent of the existing agents.
                for (int i = 0; i < formationTrees.Count; ++i) {
                    agent.SendEvent("AddAgentToGroup", formationTrees[i], i);
                }
                // Notify the current agent of the targets.
                if (agent != Owner) {
                    for (int i = 0; i < targets.Count; ++i) {
                        agent.SendEvent("AddTarget", targetTransforms[i], targets[i]);
                    }
                }

                formationTrees.Insert(index, agent);
                agentsReady.Insert(index, false);

                // Notify other agents that the current agent has joined the formation.
                for (int i = 1; i < formationTrees.Count; ++i) {
                    formationTrees[i].SendEvent("AddAgentToGroup", formationTrees[index], index);
                    formationTrees[i].SendEvent("FormationUpdated", i);
                }
            } else {
                sendListenerEvent = false;
            }

            // The agents array is maintained on both the leader and follower.
            if (agents == null) {
                agents = new List<Transform>();
            }
            agents.Insert(index, agent.transform);
        }

        /// <summary>
        /// Update the leader or follower in position status.
        /// </summary>
        /// <param name="index">The index of the agent to update.</param>
        /// <param name="status">The in position status of the index.</param>
        protected void UpdateInPosition(int index, bool inPosition)
        {
            // UpdateInPosition is registered within OnAwake which could cause the callback to be executed when the task isn't active.
            if (runStatus != TaskStatus.Running) {
                return;
            }

            agentsReady[index] = inPosition;
            var allReady = inPosition;
            for (int i = 0; i < agentsReady.Count; ++i) {
                if (agentsReady[i] != inPosition) {
                    allReady = !inPosition;
                    break;
                }
            }
            if (allReady == inPosition) {
                SendAttackEvent(inPosition);
            }
        }

        /// <summary>
        /// Sends the Attack event to all following agents.
        /// </summary>
        /// <param name="attack">Can the agent attack?</param>
        protected void SendAttackEvent(bool attack)
        {
            for (int i = 0; i < formationTrees.Count; ++i) {
                formationTrees[i].SendEvent("Attack", attack);
            }
        }

        /// <summary>
        /// Specifies if the agent can attack.
        /// </summary>
        /// <param name="attack">Can the agent start attacking?</param>
        private void Attack(bool attack)
        {
            canAttack = attack;
        }

        /// <summary>
        /// Base OnUpdate method. Return success if no targets are alive, otherwise return running.
        /// </summary>
        public override TaskStatus OnUpdate()
        {
            // If the leader has changed then reinitialize with the new leader.
            if (prevLeader != leader.Value) {
                EndFormation();
                UpdateLeader();
            }

            // Send within OnUpdate to ensure the at least one leader behavior tree is active. If registered within OnStart there is a chance that the behavior tree
            // isn't active yet and will never receive the event.
            if (sendListenerEvent) {
                leaderTree.SendEvent("StartListeningForOrders", Owner);
                return runStatus;
            }

            // There won't be any agents in the group if the group hasn't formed yet.
            if (formationIndex == -1) {
                return TaskStatus.Running;
            }

            // Remove any targets that are no logner alive
            for (int i = targets.Count - 1; i > -1; --i) {
                if (!targets[i].IsAlive()) {
                    targets.RemoveAt(i);
                    targetTransforms.RemoveAt(i);
                }
            }

            // The task succeeded if no more targets are alive.
            if (targets.Count == 0) {
                return TaskStatus.Success;
            }

            return TaskStatus.Running;
        }

        /// <summary>
        /// The formation has changed. Update the formation index.
        /// </summary>
        /// <param name="index">The new formation index.</param>
        protected virtual void FormationUpdated(int index)
        {
            formationIndex = index;
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
        /// Returns the center position of all of the target transforms.
        /// </summary>
        /// <returns>The center position of all of the target transforms.</returns>
        protected Vector3 CenterAttackPosition()
        {
            var position = Vector3.zero;
            for (int i = 0; i < targetTransforms.Count; ++i) {
                position += targetTransforms[i].position;
            }
            return position / targetTransforms.Count;
        }

        /// <summary>
        /// Returns the look rotation from the target to the center position.
        /// </summary>
        /// <param name="centerPosition">The position of the attack center.</param>
        /// <returns>The look rotation from the target to the center position.</returns>
        protected Quaternion CenterAttackRotation(Vector3 centerPosition)
        {
            var leaderTransform = leader.Value != null ? leader.Value.transform : transform;
            var direction = leaderTransform.position - centerPosition;
            direction.y = 0;
            return Quaternion.LookRotation(direction);
        }

        /// <summary>
        /// Returns the look rotation from the center position to the target.
        /// </summary>
        /// <param name="centerPosition">The position of the center position.</param>
        /// <returns>The look roation from the center position to the target.</returns>
        protected Quaternion ReverseCenterAttackRotation(Vector3 centerPosition)
        {
            var leaderTransform = leader.Value != null ? leader.Value.transform : transform;
            var direction = centerPosition - leaderTransform.position;
            direction.y = 0;
            return Quaternion.LookRotation(direction);
        }

        /// <summary>
        /// Finds the closest target transform to the agent transform.
        /// </summary>
        /// <param name="agentTransform">The transform of the agent.</param>
        /// <param name="targetTransform">The returned target transform.</param>
        /// <param name="targetDamagable">The returned IDamagable reference.</param>
        protected void ClosestTarget(Transform agentTransform, ref Transform targetTransform, ref IDamageable targetDamagable)
        {
            var distance = float.MaxValue;
            var localDistance = 0f;
            for (int i = targetTransforms.Count - 1; i > -1; --i) {
                if (targets[i].IsAlive()) {
                    if ((localDistance = (targetTransforms[i].position - agentTransform.position).sqrMagnitude) < distance) {
                        distance = localDistance;
                        targetTransform = targetTransforms[i];
                        targetDamagable = targets[i];
                    }
                } else {
                    targets.RemoveAt(i);
                    targetTransforms.RemoveAt(i);
                }
            }
        }

        /// <summary>
        /// Finds a target transform closest to the agent.
        /// </summary>
        protected void FindAttackTarget()
        {
            if (tacticalAgent.TargetTransform == null || !tacticalAgent.TargetDamagable.IsAlive()) {
                Transform target = null;
                IDamageable damageable = null;
                ClosestTarget(transform, ref target, ref damageable);
                if (useTargetBone.Value) {
                    Animator targetAnimator;
                    if ((targetAnimator = target.GetComponent<Animator>()) != null) {
                        var bone = targetAnimator.GetBoneTransform(targetBone);
                        if (bone != null) {
                            target = bone;
                        }
                    }
                }
                tacticalAgent.TargetTransform = target;
                tacticalAgent.TargetDamagable = damageable;
            }
        }

        /// <summary>
        /// Moves the agent towards and rotates towards the target transform.
        /// </summary>
        protected bool MoveToAttackPosition()
        {
            FindAttackTarget();
            if (tacticalAgent.TargetTransform == null) {
                return false;
            }
            if (!tacticalAgent.CanSeeTarget() ||
                    Vector3.Distance(tacticalAgent.TargetTransform.position, transform.position) > tacticalAgent.AttackAgent.AttackDistance()) {
                tacticalAgent.SetDestination(tacticalAgent.TargetTransform.position);
                tacticalAgent.UpdateRotation(true);
                tacticalAgent.AttackPosition = true;
            } else {
                tacticalAgent.Stop();

                return tacticalAgent.RotateTowardsPosition(tacticalAgent.TargetTransform.position);
            }
            return false;
        }

        /// <summary>
        /// The task has ended. Stop any active agents.
        /// </summary>
        public override void OnEnd()
        {
            EndFormation();
        }

        /// <summary>
        /// Ends the tactical formation.
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
                formationTrees.Clear();
                agentsReady.Clear();
            } else {
                if (leaderTree != null) {
                    leaderTree.SendEvent("StopListeningToOrders", Owner);
                }
            }
            if (tacticalAgent != null) {
                tacticalAgent.UpdateRotation(true);
            }
            formationIndex = -1;
            if (agents != null) {
                agents.Clear();
            }
        }

        /// <summary>
        /// An agent has dropped out of the group so it should be removed.
        /// </summary>
        /// <param name="agent">The agent to remove.</param>
        protected void StopListeningToOrders(Behavior agent)
        {
            // StopListeningToOrders is registered within OnAwake which could cause the callback to be executed when the task isn't active.
            if (runStatus != TaskStatus.Running) {
                return;
            }

            // The group has been formed, remove it from the group list.
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
                        agentsReady.RemoveAt(i);
                        for (int j = 1; j < formationTrees.Count; ++j) {
                            formationTrees[j].SendEvent("StopListeningToOrders", agent);
                            formationTrees[j].SendEvent("FormationUpdated", j);
                        }
                    }
                    agents.RemoveAt(i);
                    return i;
                }
            }
            return -1;
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
            Owner.UnregisterEvent<int, bool>("UpdateInPosition", UpdateInPosition);
            Owner.UnregisterEvent<Transform, IDamageable>("AddTarget", AddTarget);
            Owner.UnregisterEvent<bool>("Attack", Attack);
            Owner.UnregisterEvent<bool>("SendAttackEvent", SendAttackEvent);
            Owner.UnregisterEvent<TaskStatus>("OrdersFinished", OrdersFinished);
        }

        /// <summary>
        /// Reset the public variables back to their defaults.
        /// </summary>
        public override void OnReset()
        {
            targetGroup = null;
            targetTag = "";
            waitTime = 0;
        }

        /// <summary>
        /// Transforms position from local space to world space.
        /// </summary>
        protected static Vector3 TransformPoint(Vector3 worldPosition, Vector3 localOffset, Quaternion rotation)
        {
            return worldPosition + rotation * localOffset;
        }

        /// <summary>
        /// Transforms position from world space to local space.
        /// </summary>
        protected static Vector3 InverseTransformPoint(Vector3 position1, Vector3 position2, Quaternion rotation)
        {
            return Quaternion.Inverse(rotation) * (position1 - position2);
        }
    }
}