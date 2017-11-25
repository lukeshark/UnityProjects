using UnityEngine;
using System.Collections;

namespace BehaviorDesigner.Runtime.Tasks.UFPS
{
    [RequireComponent(typeof(AudioSource))]
    public class AIWeapon : vp_Weapon
    {
        private Vector3 m_startPosition;
        private Quaternion m_startRotation;
        private Vector3 m_startScale;

        protected override void Awake()
        {
            m_startPosition = transform.localPosition;
            m_startRotation = transform.localRotation;
            m_startScale = transform.localScale;

            base.Awake();
        }

        // Override Start, Update, and FixedUpdate to prevent them from doing anything. The agent's animations will control the weapon movement. This script exists so vp_Inventory can find the weapon.
        protected override void Start()
        {
        }

        protected override void Update()
        {
        }

        protected override void FixedUpdate()
        {
        }

        public override void Wield(bool showWeapon)
        {
            base.Wield(showWeapon);

            transform.localScale = m_startScale;
            transform.localPosition = m_startPosition;
            transform.localRotation = m_startRotation;
        }
    }
}