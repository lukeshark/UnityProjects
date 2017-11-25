using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

namespace BehaviorDesigner.Runtime.Tasks.UFPS
{
    [TaskDescription("Sets the current weapon to the weapon specified by its name")]
    [HelpURL("http://www.opsive.com/assets/BehaviorDesigner/documentation.php?id=99")]
    [TaskCategory("UFPS")]
    [TaskIcon("Assets/Behavior Designer/Third Party/UltimateFPS/Editor/Icon.png")]
    public class SetWeaponByName : Action
    {
        [Tooltip("The name of the weapon to set")]
        public SharedString weaponName;

        public vp_PlayerEventHandler agent;

        public override void OnAwake()
        {
            if (agent == null) {
                agent = gameObject.GetComponent<vp_PlayerEventHandler>();
            }
        }

        public override TaskStatus OnUpdate()
        {
            if (agent == null) {
                return TaskStatus.Failure;
            }
            return agent.SetWeaponByName.Try(weaponName.Value) ? TaskStatus.Success : TaskStatus.Failure;
        }

        public override void OnReset()
        {
            weaponName = "";
            agent = null;
        }
    }
}