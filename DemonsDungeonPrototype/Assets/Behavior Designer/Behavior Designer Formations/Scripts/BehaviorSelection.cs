using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace BehaviorDesigner.Runtime.Formations
{
    public class BehaviorSelection : MonoBehaviour
    {
        public GameObject leader;
        public GUISkin descriptionGUISkin;
        public Transform redirectTarget;
        public Transform secondaryRedirectTarget;

        private Dictionary<int, List<BehaviorTree>> behaviorTreeGroup = new Dictionary<int, List<BehaviorTree>>();
        private List<Transform> agents = new List<Transform>();
        private List<Vector3> agentStartPosition = new List<Vector3>();
        private List<Quaternion> agentStartRotation = new List<Quaternion>();

        private enum BehaviorSelectionType { Column, Row, Grid, Line, Echelon, Wedge, V, Arc, Skirmisher, Swarm, Diamond, Triangle, Square, Circle, DynamicGroup, ChangeLeader, Last }
        private BehaviorSelectionType selectionType = BehaviorSelectionType.Column;
        private BehaviorSelectionType prevSelectionType = BehaviorSelectionType.Column;

        public void Start()
        {
            var allBehaviorTrees = FindObjectsOfType<BehaviorTree>();
            for (int i = 0; i < allBehaviorTrees.Length; ++i) {
                var group = allBehaviorTrees[i].Group;
                List<BehaviorTree> groupBehaviorTrees;
                if (!behaviorTreeGroup.TryGetValue(group, out groupBehaviorTrees)) {
                    groupBehaviorTrees = new List<BehaviorTree>();
                    behaviorTreeGroup.Add(group, groupBehaviorTrees);
                }
                // The leader needs to come first for the dynamic group selection type.
                if (allBehaviorTrees[i].gameObject == leader) {
                    groupBehaviorTrees.Insert(0, allBehaviorTrees[i]);
                } else {
                    groupBehaviorTrees.Add(allBehaviorTrees[i]);
                }
            }
            var allAgents = FindObjectsOfType<MeshRenderer>();
            for (int i = 0; i < allAgents.Length; ++i) {
                if (allAgents[i].GetComponent<Behavior>() == null) {
                    continue;
                }
                agents.Add(allAgents[i].transform);
                agentStartPosition.Add(allAgents[i].transform.position);
                agentStartRotation.Add(allAgents[i].transform.rotation);
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
                if ((int)selectionType < 0) selectionType = BehaviorSelectionType.ChangeLeader;
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
                case BehaviorSelectionType.Column:
                    desc = "Arrange the group in one or more columns where the column is significantly longer than the width of rows.";
                    break;
                case BehaviorSelectionType.Row:
                    desc = "Arrange the group in one or more rows with the row significantly wider than the length of the column.";
                    break;
                case BehaviorSelectionType.Grid:
                    desc = "Arrange the group in a grid where the number of rows is equal to the number of columns.";
                    break;
                case BehaviorSelectionType.Line:
                    desc = "Arrange the group in a straight horizontal line.";
                    break;
                case BehaviorSelectionType.Echelon:
                    desc = "Arrange the group in a diagonal formation.";
                    break;
                case BehaviorSelectionType.Wedge:
                    desc = "Arrange the group in an upside down V shape where the leader is in the front.";
                    break;
                case BehaviorSelectionType.V:
                    desc = "Arrange the group in a V shape where the leader is in the back.";
                    break;
                case BehaviorSelectionType.Arc:
                    desc = "Arrange the group in an arc.";
                    break;
                case BehaviorSelectionType.Skirmisher:
                    desc = "Arrange the group in a randomly spread out line.";
                    break;
                case BehaviorSelectionType.Swarm:
                    desc = "Arrange the group in a tight circle that can move together.";
                    break;
                case BehaviorSelectionType.Diamond:
                    desc = "Arrange the group in a tactical diamond shape.";
                    break;
                case BehaviorSelectionType.Triangle:
                    desc = "Arrange the group in a triangle.";
                    break;
                case BehaviorSelectionType.Square:
                    desc = "Arrange the group in a square.";
                    break;
                case BehaviorSelectionType.Circle:
                    desc = "Arrange the group in a circle.";
                    break;
                case BehaviorSelectionType.DynamicGroup:
                    desc = "Agents can join and leave the formation at any time. The formation will dynamically respond to changes.";
                    break;
                case BehaviorSelectionType.ChangeLeader:
                    desc = "Agents can change leaders dynamically to form a new groups.";
                    break;
            }
            return desc;
        }

        private void SelectionChanged()
        {
            StopCoroutine("EnableBehavior");
            for (int i = 0; i < behaviorTreeGroup[(int)prevSelectionType].Count; ++i) {
                behaviorTreeGroup[(int)prevSelectionType][i].DisableBehavior();
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
            yield return new WaitForSeconds(0.1f);

            for (int i = 0; i < behaviorTreeGroup[(int)selectionType].Count; ++i) {
                if (selectionType != BehaviorSelectionType.DynamicGroup || (i < 4)) {
                    behaviorTreeGroup[(int)selectionType][i].EnableBehavior();
                }
                agents[i].position = agentStartPosition[i];
                agents[i].rotation = agentStartRotation[i];
            }

            if (selectionType == BehaviorSelectionType.DynamicGroup) {
                yield return new WaitForSeconds(8);
                for (int i = 4; i < agents.Count; ++i) {
                    behaviorTreeGroup[(int)selectionType][i].EnableBehavior();
                    yield return new WaitForSeconds(5);
                }

                yield return new WaitForSeconds(2);
                for (int i = 3; i < 6; ++i) {
                    behaviorTreeGroup[(int)selectionType][i].DisableBehavior();
                    yield return new WaitForSeconds(4);
                }
            } else if (selectionType == BehaviorSelectionType.ChangeLeader) {
                yield return new WaitForSeconds(6);
                // Find the current leader and new leader.
                int leaderIndex = -1;
                int newLeaderIndex = -1;
                int secondaryLeaderIndex = -1;
                for (int i = 0; i < behaviorTreeGroup[(int)selectionType].Count; ++i) {
                    if (behaviorTreeGroup[(int)selectionType][i].gameObject.name == "Agent 1") {
                        leaderIndex = i;
                    } else if (behaviorTreeGroup[(int)selectionType][i].gameObject.name == "Agent 2") {
                        newLeaderIndex = i;
                    } else if (behaviorTreeGroup[(int)selectionType][i].gameObject.name == "Agent 6") {
                        secondaryLeaderIndex = i;
                    }
                }

                // Setup the new leaders.
                behaviorTreeGroup[(int)selectionType][newLeaderIndex].SetVariableValue("Leader", null);
                behaviorTreeGroup[(int)selectionType][secondaryLeaderIndex].SetVariableValue("Leader", null);
                behaviorTreeGroup[(int)selectionType][secondaryLeaderIndex].SetVariableValue("TargetTransform", secondaryRedirectTarget);

                // Inform the agents of their new leader.
                var leader = behaviorTreeGroup[(int)selectionType][newLeaderIndex].gameObject;
                var secondaryLeader = behaviorTreeGroup[(int)selectionType][secondaryLeaderIndex].gameObject;
                for (int i = 0; i < behaviorTreeGroup[(int)selectionType].Count; ++i) {
                    if (i == leaderIndex || i == newLeaderIndex || i == secondaryLeaderIndex) {
                        continue;
                    }
                    // Distribute the followers according to the following leaders:
                    // - Agent 1 (original leader): Agent 3
                    // - Agent 2 (primary leader): Agent 4, Agent 5
                    // - Agent 3 (secondary leader): Agent 7, Agent 8, Agent 9
                    var agentName = behaviorTreeGroup[(int)selectionType][i].gameObject.name;
                    if (agentName == "Agent 4" || agentName == "Agent 5") {
                        behaviorTreeGroup[(int)selectionType][i].SetVariableValue("Leader", leader);
                    } else if (agentName != "Agent 3") {
                        behaviorTreeGroup[(int)selectionType][i].SetVariableValue("Leader", secondaryLeader);
                    }
                }

                yield return new WaitForSeconds(0.1f);

                // Change the current leader target after a small delay so the leader values have updated on all of the agents.
                behaviorTreeGroup[(int)selectionType][leaderIndex].SetVariableValue("TargetTransform", redirectTarget);
            }
        }
    }
}