using UnityEngine;
using System.Collections;
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
    public abstract class GroupFormation : Action
    {
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
        [Tooltip("The agent move speed when the agent is too far ahead of the leader and should slow down")]
        public SharedFloat slowdownSpeed = 1.5f;
        [Tooltip("The amount of time to wait until the group starts forming")]
        public SharedFloat waitTime = 0;
        [Tooltip("Should the current agent lead the group? If false the closest agent to the destination will lead")]
        public SharedBool isLeader;

        protected List<Transform> transforms = new List<Transform>();
        protected List<UnityEngine.AI.NavMeshAgent> agents = new List<UnityEngine.AI.NavMeshAgent>();
        protected List<bool> destinationPending = new List<bool>();

        private List<GameObject> pendingGroupPlacement = new List<UnityEngine.GameObject>();
        private bool initialFormation = false;

        /// <summary>
        /// Listen for any agents that want to join the group.
        /// </summary>
        public override void OnAwake()
        {
            Owner.RegisterEvent<GameObject>("StartListeningForOrders", StartListeningForOrders);
            Owner.RegisterEvent<GameObject>("StopListeningToOrders", StopListeningToOrders);
        }

        /// <summary>
        /// Start forming the group immediately on start or after a set amount of time.
        /// </summary>
        public override void OnStart()
        {
            if (waitTime.Value == 0) {
                StartFormation();
            } else {
                StartCoroutine(WaitForGroupFormation());
            }
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
        /// Start forming the group.
        /// </summary>
        private void StartFormation()
        {
            // The closest agent goes first. If the current agent is the leader then it should be placed at the start of the list regardless of distance.
            if (!isLeader.Value) {
                pendingGroupPlacement.Add(gameObject);
            }
            var target = (targetTransform.Value != null ? targetTransform.Value.position : targetPosition.Value);
            var distances = new float[pendingGroupPlacement.Count];
            var agentsByDistance = new Transform[distances.Length];
            for (int i = 0; i < pendingGroupPlacement.Count; ++i) {
                agentsByDistance[i] = pendingGroupPlacement[i].transform;
                distances[i] = Vector3.Distance(agentsByDistance[i].position, target);
            }
            System.Array.Sort(distances, agentsByDistance);
            pendingGroupPlacement.Clear();

            // Clear the old group.
            agents.Clear();
            transforms.Clear();
            destinationPending.Clear();

            // The leader goes first.
            if (isLeader.Value) {
                AddAgentToGroup(transform);
            }

            // Add the agents to the group according to their distance.
            for (int i = 0; i < agentsByDistance.Length; ++i) {
                AddAgentToGroup(agentsByDistance[i]);
            }

            // Start the movement.
            agents[0].SetDestination(target);
            initialFormation = true;
        }

        /// <summary>
        /// An agent wants to join the formation. Add them to the pending group placement list if the group hasn't formed yet, otherwise directly add them to the existing formation.
        /// </summary>
        /// <param name="obj">The agent that wants to join the group.</param>
        protected void StartListeningForOrders(GameObject obj)
        {
            // Add the agent to the pending group placement list if the group hasn't formed yet.
            if (agents == null || agents.Count == 0) {
                pendingGroupPlacement.Add(obj);
            } else { // The group is already in formation so add the new agent.
                AddAgentToGroup(obj.transform);
            }
        }

        /// <summary>
        /// Adds the agent to the formation lists.
        /// </summary>
        /// <param name="agent">The agent to add.</param>
        protected virtual void AddAgentToGroup(Transform agent)
        {
            agents.Add(agent.GetComponent<UnityEngine.AI.NavMeshAgent>());
            transforms.Add(agent);
            destinationPending.Add(false);
            agents[agents.Count - 1].Resume();
        }

        /// <summary>
        /// Move the agents in a formation. The TargetPosition method will retrieve the target position for the individual group member.
        /// </summary>
        public override TaskStatus OnUpdate()
        {
            // There won't be any agents in the group if the group hasn't formed yet.
            if (agents.Count == 0) {
                return TaskStatus.Running;
            }

            // Return success as soon as all agents have arrived. When the group is initially forming the NavMeshAgent destinations need to be updated
            // so wait a tick before checking for arrival.
            if (!initialFormation) {
                var arrived = true;
                for (int i = 0; i < agents.Count; ++i) {
                    if (agents[i].remainingDistance > 0.01f || agents[i].pathPending) {
                        arrived = false;
                        break;
                    }
                }
                if (arrived) {
                    for (int i = agents.Count - 1; i > -1; --i) {
                        agents[i].Stop();
                        agents[i].GetComponent<BehaviorTree>().SendEvent<TaskStatus>("OrdersFinished", TaskStatus.Success);
                    }

                    return TaskStatus.Success;
                }
            }

            var target = (targetTransform.Value != null ? targetTransform.Value.position : targetPosition.Value);

            // Move the group towards the target in formation.
            var inFormation = agents.Count > 1;
            for (int i = 1; i < agents.Count; ++i) {

                // If the destination is immediately in front of the agent then that agents stopping distance will take over and slow down the agent. This will make the agent
                // lag behind the leader. Prevent this from happening by adding a small look ahead distance.
                var zLookAhead = Mathf.Min(agents[0].remainingDistance, !initialFormation ? lookAhead.Value : agents[i].radius * 2);

                // TargetPosition will be overridden to return the target position for the individual agent.
                var targetPos = TargetPosition(i, zLookAhead);

                // Always move if the target distance is less than the distance to the destination or the agent is already moving. The target distance may not be less than the destination distance
                // if the target is 'behind' the current agent.
                var destinationDistance = (targetPos - target).sqrMagnitude;
                var targetDistance = (target - transforms[i].position).sqrMagnitude;
                if (destinationDistance < targetDistance || agents[i].hasPath) {
                    agents[i].SetDestination(targetPos);
                    // The target position could be invalid in which case the agent should move towards the actual destination so a better path is generated and the 
                    // agent smoothly moves around the obstacle.
                    if (agents[i].hasPath && !destinationPending[i] && (targetPos - agents[i].pathEndPosition).sqrMagnitude > 0.5f) {
                        targetPos = TargetPosition(i, agents[0].remainingDistance);
                        agents[i].SetDestination(targetPos);
                        destinationPending[i] = true;
                    } else {
                        destinationPending[i] = false;
                    }

                    // Move the agent at full speed if they are currently far away from their destination.
                    // Remaining distance will be 0 when the agent first gets a target position.
                    if (agents[i].remainingDistance > zLookAhead || !agents[i].hasPath || 
                            (initialFormation && Mathf.Abs((Quaternion.Inverse(transforms[0].rotation) * (targetPos - transforms[i].position)).x) > agents[i].radius)) {
                        agents[i].speed = fullSpeed.Value;
                        inFormation = false;
                    } else if (agents[i].remainingDistance - zLookAhead < -agents[i].radius / 2) {
                        // The agent is too far ahead of the leader so should slow down.
                        agents[i].speed = slowdownSpeed.Value;
                    } else {
                        // The agent is close to its destination so move at the same speed as the leader.
                        agents[i].speed = agents[0].speed;
                    }
                } else {
                    // Something is preventing the current agent from moving so the squad is not in formation.
                    inFormation = false;
                }
            }
            // When the agents are in formation the initial formation is over. The agents can look ahead the full distance now.
            if (inFormation) {
                initialFormation = false;
            }

            // Set the leader's destination and speed. The leader won't move at full speed until all other members of the group are moving at full speed.
            agents[0].SetDestination(target);
            agents[0].speed = inFormation ? fullSpeed.Value : formationSpeed.Value;

            return TaskStatus.Running;
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
        /// The task has ended. Stop any active agents.
        /// </summary>
        public override void OnEnd()
        {
            for (int i = 0; i < agents.Count; ++i) {
                if (agents[i].pathPending) {
                    agents[i].Stop();
                    agents[i].GetComponent<BehaviorTree>().SendEvent<TaskStatus>("OrdersFinished", TaskStatus.Failure);
                }
            }

            agents.Clear();
            transforms.Clear();
            destinationPending.Clear();
            pendingGroupPlacement.Clear();
        }

        /// <summary>
        /// An agent has dropped out of the group so it should be removed.
        /// </summary>
        /// <param name="obj">The agent to remove.</param>
        protected void StopListeningToOrders(GameObject obj)
        {
            // The agent may drop before the group is formed so in that case just remove it from the pending list.
            for (int i = 0; i < pendingGroupPlacement.Count; ++i) {
                if (pendingGroupPlacement[i].Equals(obj)) {
                    pendingGroupPlacement.RemoveAt(i);
                    return;
                }
            }

            // The group has been formed, remove it from the group list.
            RemoveAgentFromGroup(obj.transform);
        }

        /// <summary>
        /// Removes the agent from the group.
        /// </summary>
        /// <param name="agent">The agent to remove.</param>
        /// <returns>The index of the agent removed from the group.</returns>
        protected virtual int RemoveAgentFromGroup(Transform agent)
        {
            for (int i = 0; i < transforms.Count; ++i) {
                if (transforms[i].Equals(agent)) {
                    transforms.RemoveAt(i);
                    agents.RemoveAt(i);
                    destinationPending.RemoveAt(i);
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
            Owner.UnregisterEvent<GameObject>("StartListeningForOrders", StartListeningForOrders);
            Owner.UnregisterEvent<GameObject>("StopListeningToOrders", StopListeningToOrders);
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
            waitTime = 0;
            isLeader = false;
        }
    }
}