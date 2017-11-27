using UnityEngine;
using DG.Tweening;

namespace BehaviorDesigner.Runtime.Tasks.DOTween
{
    [TaskCategory("DOTween")]
    [TaskDescription("Pauses or plays a tween or all tweens if no tween is specified.")]
    [TaskIcon("Assets/Behavior Designer/Third Party/DOTween/Editor/Icon.png")]
    [HelpURL("http://www.opsive.com/assets/Behavior Designer/documentation.php?id=114")]
    public class TogglePause : Action
    {
        [RequiredField]
        [Tooltip("The optional tweener to pause/play. If null all tweens will be paused/played")]
        public SharedTweener targetTweener;

        public override TaskStatus OnUpdate()
        {
            if (targetTweener != null && targetTweener.Value != null) {
                targetTweener.Value.TogglePause();
            } else {
                DG.Tweening.DOTween.TogglePause(targetTweener.Value);
            }
            return TaskStatus.Success;
        }
        public override void OnReset()
        {
            targetTweener = null;
        }
    }
}