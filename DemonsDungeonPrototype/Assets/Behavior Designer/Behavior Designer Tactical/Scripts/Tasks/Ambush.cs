using UnityEngine;
using System.Collections.Generic;
using BehaviorDesigner.Runtime.Tasks;
using Tooltip = BehaviorDesigner.Runtime.Tasks.TooltipAttribute;
using HelpURL = BehaviorDesigner.Runtime.Tasks.HelpURLAttribute;

namespace BehaviorDesigner.Runtime.Tactical.Tasks
{
    [TaskCategory("Tactical")]
    [TaskDescription("Wait for the group of targets to pass before attacking")]
    [HelpURL("http://www.opsive.com/assets/BehaviorDesigner/Tactical/documentation.php?id=5")]
    [TaskIcon("Assets/Behavior Designer Tactical/Editor/Icons/{SkinColor}AmbushIcon.png")]
    public class Ambush : NavMeshTacticalGroup
    {
        [Tooltip("The number of seconds to wait after the enemies have passed before the agents start attacking")]
        public SharedFloat attackDelay;
        [Tooltip("The minimum distance that the agents can attack")]
        public SharedFloat minAmbushDistance = 10;

        private float attackTime;
        private float targetDistance = float.MaxValue;

        public override void OnStart()
        {
            base.OnStart();

            attackTime = -1;
        }

        public override TaskStatus OnUpdate()
        {
            var baseStatus = base.OnUpdate();
            if (baseStatus != TaskStatus.Running || !started) {
                return baseStatus;
            }

            // Attack!
            if (canAttack && MoveToAttackPosition()) {
                tacticalAgent.TryAttack();
            } else {
                // Wait to attack until the enemies have passed the agents. Only the leader needs to perform this check.
                if (leader.Value == null) {
                    if (attackTime == -1) {
                        var center = CenterAttackPosition();
                        var distance = (center - transform.position).magnitude;
                        if (distance > targetDistance && (minAmbushDistance.Value == 0 || distance < minAmbushDistance.Value)) {
                            // The enemies have passed. Set the attackTime so the agents will start attacking.
                            attackTime = Time.time + attackDelay.Value;
                        } else {
                            targetDistance = distance;
                        }
                        return TaskStatus.Running;
                    } else if (attackTime < Time.time) {
                        SendAttackEvent(true);
                    }
                }
            }

            return TaskStatus.Running;
        }

        public override void OnEnd()
        {
            base.OnEnd();

            targetDistance = float.MaxValue;
        }

        public override void OnReset()
        {
            base.OnReset();

            attackDelay = 0;
            minAmbushDistance = 10;
        }
   } 
}