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
    [TaskDescription("Arrange the group in a tactical diamond shape.")]
    [TaskIcon("Assets/Behavior Designer Formations/Editor/Icons/{SkinColor}DiamondIcon.png")]
    [HelpURL("http://www.opsive.com/assets/BehaviorDesigner/Formations/documentation.php?id=11")]
    public class Diamond : GroupFormation
    {
        [Tooltip("The separation between agents")]
        public SharedVector2 separation = new Vector2(2, 2);
        [Tooltip("Should the back agents have a left and right offset?")]
        public SharedBool backPositionOffset = false;

        protected override Vector3 TargetPosition(int index, float zLookAhead)
        {
            Vector3 targetPos;
            if (index < 3) { // form the diamond part
                targetPos = transforms[0].TransformPoint(separation.Value.x * (index % 2 == 0 ? -1 : 1), 0, -separation.Value.y + zLookAhead);
            } else { // form the back of the diamond. This is a tactical diamond so it is made for agents to cover themselves down hallways.
                if (backPositionOffset.Value) {
                    targetPos = transforms[0].TransformPoint(separation.Value.x * (index % 2 == 0 ? -0.5f : 0.5f), 0, -separation.Value.y * (((index - 1) / 2) + 1) + zLookAhead);
                } else {
                    targetPos = transforms[0].TransformPoint(0, 0, -separation.Value.y * (index - 1) + zLookAhead);
                }
            }
            return targetPos;
        }

        public override void OnReset()
        {
            base.OnReset();

            separation = new Vector2(2, 2);
            backPositionOffset = false;
        }
    }
}