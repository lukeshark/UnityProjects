using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

namespace BehaviorDesigner.Runtime.Tasks.UFPS
{
    [TaskDescription("Returns success if the agent can itneract otherwise failure.")]
    [HelpURL("http://www.opsive.com/assets/BehaviorDesigner/documentation.php?id=99")]
    [TaskCategory("UFPS")]
    [TaskIcon("Assets/Behavior Designer/Third Party/UltimateFPS/Editor/Icon.png")]
    public class CanInteract : Conditional
    {
        public vp_FPPlayerEventHandler agent;

        public override void OnAwake()
        {
            if (agent == null) {
                agent = gameObject.GetComponent<vp_FPPlayerEventHandler>();
            }
        }

        public override TaskStatus OnUpdate()
        {
            if (agent == null) {
                return TaskStatus.Failure;
            }
            return agent.CanInteract.Get() ? TaskStatus.Success : TaskStatus.Failure;
        }

        public override void OnReset()
        {
            agent = null;
        }
    }
}