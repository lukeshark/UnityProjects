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

        private Dictionary<int, BehaviorTree> behaviorTreeGroup = new Dictionary<int, BehaviorTree>();
        private List<BehaviorTree> agents = new List<BehaviorTree>();
        private Vector3 leaderStartPosition;
        private Quaternion leaderStartRotation;
        private List<Vector3> agentStartPosition = new List<Vector3>();
        private List<Quaternion> agentStartRotation = new List<Quaternion>();

        private enum BehaviorSelectionType { Column, Row, Grid, Line, Echelon, Wedge, V, Arc, Skirmisher, Swarm, Diamond, Triangle, Square, Circle, DynamicGroup, Last }
        private BehaviorSelectionType selectionType = BehaviorSelectionType.Column;
        private BehaviorSelectionType prevSelectionType = BehaviorSelectionType.Column;

        public void Start()
        {
            leaderStartPosition = leader.transform.position;
            leaderStartRotation = leader.transform.rotation;

            var behaviorTrees = leader.GetComponents<BehaviorTree>();
            for (int i = 0; i < behaviorTrees.Length; ++i) {
                behaviorTreeGroup.Add(behaviorTrees[i].Group, behaviorTrees[i]);
            }
            var allAgents = FindObjectsOfType<BehaviorTree>();
            for (int i = 0; i < allAgents.Length; ++i) {
                if (allAgents[i].gameObject != leader) {
                    agentStartPosition.Add(allAgents[i].transform.position);
                    agentStartRotation.Add(allAgents[i].transform.rotation);
                    agents.Add(allAgents[i]);
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
                if ((int)selectionType < 0) selectionType = BehaviorSelectionType.DynamicGroup;
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
            }
            return desc;
        }

        private void SelectionChanged()
        {
            StopCoroutine("EnableBehavior");
            behaviorTreeGroup[(int)prevSelectionType].DisableBehavior();
            for (int i = 0; i < agents.Count; ++i) {
                agents[i].DisableBehavior();
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

            leader.transform.position = leaderStartPosition;
            leader.transform.rotation = leaderStartRotation;
            behaviorTreeGroup[(int)selectionType].EnableBehavior();
            for (int i = 0; i < agents.Count; ++i) {
                agents[i].transform.position = agentStartPosition[i];
                agents[i].transform.rotation = agentStartRotation[i];
                if (selectionType != BehaviorSelectionType.DynamicGroup || (i < 4)) {
                    agents[i].EnableBehavior();
                }
            }

            if (selectionType == BehaviorSelectionType.DynamicGroup) {
                yield return new WaitForSeconds(5);
                for (int i = 4; i < agents.Count; ++i) {
                    agents[i].EnableBehavior();
                    yield return new WaitForSeconds(3);
                }

                yield return new WaitForSeconds(4);
                for (int i = 3; i < 6; ++i) {
                    agents[i].DisableBehavior();
                    yield return new WaitForSeconds(2);
                }
            }
        }
    }
}