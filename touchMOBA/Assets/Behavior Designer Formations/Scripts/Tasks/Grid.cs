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
    [TaskDescription("Arrange the group in a grid where the number of rows is equal to the number of columns.")]
    [TaskIcon("Assets/Behavior Designer Formations/Editor/Icons/{SkinColor}GridIcon.png")]
    [HelpURL("http://www.opsive.com/assets/BehaviorDesigner/Formations/documentation.php?id=3")]
    public class Grid : GroupFormation
    {
        [Tooltip("The separation between agents")]
        public SharedVector2 separation = new Vector2(2, 2);

        private int rows;

        protected override void AddAgentToGroup(Transform agent)
        {
            base.AddAgentToGroup(agent);

            // Form an upper bounds of points on the grid
            rows = Mathf.CeilToInt(Mathf.Sqrt(agents.Count));
        }

        protected override int RemoveAgentFromGroup(Transform agent)
        {
            var index = base.RemoveAgentFromGroup(agent);

            // Form an upper bounds of points on the grid
            rows = Mathf.CeilToInt(Mathf.Sqrt(agents.Count));

            return index;
        }

        protected override Vector3 TargetPosition(int index, float zLookAhead)
        {
            var row = index % rows;
            var column = index / rows;
            return transforms[0].TransformPoint(separation.Value.x * column, 0, -separation.Value.y * row + zLookAhead);
        }

        public override void OnReset()
        {
            base.OnReset();

            separation = new Vector2(2, 2);
        }
    }
}