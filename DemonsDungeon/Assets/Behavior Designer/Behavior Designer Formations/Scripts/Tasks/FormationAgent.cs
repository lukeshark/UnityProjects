using UnityEngine;

namespace BehaviorDesigner.Runtime.Formations.Tasks
{
    /// <summary>
    /// The FormationAgent class contains component references and variables for each FormationAgent.
    /// </summary>
    public abstract class FormationAgent
    {
        protected Transform transform;

        public abstract float Speed { set; }
        public abstract float Radius { get; }
        public abstract float RemainingDistance { get; }
        public abstract float StoppingDistance { get; }
        public abstract bool HasPath { get; }
        public abstract bool PathPending { get; }
        public abstract bool AutoBreaking { set; }

        /// <summary>
        /// Caches the component referneces.
        /// </summary>
        public FormationAgent(Transform agent)
        {
            transform = agent;
        }

        /// <summary>
        /// Resumes pathfinding.
        /// </summary>
        public abstract void Resume();

        /// <summary>
        /// Sets the destination.
        /// </summary>
        public abstract void SetDestination(Vector3 destination);

        /// <summary>
        /// Rotates towards the target rotation.
        /// </summary>
        public abstract bool RotateTowards(Quaternion targetRotation);

        /// <summary>
        /// Stops the agent from moving.
        /// </summary>
        public abstract void Stop();

        /// <summary>
        /// The task has ended. Perform any cleanup.
        /// </summary>
        public abstract void End();
    }
}