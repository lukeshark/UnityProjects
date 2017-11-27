using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

namespace BehaviorDesigner.Samples
{
    public class SetDamagableTransform : Action
    {
        public SharedDamageHandler damageHandler;
        public SharedTransform targetTransform;

        public override TaskStatus OnUpdate()
        {
            targetTransform.Value = damageHandler.Value.GetComponent<Transform>();
            return TaskStatus.Success;
        }
    }
}