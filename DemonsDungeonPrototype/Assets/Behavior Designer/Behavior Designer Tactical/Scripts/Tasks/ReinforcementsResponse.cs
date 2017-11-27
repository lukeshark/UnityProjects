using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
using Tooltip = BehaviorDesigner.Runtime.Tasks.TooltipAttribute;
using HelpURL = BehaviorDesigner.Runtime.Tasks.HelpURLAttribute;

namespace BehaviorDesigner.Runtime.Tactical.Tasks
{
    [TaskCategory("Tactical")]
    [TaskDescription("Responds to a reinforcement request. Will move towards the requesting agent and start attacking as soon as the target is within distance")]
    [HelpURL("http://www.opsive.com/assets/BehaviorDesigner/Tactical/documentation.php?id=13")]
    [TaskIcon("Assets/Behavior Designer Tactical/Editor/Icons/{SkinColor}ReinforcementsResponseIcon.png")]
    public class ReinforcementsResponse : NavMeshTacticalGroup
    {
        [Tooltip("A list of agents that may call for reinforcements")]
        public SharedGameObjectList listenForReinforcements;

        private Transform requestTransform; 

        public override void OnAwake()
        {
            base.OnAwake();

            // Listen to any behavior trees that could request reinforcements.
            for (int i = 0; i < listenForReinforcements.Value.Count; ++i) {
                var behaviorTrees = listenForReinforcements.Value[i].GetComponents<BehaviorTree>();
                for (int j = 0; j < behaviorTrees.Length; ++j) {
                    behaviorTrees[j].RegisterEvent<GameObject>("RequestReinforcements", OnReinforcementsRequest);
                }
            }
        }

        public override TaskStatus OnUpdate()
        {
            var baseStatus = base.OnUpdate();
            if (baseStatus != TaskStatus.Running || !started) {
                return baseStatus;
            }

            if (requestTransform != null) {
                // Start attacking as soon as the agent has arrived close to the reinforcement position.
                if (tacticalAgent.AttackPosition || Vector3.Distance(transform.position, requestTransform.position) <= tacticalAgent.AttackAgent.AttackDistance()) {
                    FindAttackTarget();
                    if (MoveToAttackPosition()) {
                        tacticalAgent.TryAttack();
                    }
                } else {
                    // Move to the agent requesting reinforcements.
                    tacticalAgent.SetDestination(requestTransform.position);
                }
            }
            return TaskStatus.Running;
        }

        private void OnReinforcementsRequest(GameObject requestGameObject)
        {
            requestTransform = requestGameObject.transform;
        }

        public override void OnReset()
        {
            base.OnReset();

            listenForReinforcements.Value.Clear();
        }
    }
}