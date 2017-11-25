using UnityEngine;
using DG.Tweening;

namespace BehaviorDesigner.Runtime.Tasks.DOTween
{
    [TaskCategory("DOTween")]
    [TaskDescription("Initializes DOTween.")]
    [TaskIcon("Assets/Behavior Designer/Third Party/DOTween/Editor/Icon.png")]
    [HelpURL("http://www.opsive.com/assets/Behavior Designer/documentation.php?id=114")]
    public class Init : Action
    {
        [Tooltip("Should DOTween recycle all objects by default?")]
        public SharedBool recycleAllByDefault = false;
        [Tooltip("Should DOTween use safe mode?")]
        public SharedBool useSafeMode = true;
        [Tooltip("The level of DOTween logging")]
        public LogBehaviour logBehavior = LogBehaviour.ErrorsOnly;

        public override TaskStatus OnUpdate()
        {
            DG.Tweening.DOTween.Init(recycleAllByDefault.Value, useSafeMode.Value, logBehavior);
            return TaskStatus.Success;
        }

        public override void OnReset()
        {
            recycleAllByDefault = false;
            useSafeMode = true;
            logBehavior = LogBehaviour.ErrorsOnly;
        }
    }
}