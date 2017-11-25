using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

namespace BehaviorDesigner.Samples
{
    public class MoveTowards : Action
    {
        // The speed of the object
        public float speed = 0;
        // The transform that the object is moving towards
        public SharedTransform target;

        public override TaskStatus OnUpdate()
        {
            // Return a task status of success once we've reached the target
            if (target.Value == null || Vector3.SqrMagnitude(transform.position - target.Value.position) < 0.1f) {
                return TaskStatus.Success;
            }
            // We haven't reached the target yet so keep moving towards it
            transform.position = Vector3.MoveTowards(transform.position, target.Value.position, speed * Time.deltaTime);
            return TaskStatus.Running;
        }
    }
}