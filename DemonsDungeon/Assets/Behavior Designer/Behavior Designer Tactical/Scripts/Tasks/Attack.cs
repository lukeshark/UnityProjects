using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
using Tooltip = BehaviorDesigner.Runtime.Tasks.TooltipAttribute;
using HelpURL = BehaviorDesigner.Runtime.Tasks.HelpURLAttribute;

namespace BehaviorDesigner.Runtime.Tactical.Tasks
{
    [TaskCategory("Tactical")]
    [TaskDescription("Moves to the closest target and starts attacking as soon as the agent is within distance")]
    [HelpURL("http://www.opsive.com/assets/BehaviorDesigner/Tactical/documentation.php?id=1")]
    [TaskIcon("Assets/Behavior Designer Tactical/Editor/Icons/{SkinColor}AttackIcon.png")]
    public class Attack : NavMeshTacticalGroup
    {
        public override TaskStatus OnUpdate()
        {
            var baseStatus = base.OnUpdate();
            if (baseStatus != TaskStatus.Running || !started) {
                return baseStatus;
            }

            if (MoveToAttackPosition()) {
                tacticalAgent.TryAttack();
            }

            return TaskStatus.Running;
        }
    }
}