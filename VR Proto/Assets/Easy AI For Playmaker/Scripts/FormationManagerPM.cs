using UnityEngine;
using System.Collections;
using System.Collections.Generic;
 
namespace  AxlPlay{
public class FormationManagerPM : MonoBehaviour {
	// public static FormationManagerPM current;
    public int agentsNum;

    public bool hasLeader;

    public bool isPlayMaker;

    [Header("Formation Variables")]
    public string NameOfVariable_isDead = "isDead";
    public string NameOfVariable_isLeader = "isLeader";

    [Tooltip("The distance to look ahead to the destination. The higher the value the better the agent will avoid obstacles and less keep formation")]
    public float zLookAhead = 1f;
    [Tooltip("The agent move speed as the group is forming")]
    public float formationSpeed = 2f;
    public int leaderIndex;

    [Header("Formation Game Objects")]
    [Tooltip("The Leader of the group.")]
    public GameObject leaderAgent;
    [Tooltip("The Target that the leader must go to.")]
    public GameObject target;
    public GameObject[] agentes;

    [Header("Triangle")]
    [Tooltip("The length of the triangle")]
    public float length = 5;

    [Header("Circle & Semi Circle")]
    public bool concave = true;
    [Tooltip("Formation radius.")]
    public float radius = 5f;

    [HideInInspector]
    public string selectedTag = "";


    public Vector3 LastLeaderPos;


    public List<Agents> listAgents = new List<Agents>();
    public enum Formations
    {
        SemiCircle,
        Circle,
        V,
        Triangle,
        Wedge

    }
    [HideInInspector]
    public Formations formation;


    // V position
    [Header("formation V & Wedge")]
    public float _separation = 2;
    [HideInInspector]
    public Vector2 separation = new Vector2(2, 2);

    // triangle
    [HideInInspector]
    public int[] agentsPerSide = new int[3];
    // wedge
    [Header("formation Wedge")]
    public bool fill;

    int currentRow = 1;
    int currentAgentsPerRow = 0;
    int lastIndex;


    [HideInInspector]
    public float theta;
    void Awake()
    {
        //if (current == null)
        //    current = this;


        if (agentsNum == 0)
            Debug.Log("YOU MUST SELECT THE TAG OF THE AGENTS IN THE FORMATION MANAGER GAME OBJECT");

    }
    void Start()
    {
        loadFromTags();
        separation = new Vector2(_separation, _separation);
        // for triangle formation
        for (int i = 0; i < 3; ++i)
        {
            agentsPerSide[i] = agentsNum / 3 + (agentsNum % 3 > i ? 1 : 0);
        }
        LoadAgentList();
    }
    public void loadFromTags()
    {
        agentes = GameObject.FindGameObjectsWithTag(selectedTag);
    }
    public void LoadAgentList()
    {
        listAgents.Clear();


        //var _agentLeader = new Agents(0, leaderAgent, false, false, true);
        //listAgents.Add(_agentLeader);

        var count = 0;
        for (int i = 0; i < agentes.Length; i++)
        {
            bool _isLeader = false;
            bool _isDead = false;

            // FOR C#    
            var tempFSM = agentes[i].GetComponent<PlayMakerFSM>();



            if (tempFSM != null)
            {
                _isDead =  tempFSM.FsmVariables.FindFsmBool(NameOfVariable_isDead).Value;
                if (count == 0 && _isDead == false)
                {
                    tempFSM.FsmVariables.FindFsmBool(NameOfVariable_isLeader).Value = true;
                    var _agent = new Agents(count, agentes[i], false, false, true);
                    listAgents.Add(_agent);
                    count++;

                }
                else if (_isDead == false)
                {
                    _isLeader = false;
                    var _agent = new Agents(count, agentes[i], false, false, _isLeader);
                    listAgents.Add(_agent);
                    count++;
                }


            }
               


        }
        if (listAgents.Count > 0)
        {
            // set leader
            leaderAgent = listAgents[0].go;


            agentsNum = listAgents.Count;
        }
    }
    public int getMyIndice(GameObject _go)
    {

        // buscar el indice de mi gameobject
        foreach (var item in listAgents)
        {
            // assign a position to the agent
            if (item.go == _go)
            {
                return item.idAgent;
            }
        }
        return -1;

    }
    public Vector3 GetPosition(int index)
    {

        Vector3 newPos = Vector3.zero;

        if (leaderAgent != null)
        {
            newPos = leaderAgent.transform.position;
        }
        switch (formation)
        {
            case Formations.SemiCircle:
                newPos += GetSemiCirclePos(index, zLookAhead);
                break;
            case Formations.Circle:
                newPos += GetCirclePos(index, zLookAhead);
                break;
            case Formations.V:
                newPos += GetVPosition(index, zLookAhead);
                break;
            case Formations.Triangle:
                newPos += GetTrianglePos(index, zLookAhead);
                break;
            case Formations.Wedge:
                newPos += GetWedgePos(index, zLookAhead);

                break;
            default:
                break;
        }
        return newPos;
    }
    public Vector3 GetCirclePos(int index, float zLookAhead)
    {
        theta = Mathf.PI / agentsNum * 2;
        return new Vector3(radius * Mathf.Sin(theta * index), 0, radius * Mathf.Cos(theta * index) - radius + zLookAhead);


    }
    public Vector3 GetVPosition(int index, float zLookAhead)
    {
        return new Vector3(separation.x * (index % 2 == 0 ? -1 : 1) * (((index - 1) / 2) + 1), 0, separation.y * (((index - 1) / 2) + 1) + zLookAhead);
    }
    public Vector3 GetSemiCirclePos(int index, float zLookAhead)
    {


        var radians = theta * (((index - 1) / 2) + 1) + (concave ? 0 : Mathf.PI);
        return new Vector3(radius * Mathf.Sin(radians) * (index % 2 == 0 ? -1 : 1), 0,
                                                radius * Mathf.Cos(radians) + radius * (concave ? -1 : 1) + zLookAhead);
    }
    public Vector3 GetTrianglePos(int index, float zLookAhead)
    {

        var side = index % 3;
        var lengthMultiplier = (index / 3) / (float)agentsPerSide[side];
        lengthMultiplier = 1 - (lengthMultiplier - (int)lengthMultiplier);
        var height = length / 2 * Mathf.Sqrt(3); // Equilaterial triangle height
        if (side == 0)
        { // Right
            return new Vector3(length / 2 * lengthMultiplier, 0, -height * lengthMultiplier + zLookAhead);
        }
        else if (side == 1)
        { // Bottom
            return new Vector3(Mathf.Lerp(-length / 2, length / 2, lengthMultiplier), 0, -height + zLookAhead);
        }
        else
        { // Left
            return new Vector3(-length / 2 * lengthMultiplier, 0, -height * lengthMultiplier + zLookAhead);
        }
    }
    public Vector3 GetWedgePos(int index, float zLookAhead)
    {
        if (fill)
        {
            if (index <= lastIndex)
            {
                currentRow = 1;
                currentAgentsPerRow = 0;
            }
            lastIndex = index;

            var targetPosition = new Vector3(Mathf.Lerp(-currentRow * separation.x, currentRow * separation.x, currentAgentsPerRow / (float)currentRow), 0, -separation.y * currentRow + zLookAhead);

            currentAgentsPerRow++;
            if (currentAgentsPerRow > currentRow)
            {
                currentAgentsPerRow = 0;
                currentRow++;
            }

            return targetPosition;
        }
        else
        {
            // The wedge is not filled
            return new Vector3(separation.x * (index % 2 == 0 ? -1 : 1) * (((index - 1) / 2) + 1), 0, -separation.y * (((index - 1) / 2) + 1) + zLookAhead);
        }
    }


}
}
