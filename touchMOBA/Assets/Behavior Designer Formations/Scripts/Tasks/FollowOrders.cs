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
    [TaskDescription("Tells the leader that the current agent is ready to follow its orders")]
    [TaskIcon("Assets/Behavior Designer Formations/Editor/Icons/{SkinColor}FollowOrdersIcon.png")]
    [HelpURL("http://www.opsive.com/assets/BehaviorDesigner/Formations/documentation.php?id=15")]
    public class FollowOrders : Action
    {
        [Tooltip("The leader to follow")]
        public SharedGameObject leader;

        private BehaviorTree[] leaderTrees;
        private bool sendListenerEvent;
        private TaskStatus runStatus;

        public override void OnAwake()
        {
            leaderTrees = leader.Value.GetComponents<BehaviorTree>();
            if (leaderTrees.Length == 0) {
                Debug.LogError("Error: The leader doesn't have a behavior tree component.");
            }
        }

        public override void OnStart()
        {
            if (leaderTrees == null || leaderTrees.Length == 0) {
                runStatus = TaskStatus.Failure;
            } else {
                Owner.RegisterEvent<TaskStatus>("OrdersFinished", OrdersFinished);
                runStatus = TaskStatus.Running;
                sendListenerEvent = true;
            }
        }

        public override TaskStatus OnUpdate()
        {
            // Send within OnUpdate to ensure the at least one leader behavior tree is active. If registered within OnStart there is a chance that the behavior tree
            // isn't active yet and will never receive the event.
            if (sendListenerEvent) {
                // Listen to orders from any of the behavior trees on the leader
                for (int i = 0; i < leaderTrees.Length; ++i) {
                    if (leaderTrees[i].ExecutionStatus == TaskStatus.Running) {
                        leaderTrees[i].SendEvent<GameObject>("StartListeningForOrders", gameObject);
                        sendListenerEvent = false;
                    }
                }
            }
            return runStatus;
        }

        private void OrdersFinished(TaskStatus status)
        {
            runStatus = status;
            for (int i = 0; i < leaderTrees.Length; ++i) {
                leaderTrees[i].SendEvent<GameObject>("StopListeningToOrders", gameObject);
            }
            Owner.UnregisterEvent<TaskStatus>("OrdersFinished", OrdersFinished);
        }

        public override void OnEnd()
        {
            for (int i = 0; i < leaderTrees.Length; ++i) {
                leaderTrees[i].SendEvent<GameObject>("StopListeningToOrders", gameObject);
            }
            Owner.UnregisterEvent<TaskStatus>("OrdersFinished", OrdersFinished);
        }

        public override void OnReset()
        {
            leader = null;
        }
    }
}