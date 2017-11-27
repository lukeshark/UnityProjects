using UnityEngine;
using DG.Tweening;

namespace BehaviorDesigner.Runtime.Tasks.DOTween
{
    [TaskCategory("DOTween")]
    [TaskDescription("Tween any particular float value.")]
    [TaskIcon("Assets/Behavior Designer/Third Party/DOTween/Editor/Icon.png")]
    [HelpURL("http://www.opsive.com/assets/Behavior Designer/documentation.php?id=114")]
    public class FloatTo : Action
    {
        [Tooltip("The original tween value")]
        public SharedFloat from;
        [Tooltip("The target tween value")]
        public SharedFloat to;
        [Tooltip("The time the tween takes to complete")]
        public SharedFloat time;
        [SharedRequired]
        [Tooltip("The stored tweener")]
        public SharedTweener storeTweener;

        private bool complete;

        public override void OnStart()
        {
            storeTweener.Value = DG.Tweening.DOTween.To(() => from.Value, x => from.Value = x, to.Value, time.Value);
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
            from = 0;
            to = 0;
            time = 0;
            storeTweener = null;
        }
    }
}