using UnityEngine;

namespace BehaviorDesigner.Runtime.Tactical
{
    public class Bullet : MonoBehaviour
    {
        // The speed of the bullet
        public float speed;
        // The amount of damage the bullet does
        public float damageAmount = 5;
        // Destroy itself after this amount of time
        public float selfDestructTime = 5;

        private Rigidbody m_Rigidbody;
        private Transform m_Transform;

        /// <summary>
        /// Cache the component references and initialize the default values.
        /// </summary>
        private void Awake()
        {
            m_Rigidbody = GetComponent<Rigidbody>();
            m_Transform = transform;

            Invoke("SelfDestruct", selfDestructTime);
        }

        /// <summary>
        /// Move in the forward direction.
        /// </summary>
        void Update()
        {
            m_Rigidbody.MovePosition(m_Rigidbody.position + speed * m_Transform.forward * Time.deltaTime);
        }

        /// <summary>
        /// Perform any damage to the collided object and destroy itself.
        /// </summary>
        /// <param name="collision"></param>
        private void OnCollisionEnter(Collision collision)
        {
            IDamageable damageable;
            if ((damageable = collision.gameObject.GetComponent(typeof(IDamageable)) as IDamageable) != null) {
                damageable.Damage(damageAmount);
                Destroy(gameObject);
            }
        }

        /// <summary>
        /// Destroy itself.
        /// </summary>
        private void SelfDestruct()
        {
            Destroy(gameObject);
        }
    }
}