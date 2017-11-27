using UnityEngine;
using DG.Tweening;

namespace BehaviorDesigner.Runtime.Tasks.DOTween
{
    [TaskCategory("DOTween")]
    [TaskDescription("Tween the GameObject's LookAt position.")]
    [TaskIcon("Assets/Behavior Designer/Third Party/DOTween/Editor/Icon.png")]
    [HelpURL("http://www.opsive.com/assets/Behavior Designer/documentation.php?id=114")]
    public class DOLookAt : Action
    {
        [Tooltip("The GameObject to tween")]
        public SharedGameObject targetGameObject;
        [Tooltip("The target tween value")]
        public SharedVector3 to;
        [Tooltip("The time the tween takes to complete")]
        public SharedFloat time;
        [Tooltip("Are any of the axes constrained?")]
        public AxisConstraint axisConstraint = AxisConstraint.None;
        [Tooltip("The up direction")]
        public SharedVector3 up = Vector3.up;
        [SharedRequired]
        [Tooltip("The stored tweener")]
        public SharedTweener storeTweener;

        private bool complete;

        public override void OnStart()
        {
            var target = GetDefaultGameObject(targetGameObject.Value).transform;
            storeTweener.Value = target.DOLookAt(to.Value, time.Value, axisConstraint, up.Value);
            storeTweener.Value.OnComplete(() => complete = true);
        }

        public override TaskStatus OnUpdate()
        {
            return complete ? TaskStatus.Success : TaskStatus.Running;
        }

        public override void OnEnd()
        {
            complete = false;
        }

        public override void OnReset()
        {
            to = Vector3.zero;
            time = 0;
            axisConstraint = AxisConstraint.None;
            up = Vector3.up;
            storeTweener = null;
        }
    }
}