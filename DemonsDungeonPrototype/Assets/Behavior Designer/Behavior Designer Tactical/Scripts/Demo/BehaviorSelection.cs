using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace BehaviorDesigner.Runtime.Tactical
{
    public class BehaviorSelection : MonoBehaviour
    {
        public GameObject agentGroup;
        public GameObject enemyGroup;
        public GameObject defendObject;
        public GUISkin descriptionGUISkin;

        private Dictionary<int, List<BehaviorTree>> agentBehaviorTreeGroup = new Dictionary<int, List<BehaviorTree>>();
        private Dictionary<int, List<BehaviorTree>> enemyBehaviorTreeGroup = new Dictionary<int, List<BehaviorTree>>();
        private Health[] enemyHealth;

        private enum BehaviorSelectionType { Attack, Charge, MarchingFire, Flank, Ambush, ShootAndScoot, Leapfrog, Surround, Defend, Hold, Retreat, Reinforcements, Last }
        private BehaviorSelectionType selectionType = BehaviorSelectionType.Attack;
        private BehaviorSelectionType prevSelectionType = BehaviorSelectionType.Attack;

        public void Start()
        {
            for (int i = 0; i < agentGroup.transform.childCount; ++i) {
                var child = agentGroup.transform.GetChild(i);
                var agentTrees = child.GetComponents<BehaviorTree>();
                for (int j = 0; j < agentTrees.Length; ++j) {
                    var group = agentTrees[j].Group;
                    List<BehaviorTree> groupBehaviorTrees;
                    if (!agentBehaviorTreeGroup.TryGetValue(group, out groupBehaviorTrees)) {
                        groupBehaviorTrees = new List<BehaviorTree>();
                        agentBehaviorTreeGroup.Add(group, groupBehaviorTrees);
                    }
                    groupBehaviorTrees.Add(agentTrees[j]);
                }
            }
            enemyHealth = enemyGroup.GetComponentsInChildren<Health>();
            var behaviorTrees = enemyGroup.GetComponentsInChildren<BehaviorTree>();
            for (int i = 0; i < behaviorTrees.Length; ++i) {
                List<BehaviorTree> list;
                if (enemyBehaviorTreeGroup.TryGetValue(behaviorTrees[i].Group, out list)) {
                    list.Add(behaviorTrees[i]);
                } else {
                    list = new List<BehaviorTree>();
                    list.Add(behaviorTrees[i]);
                    enemyBehaviorTreeGroup[behaviorTrees[i].Group] = list;
                }
            }

            SelectionChanged();
        }

        public void OnGUI()
        {
            GUILayout.BeginVertical(GUILayout.Width(300));
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("<-")) {
                prevSelectionType = selectionType;
                selectionType = (BehaviorSelectionType)(((int)selectionType - 1) % (int)BehaviorSelectionType.Last);
                if ((int)selectionType < 0) selectionType = BehaviorSelectionType.Reinforcements;
                SelectionChanged();
            }
            GUILayout.Box(SplitCamelCase(selectionType.ToString()), GUILayout.Width(220));
            if (GUILayout.Button("->")) {
                prevSelectionType = selectionType;
                selectionType = (BehaviorSelectionType)(((int)selectionType + 1) % (int)BehaviorSelectionType.Last);
                SelectionChanged();
            }
            GUILayout.EndHorizontal();
            GUILayout.Box(Description(), descriptionGUISkin.box);
            GUILayout.EndVertical();
        }

        private string Description()
        {
            string desc = "";
            switch (selectionType) {
                case BehaviorSelectionType.Attack:
                    desc = "Moves to the closest target and starts attacking as soon as the agent is within distance.";
                    break;
                case BehaviorSelectionType.Charge:
                    desc = "Charges towards the target. The agents will start attacking when they are done charging.";
                    break;
                case BehaviorSelectionType.MarchingFire:
                    desc = "Move towards the target. The agents will start attacking when they are within distance.";
                    break;
                case BehaviorSelectionType.Flank:
                    desc = "Flanks the target from the left and right.";
                    break;
                case BehaviorSelectionType.Ambush:
                    desc = "Wait for the group of targets to pass before attacking.";
                    break;
                case BehaviorSelectionType.ShootAndScoot:
                    desc = "Attacks the target and moves position after a short amount of time.";
                    break;
                case BehaviorSelectionType.Leapfrog:
                    desc = "Search for the target by forming two groups and leapfrogging each other. Both groups will start attacking as soon as the target is within sight";
                    break;
                case BehaviorSelectionType.Surround:
                    desc = "Surrounds the enemy and starts to attack after all agents are in position";
                    break;
                case BehaviorSelectionType.Retreat:
                    desc = "Retreats in the opposite direction of the target";
                    break;
                case BehaviorSelectionType.Defend:
                    desc = "Defends the object within a defend radius. Will seek and attack a target within a specified radius";
                    break;
                case BehaviorSelectionType.Hold:
                    desc = "Defends the object within a defend radius. Will seek and attack a target for as long as it takes";
                    break;
                case BehaviorSelectionType.Reinforcements:
                    desc = "The attacking agent will request for reinforcements after waiting a moment. " + 
                           "The reinforcement agents will move towards the requesting agent and start attacking the targets when within distnace.";
                    break;
            }
            return desc;
        }

        private void SelectionChanged()
        {
            StopCoroutine("EnableBehavior");
            for (int i = 0; i < agentBehaviorTreeGroup[(int)prevSelectionType].Count; ++i) {
                agentBehaviorTreeGroup[(int)prevSelectionType][i].DisableBehavior();
            }

            if (enemyBehaviorTreeGroup.ContainsKey((int)prevSelectionType)) {
                var trees = enemyBehaviorTreeGroup[(int)prevSelectionType];
                for (int i = 0; i < trees.Count; ++i) {
                    trees[i].DisableBehavior();
                }
            }

            StartCoroutine("EnableBehavior");
        }

        private static string SplitCamelCase(string s)
        {
            var r = new Regex(@"(?<=[A-Z])(?=[A-Z][a-z])|(?<=[^A-Z])(?=[A-Z])|(?<=[A-Za-z])(?=[^A-Za-z])", RegexOptions.IgnorePatternWhitespace);
            s = r.Replace(s, " ");
            return (char.ToUpper(s[0]) + s.Substring(1)).Trim();
        }

        private IEnumerator EnableBehavior()
        {
            defendObject.SetActive(false);
            switch (selectionType) {
                case BehaviorSelectionType.Attack:
                case BehaviorSelectionType.Charge:
                case BehaviorSelectionType.MarchingFire:
                case BehaviorSelectionType.Flank:
                    SetPosRot(new Vector3(-16.85f, 0, 38.9f), new Vector3(0, 0, 0), new Vector3(-16.9f, 0f, 58.75f), new Vector3(0, 180, 0), new Vector3(-12.8f, 36.75f, 43.35f));
                    SetChildPosRot(new Vector3[] { new Vector3(0, 0, -4.25f), new Vector3(0.55f, 0, -5.55f), new Vector3(-4.1f, 0, -3.88f), new Vector3(2.5f, 0, -5.7f), new Vector3(-3.6f, 0, -4.75f)},
                                   new Vector3[] { new Vector3(0, 0, 0), new Vector3(0, 0, 0), new Vector3(0, 0, 0), new Vector3(0, 0, 0), new Vector3(0, 0, 0) },
                                   new Vector3[] { new Vector3(-1.65f, 0, 0), new Vector3(0, 0, 0) },
                                   new Vector3[] { new Vector3(0, 0, 0), new Vector3(0, 0, 0) });
                    break;
                case BehaviorSelectionType.Ambush:
                    SetPosRot(new Vector3(33.75f, 0, -13.05f), new Vector3(0, 270, 0), new Vector3(29.9f, 0, -4.5f), new Vector3(0, 180, 0), new Vector3(30, 36.75f, -15f));
                    SetChildPosRot(new Vector3[] { new Vector3(0, 0, -4.25f), new Vector3(0.55f, 0, -5.55f), new Vector3(-1.75f, 0, -7.66f), new Vector3(2.5f, 0, -5.7f), new Vector3(-1.1f, 0, -5.6f) },
                                   new Vector3[] { new Vector3(0, 0, 0), new Vector3(0, 0, 0), new Vector3(0, 0, 0), new Vector3(0, 0, 0), new Vector3(0, 0, 0) },
                                   new Vector3[] { new Vector3(-1.65f, 0, 0), new Vector3(0, 0, 0) },
                                   new Vector3[] { new Vector3(0, 0, 0), new Vector3(0, 0, 0) });
                    break;
                case BehaviorSelectionType.ShootAndScoot:
                    SetPosRot(new Vector3(-20.25f, 0, -16.87f), new Vector3(0, 0, 0), new Vector3(-16.9f, 0, -4.5f), new Vector3(0, 180, 0), new Vector3(-21, 36.75f, -12.5f));
                    SetChildPosRot(new Vector3[] { new Vector3(0, 0, -4.25f), new Vector3(0.55f, 0, -5.55f), new Vector3(-4.1f, 0, -3.88f), new Vector3(2.5f, 0, -5.7f), new Vector3(-3.6f, 0, -4.75f)},
                                   new Vector3[] { new Vector3(0, 0, 0), new Vector3(0, 0, 0), new Vector3(0, 0, 0), new Vector3(0, 0, 0), new Vector3(0, 0, 0) },
                                   new Vector3[] { new Vector3(-1.65f, 0, 0), new Vector3(0, 0, 0) },
                                   new Vector3[] { new Vector3(0, 0, 0), new Vector3(0, 0, 0) });
                    break;
                case BehaviorSelectionType.Leapfrog:
                    SetPosRot(new Vector3(-20.25f, 0, -30.87f), new Vector3(0, 0, 0), new Vector3(-16.9f, 0, -4.5f), new Vector3(0, 180, 0), new Vector3(-21, 36.75f, -22.5f));
                    SetChildPosRot(new Vector3[] { new Vector3(0, 0, -4.25f), new Vector3(0.55f, 0, -5.55f), new Vector3(-4.1f, 0, -3.88f), new Vector3(2.5f, 0, -5.7f), new Vector3(-3.6f, 0, -4.75f) },
                                   new Vector3[] { new Vector3(0, 0, 0), new Vector3(0, 0, 0), new Vector3(0, 0, 0), new Vector3(0, 0, 0), new Vector3(0, 0, 0) },
                                   new Vector3[] { new Vector3(-1.65f, 0, 0), new Vector3(0, 0, 0) },
                                   new Vector3[] { new Vector3(0, 0, 0), new Vector3(0, 0, 0) });
                    break;
                case BehaviorSelectionType.Surround:
                    SetPosRot(new Vector3(-10, 0, -18.5f), new Vector3(0, 0, 0), new Vector3(-10.9f, 0, -7.25f), new Vector3(0, 180, 0), new Vector3(-10.9f, 36.75f, -7.25f));
                    SetChildPosRot(new Vector3[] { new Vector3(0, 0, -4.25f), new Vector3(0.55f, 0, -5.55f), new Vector3(-4.1f, 0, -3.88f), new Vector3(2.5f, 0, -5.7f), new Vector3(-3.6f, 0, -4.75f) },
                                   new Vector3[] { new Vector3(0, 0, 0), new Vector3(0, 0, 0), new Vector3(0, 0, 0), new Vector3(0, 0, 0), new Vector3(0, 0, 0) },
                                   new Vector3[] { new Vector3(-1.65f, 0, 0), new Vector3(0, 0, 0) },
                                   new Vector3[] { new Vector3(0, 0, 0), new Vector3(0, 0, 0) });
                    break;
                case BehaviorSelectionType.Retreat:
                    SetPosRot(new Vector3(-16.85f, 0, -2.7f), new Vector3(0, 0, 0), new Vector3(-16.9f, 0, -4.5f), new Vector3(0, 180, 0), new Vector3(-21, 36.75f, -22.5f));
                    SetChildPosRot(new Vector3[] { new Vector3(0, 0, -4.25f), new Vector3(0.55f, 0, -5.55f), new Vector3(-4.1f, 0, -3.88f), new Vector3(2.5f, 0, -5.7f), new Vector3(-3.6f, 0, -4.75f) },
                                   new Vector3[] { new Vector3(0, 0, 0), new Vector3(0, 0, 0), new Vector3(0, 0, 0), new Vector3(0, 0, 0), new Vector3(0, 0, 0) },
                                   new Vector3[] { new Vector3(-1.65f, 0, 0), new Vector3(0, 0, 0) },
                                   new Vector3[] { new Vector3(0, 0, 0), new Vector3(0, 0, 0) });
                    break;
                case BehaviorSelectionType.Defend:
                case BehaviorSelectionType.Hold:
                    defendObject.SetActive(true);
                    SetPosRot(new Vector3(-16.85f, 0, 38.9f), new Vector3(0, 0, 0), new Vector3(-16.9f, 0f, 58.75f), new Vector3(0, 180, 0), new Vector3(-12.8f, 36.75f, 43.35f));
                    SetChildPosRot(new Vector3[] { new Vector3(-2f, 0, -4.25f), new Vector3(0.55f, 0, -5.55f), new Vector3(-4.1f, 0, -3.88f), new Vector3(2.5f, 0, -5.7f), new Vector3(-3.6f, 0, -4.75f) },
                                   new Vector3[] { new Vector3(0, 0, 0), new Vector3(0, 0, 0), new Vector3(0, 0, 0), new Vector3(0, 0, 0), new Vector3(0, 0, 0) },
                                   new Vector3[] { new Vector3(-8.75f, 0, 50f), new Vector3(25.1f, 0, -7.0f) },
                                   new Vector3[] { new Vector3(0, 180, 0), new Vector3(0, 320, 0) });
                    break;
                case BehaviorSelectionType.Reinforcements:
                    SetPosRot(new Vector3(-16.85f, 0, 38.9f), new Vector3(0, 0, 0), new Vector3(-16.9f, 0f, 58.75f), new Vector3(0, 180, 0), new Vector3(-12.8f, 36.75f, 43.35f));
                    SetChildPosRot(new Vector3[] { new Vector3(3, 0, 16.4f), new Vector3(0.55f, 0, -5.55f), new Vector3(-4.1f, 0, -3.88f), new Vector3(2.5f, 0, -5.7f), new Vector3(-3.6f, 0, -4.75f) },
                                   new Vector3[] { new Vector3(0, 335, 0), new Vector3(0, 0, 0), new Vector3(0, 0, 0), new Vector3(0, 0, 0), new Vector3(0, 0, 0) },
                                   new Vector3[] { new Vector3(-1.65f, 0, 0), new Vector3(0, 0, 0) },
                                   new Vector3[] { new Vector3(0, 0, 0), new Vector3(0, 0, 0) });
                    break;
            }

            yield return new WaitForSeconds(0.1f);

            for (int i = 0; i < enemyHealth.Length; ++i) {
                enemyHealth[i].ResetHealth();
            }

            if (enemyBehaviorTreeGroup.ContainsKey((int)selectionType)) {
                var trees = enemyBehaviorTreeGroup[(int)selectionType];
                for (int i = 0; i < trees.Count; ++i) {
                    trees[i].EnableBehavior();
                }
            }

            for (int i = 0; i < agentBehaviorTreeGroup[(int)selectionType].Count; ++i) {
                agentBehaviorTreeGroup[(int)selectionType][i].EnableBehavior();
            }
        }

        private void SetPosRot(Vector3 agentGroupPosition, Vector3 agentGroupRotation, Vector3 enemyGroupPosition, Vector3 enemyGroupRotation, Vector3 cameraPosition)
        {
            agentGroup.transform.position = agentGroupPosition;
            agentGroup.transform.eulerAngles = agentGroupRotation;
            enemyGroup.transform.position = enemyGroupPosition;
            enemyGroup.transform.eulerAngles = enemyGroupRotation;
            Camera.main.transform.position = cameraPosition;
        }

        private void SetChildPosRot(Vector3[] agentPositions, Vector3[] agentRotations, Vector3[] enemyPositions, Vector3[] enemyRotations)
        {
            for (int i = 0; i < agentGroup.transform.childCount; ++i) {
                var child = agentGroup.transform.Find("Agent " + (i + 1));
                child.localPosition = agentPositions[i];
                child.localEulerAngles = agentRotations[i];
            }
            for (int i = 0; i < enemyGroup.transform.childCount; ++i) {
                var child = enemyGroup.transform.Find("Enemy " + (i + 1));
                child.localPosition = enemyPositions[i];
                child.localEulerAngles = enemyRotations[i];
            }
        }
    }
}