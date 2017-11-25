using UnityEngine;
using DG.Tweening;

namespace BehaviorDesigner.Runtime.Tasks.DOTween
{
    [TaskCategory("DOTween")]
    [TaskDescription("Tween the GameObject's local rotation.")]
    [TaskIcon("Assets/Behavior Designer/Third Party/DOTween/Editor/Icon.png")]
    [HelpURL("http://www.opsive.com/assets/Behavior Designer/documentation.php?id=114")]
    public class DOLocalRotate : Action
    {
        [Tooltip("The GameObject to tween")]
        public SharedGameObject targetGameObject;
        [Tooltip("The target tween value")]
        public SharedVector3 to;
        [Tooltip("The time the tween takes to complete")]
        public SharedFloat time;
        [Tooltip("The DOTween rotate mode")]
        public RotateMode rotateMode = RotateMode.Fast;
        [SharedRequired]
        [Tooltip("The stored tweener")]
        public SharedTweener storeTweener;

        private bool complete;

        public override void OnStart()
        {
            var target = GetDefaultGameObject(targetGameObject.Value).transform;
            storeTweener.Value = target.DOLocalRotate(to.Value, time.Value, rotateMode);
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
            rotateMode = RotateMode.Fast;
            storeTweener = null;
        }
    }
}