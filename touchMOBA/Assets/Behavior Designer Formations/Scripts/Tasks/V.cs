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
    [TaskDescription("Arrange the group in a V shape where the leader is in the back.")]
    [TaskIcon("Assets/Behavior Designer Formations/Editor/Icons/{SkinColor}VIcon.png")]
    [HelpURL("http://www.opsive.com/assets/BehaviorDesigner/Formations/documentation.php?id=7")]
    public class V : GroupFormation
    {
        [Tooltip("The separation between agents")]
        public SharedVector2 separation = new Vector2(2, 2);

        protected override Vector3 TargetPosition(int index, float zLookAhead)
        {
            // Alternate between the left and right sides of the v
            return transforms[0].TransformPoint(separation.Value.x * (index % 2 == 0 ? -1 : 1) * (((index - 1) / 2) + 1), 0, separation.Value.y * (((index - 1) / 2) + 1) + zLookAhead);
        }

        public override void OnReset()
        {
            base.OnReset();

            separation = new Vector2(2, 2);
        }
    }
}