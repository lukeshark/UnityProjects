using UnityEngine;

namespace BehaviorDesigner.Runtime.Tactical
{
    /// <summary>
    /// Interface for an agent that is able to attack.
    /// </summary>
    public interface IAttackAgent
    {
        /// <summary>
        /// Returns the furthest distance that the agent is able to attack from.
        /// </summary>
        /// <returns>The distance that the agent can attack from.</returns>
        float AttackDistance();

        /// <summary>
        /// Can the agent attack?
        /// </summary>
        /// <returns>Returns true if the agent can attack.</returns>
        bool CanAttack();

        /// <summary>
        /// Returns the maximum angle that the agent can attack from.
        /// </summary>
        /// <returns>The maximum angle that the agent can attack from.</returns>
        float AttackAngle();

        /// <summary>
        /// Does the actual attack.
        /// </summary>
        /// <param name="targetPosition">The position to attack.</param>
        void Attack(Vector3 targetPosition);
    }
}