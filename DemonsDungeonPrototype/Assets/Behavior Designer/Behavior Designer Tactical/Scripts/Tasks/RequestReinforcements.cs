using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
using Tooltip = BehaviorDesigner.Runtime.Tasks.TooltipAttribute;
using HelpURL = BehaviorDesigner.Runtime.Tasks.HelpURLAttribute;

namespace BehaviorDesigner.Runtime.Tactical.Tasks
{
    [TaskCategory("Tactical")]
    [TaskDescription("Requests reinforcements")]
    [HelpURL("http://www.opsive.com/assets/BehaviorDesigner/Tactical/documentation.php?id=12")]
    [TaskIcon("Assets/Behavior Designer Tactical/Editor/Icons/{SkinColor}RequestReinforcementsIcon.png")]
    public class RequestReinforcements : Action
    {
        public override TaskStatus OnUpdate()
        {
            Owner.SendEvent<GameObject>("RequestReinforcements", gameObject);

            return TaskStatus.Success;
        }
    }
}