using UnityEngine;


namespace BehaviorDesigner.Runtime.Tactical
{
    /// <summary>
    /// The TacticalAgent class contains component references and variables for each TacticalAgent.
    /// </summary>
    public abstract class TacticalAgent
    {
        private static int ignoreRaycast = ~(1 << LayerMask.NameToLayer("Ignore Raycast"));

        protected Transform transform;
        private IAttackAgent attackAgent;
        private Transform targetTransform = null;
        private IDamageable targetDamagable = null;
        private bool attackPosition = false;
        private Vector3 attackOffset;
        private Vector3 targetOffset;

        public IAttackAgent AttackAgent { get { return attackAgent; } }
        public Transform TargetTransform { get { return targetTransform; } set { targetTransform = value; } }
        public IDamageable TargetDamagable { get { return targetDamagable; } set { targetDamagable = value; } }
        public bool AttackPosition { get { return attackPosition; } set { attackPosition = value; } }
        public Vector3 AttackOffset { set { attackOffset = value; } }
        public Vector3 TargetOffset { set { targetOffset = value; } }

        /// <summary>
        /// Caches the component referneces.
        /// </summary>
        public TacticalAgent(Transform agent)
        {
            transform = agent;
            attackAgent = agent.GetComponent(typeof(IAttackAgent)) as IAttackAgent;
        }

        /// <summary>
        /// Sets the destination.
        /// </summary>
        public abstract void SetDestination(Vector3 destination);

        /// <summary>
        /// Has the agent arrived at its destination?
        /// </summary>
        public abstract bool HasArrived();

        /// <summary>
        /// Rotates towards the target rotation.
        /// </summary>
        public abstract bool RotateTowards(Quaternion targetRotation);

        /// <summary>
        /// Returns the radius of the agent.
        /// </summary>
        public abstract float Radius();

        /// <summary>
        /// Starts or stops the rotation from updating. Not all implementations will use this.
        /// </summary>
        public abstract void UpdateRotation(bool update);

        /// <summary>
        /// Stops the agent from moving.
        /// </summary>
        public abstract void Stop();

        /// <summary>
        /// The task has ended. Perform any cleanup.
        /// </summary>
        public abstract void End();

        /// <summary>
        /// Looks at position parameter.
        /// </summary>
        public bool RotateTowardsPosition(Vector3 position)
        {
            var targetRotation = Quaternion.LookRotation(position - transform.position);
            return RotateTowards(targetRotation);
        }

        /// <summary>
        /// Can the agent see the target transform?
        /// </summary>
        public bool CanSeeTarget()
        {
            var distance = (transform.position - targetTransform.position).magnitude;
            if (distance >= attackAgent.AttackDistance()) {
                return false;
            }
            RaycastHit hit;
            if (Physics.Linecast(transform.TransformPoint(attackOffset), targetTransform.TransformPoint(targetOffset), out hit, ignoreRaycast)) {
                if (ContainsTransform(targetTransform, hit.transform)) {
                    return true; // return the target object meaning it is within sight
                }
            } else if (targetTransform.GetComponent<Collider>() == null || targetTransform.GetComponent<CharacterController>() != null) {
                // If the linecast doesn't hit anything then that the target object doesn't have a collider and there is nothing in the way
                if (targetTransform.gameObject.activeSelf) {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Attacks the target.
        /// </summary>
        public bool TryAttack()
        {
            if (attackAgent.CanAttack()) {
                attackAgent.Attack(targetTransform.position);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Returns true if the target transform is a child of the parent transform
        /// </summary>
        private static bool ContainsTransform(Transform target, Transform parent)
        {
            if (target == null) {
                return false;
            }
            if (target.Equals(parent)) {
                return true;
            }
            return ContainsTransform(target.parent, parent);
        }
    }
}