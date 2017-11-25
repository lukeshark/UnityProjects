using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

namespace BehaviorDesigner.Runtime.Tasks.UFPS
{
    [TaskDescription("Aims and attacks with the current weapon. Returns success after the weapon has been used to attack.")]
    [HelpURL("http://www.opsive.com/assets/BehaviorDesigner/documentation.php?id=99")]
    [TaskCategory("UFPS")]
    [TaskIcon("Assets/Behavior Designer/Third Party/UltimateFPS/Editor/Icon.png")]
    public class Attack : Action
    {
        [Tooltip("The target to aim for")]
        public SharedTransform target;
        [Tooltip("Specifies how quickly the agent should rotate to aim for the target")]
        public SharedFloat rotationDampening /*= 10*/;
        [Tooltip("If true then the agent will rotates its weapon instead of just its body in order to aim at the target")]
        public SharedBool rotateWeapon /*= true*/;
        [Tooltip("The agent can attack when the angle between the target and the agent is less than this threshold")]
        public SharedFloat aimThreshold /*= 0.4f*/;

        private vp_PlayerEventHandler agent;
        private vp_WeaponHandler weaponHandler;
        private Vector3 weaponUp = Vector3.zero;

        public override void OnAwake()
        {
            agent = gameObject.GetComponent<vp_PlayerEventHandler>();
            if (rotateWeapon.Value) {
                weaponHandler = gameObject.GetComponent<vp_WeaponHandler>();
            }
        }

        public override void OnStart()
        {
            if (rotateWeapon.Value && weaponUp == Vector3.zero) {
                weaponUp = weaponHandler.CurrentWeapon.transform.up;
            }
        }

        public override TaskStatus OnUpdate()
        {
            // Wait to attack until the agent is facing the target
            // ignore the height difference when aiming.
            var lookPos = target.Value.position - transform.position;
            lookPos.y = 0;
            var lookRotation = Quaternion.LookRotation(lookPos);
            var weaponLookRotation = Quaternion.LookRotation(target.Value.position - weaponHandler.CurrentWeapon.Transform.position, weaponUp);

            // If the angle difference is less than aimThreshold then the target is facing in the correct direction
            if (Quaternion.Angle(transform.rotation, lookRotation) < aimThreshold.Value && (!rotateWeapon.Value || Quaternion.Angle(weaponHandler.CurrentWeapon.Transform.rotation, weaponLookRotation) < aimThreshold.Value)) {
                return agent.Attack.TryStart() ? TaskStatus.Success : TaskStatus.Failure;
            }
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationDampening.Value);
            if (rotateWeapon.Value) {
                weaponHandler.CurrentWeapon.Transform.rotation = Quaternion.Slerp(weaponHandler.CurrentWeapon.Transform.rotation,
                                                                 weaponLookRotation, Time.deltaTime * rotationDampening.Value);
            }
            return TaskStatus.Running;
        }

        public override void OnReset()
        {
            target = null;
            rotationDampening = 10;
            rotateWeapon = true;
            aimThreshold = 0.4f;
        }
    }
}