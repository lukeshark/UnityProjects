using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

namespace BehaviorDesigner.Runtime.Tasks.UFPS
{
    [TaskDescription("Adds a item to the inventory specified by its name and count.")]
    [HelpURL("http://www.opsive.com/assets/BehaviorDesigner/documentation.php?id=99")]
    [TaskCategory("UFPS")]
    [TaskIcon("Assets/Behavior Designer/Third Party/UltimateFPS/Editor/Icon.png")]
    public class AddItem : Action
    {
        [Tooltip("The name of the item to add")]
        public SharedString itemName;
        [Tooltip("The number of items to add")]
        public SharedInt itemCount;

        private vp_PlayerEventHandler agent;

        public override void OnAwake()
        {
            agent = gameObject.GetComponent<vp_PlayerEventHandler>();
        }

        public override TaskStatus OnUpdate()
        {
            return agent.AddItem.Try(new object[] { itemName.Value, itemCount.Value }) ? TaskStatus.Success : TaskStatus.Failure;
        }

        public override void OnReset()
        {
            itemName = "";
            itemCount = 0;
        }
    }
}