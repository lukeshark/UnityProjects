using UnityEngine;
using DG.Tweening;

namespace BehaviorDesigner.Runtime.Tasks.DOTween
{
    [TaskCategory("DOTween")]
    [TaskDescription("Tween the GameObject's local y position.")]
    [TaskIcon("Assets/Behavior Designer/Third Party/DOTween/Editor/Icon.png")]
    [HelpURL("http://www.opsive.com/assets/Behavior Designer/documentation.php?id=114")]
    public class DOLocalMoveY : Action
    {
        [Tooltip("The GameObject to tween")]
        public SharedGameObject targetGameObject;
        [Tooltip("The target tween value")]
        public SharedFloat to;
        [Tooltip("The time the tween takes to complete")]
        public SharedFloat time;
        [Tooltip("Should the tween smoothly snap all values to integters?")]
        public SharedBool snap;
        [SharedRequired]
        [Tooltip("The stored tweener")]
        public SharedTweener storeTweener;

        private bool complete;

        public override void OnStart()
        {
            var target = GetDefaultGameObject(targetGameObject.Value).transform;
            storeTweener.Value = target.DOLocalMoveY(to.Value, time.Value, snap.Value);
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
            to = 0;
            time = 0;
            snap = false;
            storeTweener = null;
        }
    }
}