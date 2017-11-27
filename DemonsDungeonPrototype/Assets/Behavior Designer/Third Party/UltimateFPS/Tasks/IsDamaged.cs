using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

namespace BehaviorDesigner.Runtime.Tasks.UFPS
{
    [TaskDescription("Did the Damage Handler take damage?")]
    [HelpURL("http://www.opsive.com/assets/BehaviorDesigner/documentation.php?id=99")]
    [TaskCategory("UFPS")]
    [TaskIcon("Assets/Behavior Designer/Third Party/UltimateFPS/Editor/Icon.png")]
    public class IsDamaged : Conditional
    {
        public SharedGameObject targetGameObject;

        private vp_DamageHandler damageHandler;
        private float prevHealth;

        public override void OnAwake()
        {
            damageHandler = GetDefaultGameObject(targetGameObject.Value).GetComponent<vp_DamageHandler>();
            if (damageHandler == null) {
                Debug.LogError("Error: Unable to find the vp_DamageHandler component on " + targetGameObject.Value);
                return;
            }

            prevHealth = damageHandler.CurrentHealth;
        }

        public override TaskStatus OnUpdate()
        {
            if (damageHandler.CurrentHealth < prevHealth) {
                prevHealth = damageHandler.CurrentHealth;
                return TaskStatus.Success;
            }
            return TaskStatus.Failure;
        }

        public override void OnReset()
        {
            targetGameObject = null;
        }
    }
}