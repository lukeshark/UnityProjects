using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

namespace BehaviorDesigner.Runtime.Tasks.UFPS
{
    [TaskDescription("Returns success if the damage handler has a health greater than 0, otherwise failure.")]
    [HelpURL("http://www.opsive.com/assets/BehaviorDesigner/documentation.php?id=99")]
    [TaskCategory("UFPS")]
    [TaskIcon("Assets/Behavior Designer/Third Party/UltimateFPS/Editor/Icon.png")]
    public class IsDamagableAlive : Conditional
    {
        [Tooltip("The damage handler to check the health of")]
        [RequiredField]
        public SharedDamageHandler damageHandler;

        public override TaskStatus OnUpdate()
        {
            if (damageHandler == null || damageHandler.Value == null) {
                return TaskStatus.Failure;
            }

            return damageHandler.Value.CurrentHealth > 0 ? TaskStatus.Success : TaskStatus.Failure;
        }

        public override void OnReset()
        {
            damageHandler = null;
        }
    }
}