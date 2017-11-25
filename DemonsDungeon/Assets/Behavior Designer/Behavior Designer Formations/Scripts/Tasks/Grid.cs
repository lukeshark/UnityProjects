using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
using Tooltip = BehaviorDesigner.Runtime.Tasks.TooltipAttribute;
using HelpURL = BehaviorDesigner.Runtime.Tasks.HelpURLAttribute;

namespace BehaviorDesigner.Runtime.Formations.Tasks
{
    [TaskCategory("Formations")]
    [TaskDescription("Arrange the group in a grid where the number of rows is equal to the number of columns.")]
    [TaskIcon("Assets/Behavior Designer Formations/Editor/Icons/{SkinColor}GridIcon.png")]
    [HelpURL("http://www.opsive.com/assets/BehaviorDesigner/Formations/documentation.php?id=3")]
    public class Grid : NavMeshFormationGroup
    {
        [Tooltip("The separation between agents")]
        public SharedVector2 separation = new Vector2(2, 2);

        private int rows;

        protected override void AddAgentToGroup(Behavior agent, int index)
        {
            base.AddAgentToGroup(agent, index);

            // Form an upper bounds of points on the grid
            rows = Mathf.CeilToInt(Mathf.Sqrt(agents.Count));
        }

        protected override int RemoveAgentFromGroup(Behavior agent)
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
            var leaderTransform = leader.Value == null ? transform : leader.Value.transform;
            return leaderTransform.TransformPoint(separation.Value.x * column, 0, -separation.Value.y * row + zLookAhead);
        }

        public override void OnReset()
        {
            base.OnReset();

            separation = new Vector2(2, 2);
        }
    }
}