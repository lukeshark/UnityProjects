using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

namespace BehaviorDesigner.Runtime.Tasks.UFPS
{
    [TaskDescription("Removes the specified item from the inventory.")]
    [HelpURL("http://www.opsive.com/assets/BehaviorDesigner/documentation.php?id=99")]
    [TaskCategory("UFPS")]
    [TaskIcon("Assets/Behavior Designer/Third Party/UltimateFPS/Editor/Icon.png")]
    public class RemoveItem : Action
    {
        [Tooltip("The item to remove")]
        public SharedString itemName;

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
            return agent.RemoveItem.Try(new object[] { itemName.Value }) ? TaskStatus.Success : TaskStatus.Failure;
        }

        public override void OnReset()
        {
            itemName = "";
            agent = null;
        }
    }
}