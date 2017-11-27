using UnityEngine;
using DG.Tweening;

namespace BehaviorDesigner.Runtime.Tasks.DOTween
{
    [TaskCategory("DOTween")]
    [TaskDescription("Sets the delay of the tween.")]
    [TaskIcon("Assets/Behavior Designer/Third Party/DOTween/Editor/Icon.png")]
    [HelpURL("http://www.opsive.com/assets/Behavior Designer/documentation.php?id=114")]
    public class SetDelay : Action
    {
        [RequiredField]
        [Tooltip("The tweener to set the ease of")]
        public SharedTweener targetTweener;
        [Tooltip("The delay amount")]
        public SharedFloat delay;

        public override TaskStatus OnUpdate()
        {
            targetTweener.Value.SetDelay(delay.Value);
            return TaskStatus.Success;
        }
        public override void OnReset()
        {
            targetTweener = null;
            delay = 0;
        }
    }
}