using UnityEngine;
using System.Collections;

namespace BehaviorDesigner.Runtime.Tasks.UFPS
{
    [RequireComponent(typeof(AIWeapon))]
    public class AIWeaponShooter : vp_Shooter
    {
        vp_PlayerEventHandler m_Player = null;
        vp_PlayerEventHandler Player
        {
            get
            {
                if (m_Player == null) {
                    if (EventHandler != null)
                        m_Player = (vp_PlayerEventHandler)EventHandler;
                }
                return m_Player;
            }
        }

        protected override void Update()
        {

            base.Update();

            if (Player.Attack.Active) {
                TryFire();
                Player.Attack.Stop();
            }
        }

        public override bool TryFire()
        {
            // return if we can't fire yet
            if (Time.time < m_NextAllowedFireTime) {
                return false;
            }

            if (Player.SetWeapon.Active) {
                return false;
            }

            if (!Player.DepleteAmmo.Try()) {
                return false;
            }
            Fire();
            return true;
        }
    }
}