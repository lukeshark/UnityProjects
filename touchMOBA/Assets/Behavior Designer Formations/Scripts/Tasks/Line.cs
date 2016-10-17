using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
#if !(UNITY_4_3 || UNITY_4_4)
using Tooltip = BehaviorDesigner.Runtime.Tasks.TooltipAttribute;
#if !(UNITY_4_5 || UNITY_4_6 || UNITY_5_0)
using HelpURL = BehaviorDesigner.Runtime.Tasks.HelpURLAttribute;
#endif
#endif

namespace BehaviorDesigner.Runtime.Formations.Tasks
{
    [TaskCategory("Formations")]
    [TaskDescription("Arrange the group in a straight horizontal line.")]
    [TaskIcon("Assets/Behavior Designer Formations/Editor/Icons/{SkinColor}LineIcon.png")]
    [HelpURL("http://www.opsive.com/assets/BehaviorDesigner/Formations/documentation.php?id=4")]
    public class Line : GroupFormation
    {
        [Tooltip("The separation between agents")]
        public SharedFloat separation = 2;
        [Tooltip("Should the formation be to the right of the leader?")]
        public SharedBool right;

        protected override Vector3 TargetPosition(int index, float zLookAhead)
        {
            return transforms[0].TransformPoint(separation.Value * index * (right.Value ? 1 : -1), 0, zLookAhead);
        }

        public override void OnReset()
        {
            base.OnReset();

            separation = 2;
            right = false;
        }
    }
}