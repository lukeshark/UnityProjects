using UnityEngine;
#if !(UNITY_5_1 || UNITY_5_2 || UNITY_5_3 || UNITY_5_4)
using UnityEngine.AI;
#endif
using HelpURL = BehaviorDesigner.Runtime.Tasks.HelpURLAttribute;

namespace BehaviorDesigner.Runtime.Tactical.Tasks
{
    /// <summary>
    /// Base class for all NavMeshAgent Tactical tasks.
    /// </summary>
    public abstract class NavMeshTacticalGroup : TacticalGroup
    {
        /// <summary>
        /// The NavMeshTacticalAgent class contains component references and variables for each NavMeshAgent.
        /// </summary>
        private class NavMeshTacticalAgent : TacticalAgent
        {
            private NavMeshAgent navMeshAgent;
            private bool destinationSet;

            /// <summary>
            /// Caches the component references and initialize default values.
            /// </summary>
            public NavMeshTacticalAgent(Transform agent) : base(agent)
            {
                navMeshAgent = agent.GetComponent<NavMeshAgent>();

                if (navMeshAgent.hasPath) {
                    navMeshAgent.ResetPath();
#if UNITY_5_1 || UNITY_5_2 || UNITY_5_3 || UNITY_5_4 || UNITY_5_5
                    navMeshAgent.Stop();
#else
                    navMeshAgent.isStopped = true;
#endif
                }
            }

            /// <summary>
            /// Sets the destination.
            /// </summary>
            public override void SetDestination(Vector3 destination)
            {
                destinationSet = true;
                destination.y = navMeshAgent.destination.y;
                if (navMeshAgent.destination != destination) {
                    navMeshAgent.SetDestination(destination);
#if UNITY_5_1 || UNITY_5_2 || UNITY_5_3 || UNITY_5_4 || UNITY_5_5
                    navMeshAgent.Resume();
#else
                    navMeshAgent.isStopped = false;
#endif
                }
            }

            /// <summary>
            /// Has the agent arrived at its destination?
            /// </summary>
            public override bool HasArrived()
            {
                return destinationSet && !navMeshAgent.pathPending && (transform.position - navMeshAgent.destination).magnitude <= navMeshAgent.stoppingDistance;
            }

            /// <summary>
            /// Rotates towards the target rotation.
            /// </summary>
            public override bool RotateTowards(Quaternion targetRotation)
            {
                if (navMeshAgent.updateRotation) {
                    navMeshAgent.updateRotation = false;
                }
                transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, navMeshAgent.angularSpeed * Time.deltaTime);
                if (Quaternion.Angle(transform.rotation, targetRotation) < AttackAgent.AttackAngle()) {
                    return true;
                }
                return false;
            }

            /// <summary>
            /// Returns the radius of the agent.
            /// </summary>
            public override float Radius()
            {
                return navMeshAgent.radius;
            }

            /// <summary>
            /// Starts or stops the rotation from updating.
            /// </summary>
            public override void UpdateRotation(bool update)
            {
                navMeshAgent.updateRotation = update;
            }

            /// <summary>
            /// Stops the agent from moving.
            /// </summary>
            public override void Stop()
            {
                if (navMeshAgent.hasPath) {
#if UNITY_5_1 || UNITY_5_2 || UNITY_5_3 || UNITY_5_4 || UNITY_5_5
                    navMeshAgent.Stop();
#else
                    navMeshAgent.isStopped = true;
#endif
                    destinationSet = false;
                }
            }

            /// <summary>
            /// The task has ended. Perform any cleanup.
            /// </summary>
            public override void End()
            {
                Stop();
                navMeshAgent.updateRotation = true;
                navMeshAgent.velocity = Vector3.zero;
            }
        }

        /// <summary>
        /// Adds the agent to the agent list.
        /// </summary>
        /// <param name="agent">The agent to add.</param>
        protected override void AddAgentToGroup(Behavior agent, int index)
        {
            base.AddAgentToGroup(agent, index);

            if (tacticalAgent == null && gameObject == agent.gameObject) {
                tacticalAgent = new NavMeshTacticalAgent(agent.transform);
                tacticalAgent.AttackOffset = attackOffset.Value;
                tacticalAgent.TargetOffset = targetOffset.Value;
            }
        }
    }
}