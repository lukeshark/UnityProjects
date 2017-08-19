using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using AxlPlay;

public class FormationManager : MonoBehaviour
{
	public FormationManager current;
	public int agentsNum;

	public bool hasLeader;

	public bool isPlayMaker;

	[Header ("Formation Variables")]

	[Tooltip ("The distance to look ahead to the destination. The higher the value the better the agent will avoid obstacles and less keep formation")]
	public float zLookAhead = 1f;
	[Tooltip ("The agent move speed as the group is forming")]
	public float formationSpeed = 2f;
	public int leaderIndex;



	[Header ("Formation Game Objects")]
	[Tooltip ("The Leader of the group.")]
	public GameObject leaderAgent;
	[Tooltip ("The Target that the leader must go to.")]
	public GameObject target;
	public GameObject[] agentes;

	[Header ("Triangle")]
	[Tooltip ("The length of the triangle")]
	public float length = 5;

	[Header ("Circle & Semi Circle")]
	public bool concave = true;
	[Tooltip ("Formation radius.")]
	public float radius = 5f;

	public bool right;

	public int columns = 1;

	public bool backPositionOffset;

	[HideInInspector]
	public string selectedTag = "";


	public Vector3 LastLeaderPos;


	public List<Agents> listAgents = new List<Agents> ();

	public enum Formations
	{
		SemiCircle,
		Circle,
		V,
		Triangle,
		Wedge,
		Column,
		Diamond,
		Diagonal,
		Grid,
		Line
	}

	[HideInInspector]
	public Formations formation;


	// V position
	[Header ("formation V & Wedge")]
	public float _separation = 2;
	[HideInInspector]
	public Vector2 separation = new Vector2 (2, 2);

	// triangle
	[HideInInspector]
	public int[] agentsPerSide = new int[3];
	// wedge
	[Header ("formation Wedge")]
	public bool fill;

	int currentRow = 1;
	int currentAgentsPerRow = 0;
	int lastIndex;


	[HideInInspector]
	public float theta;

	void Awake ()
	{
		if (current == null)
			current = this;


		if (agentsNum == 0)
			Debug.Log ("YOU MUST SELECT THE TAG OF THE AGENTS IN THE FORMATION MANAGER GAME OBJECT");

	}

	void Start ()
	{
		loadFromTags ();
		separation = new Vector2 (_separation, _separation);
		// for triangle formation
		for (int i = 0; i < 3; ++i) {
			agentsPerSide [i] = agentsNum / 3 + (agentsNum % 3 > i ? 1 : 0);
		}
		LoadAgentList ();
	}

	public void loadFromTags ()
	{
		agentes = GameObject.FindGameObjectsWithTag (selectedTag);
	}

	public void LoadAgentList ()
	{
		listAgents.Clear ();


		//var _agentLeader = new Agents(0, leaderAgent, false, false, true);
		//listAgents.Add(_agentLeader);

		var count = 0;
		for (int i = 0; i < agentes.Length; i++) {
			bool _isLeader = false;

			// FOR C#    
			var _script = agentes [i].GetComponent<AgentScript> ();
			if (_script != null && count == 0 && _script.isDead == false) {
				_script.isLeader = true;
				_isLeader = true;
				var _agent = new Agents (count, agentes [i], false, false, _isLeader);
				listAgents.Add (_agent);
				count++;

			} else if (_script != null && _script.isDead == false) {
				_script.isLeader = false;
				var _agent = new Agents (count, agentes [i], false, false, _isLeader);
				listAgents.Add (_agent);
				count++;
			}


		}
		if (listAgents.Count > 0) {
			// set leader
			leaderAgent = listAgents [0].go;


			agentsNum = listAgents.Count;
		}
	}

	public void ListTest ()
	{
		for (int i = 0; i < listAgents.Count; i++) {
			var pos = GetPosition (i);
			var go = GameObject.CreatePrimitive (PrimitiveType.Sphere);
			go.name = listAgents [i].go + " => " + i.ToString ();
			go.transform.position = pos;
			Debug.Log (i + " -- " + listAgents [i].go + " --- " + listAgents [i].idAgent + " -- " + pos);
		}


	}

	public Vector3 GetPosition (int index)
	{
		
		Vector3 newPos = Vector3.zero;

		if (leaderAgent != null) {
			newPos = leaderAgent.transform.position;
		}
		switch (formation) {
		case Formations.SemiCircle:
			newPos += GetSemiCirclePos (index, zLookAhead);
			break;
		case Formations.Circle:
			newPos += GetCirclePos (index, zLookAhead);
			break;
		case Formations.V:
			newPos += GetVPosition (index, zLookAhead);
			break;
		case Formations.Triangle:
			newPos += GetTrianglePos (index, zLookAhead);
			break;
		case Formations.Wedge:
			newPos += GetWedgePos (index, zLookAhead);

			break;
		case Formations.Column:
			newPos += GetColumnPos (index, zLookAhead);

			break;
		case Formations.Diamond:
			newPos += GetDiamondPos (index, zLookAhead);

			break;
		case Formations.Diagonal:
			newPos += GetDiagonalPos (index, zLookAhead);

			break;
		case Formations.Grid:
			newPos += GetGridPos (index, zLookAhead);

			break;
		case Formations.Line:
			newPos += GetLinePos (index, zLookAhead);

			break;
		default:
			break;
		}
		return newPos;
	}
	//public Vector3 GetcirclePosition(int pointID)
	//{

	//    float curAngle = ((float)pointID / (float)agentsNum) * (maxAngle);

	//    curAngle = curAngle + (leaderAgent.transform.localEulerAngles.y * -1);
	//    float curRadians = (curAngle + moverAngle) * Mathf.Deg2Rad;

	//    Vector3 curPos = new Vector3(Mathf.Cos(curRadians), 0f, Mathf.Sin(curRadians)) * radius;

	//    return curPos + leaderAgent.transform.localPosition;
	//}
	public Vector3 GetCirclePos (int index, float zLookAhead)
	{
		theta = Mathf.PI / agentsNum * 2;
		return new Vector3 (radius * Mathf.Sin (theta * index), 0, radius * Mathf.Cos (theta * index) - radius + zLookAhead);


	}

	public Vector3 GetVPosition (int index, float zLookAhead)
	{
		return new Vector3 (separation.x * (index % 2 == 0 ? -1 : 1) * (((index - 1) / 2) + 1), 0, separation.y * (((index - 1) / 2) + 1) + zLookAhead);
	}

	public Vector3 GetSemiCirclePos (int index, float zLookAhead)
	{


		var radians = theta * (((index - 1) / 2) + 1) + (concave ? 0 : Mathf.PI);
		return new Vector3 (radius * Mathf.Sin (radians) * (index % 2 == 0 ? -1 : 1), 0,
			radius * Mathf.Cos (radians) + radius * (concave ? -1 : 1) + zLookAhead);
	}

	public Vector3 GetTrianglePos (int index, float zLookAhead)
	{

		var side = index % 3;
		var lengthMultiplier = (index / 3) / (float)agentsPerSide [side];
		lengthMultiplier = 1 - (lengthMultiplier - (int)lengthMultiplier);
		var height = length / 2 * Mathf.Sqrt (3); // Equilaterial triangle height
		if (side == 0) { // Right
			return new Vector3 (length / 2 * lengthMultiplier, 0, -height * lengthMultiplier + zLookAhead);
		} else if (side == 1) { // Bottom
			return new Vector3 (Mathf.Lerp (-length / 2, length / 2, lengthMultiplier), 0, -height + zLookAhead);
		} else { // Left
			return new Vector3 (-length / 2 * lengthMultiplier, 0, -height * lengthMultiplier + zLookAhead);
		}
	}

	public Vector3 GetColumnPos (int index, float zLookAhead)
	{
		var column = index % columns;
		var row = index / columns;

		Vector3 targetPos;
		if (column == 0) {
			// Position directly behind the leader
			targetPos = new Vector3 (0, 0, -separation.y * row + zLookAhead);
		} else {
			// Alternate between the right and the left side of the center column
			targetPos = new Vector3 (separation.x * (column % 2 == 0 ? -1 : 1) * (((column - 1) / 2) + 1), 0, -separation.y * row + zLookAhead);
		}

		return targetPos;


	}

	public Vector3 GetWedgePos (int index, float zLookAhead)
	{
		if (fill) {
			if (index <= lastIndex) {
				currentRow = 1;
				currentAgentsPerRow = 0;
			}
			lastIndex = index;

			var targetPosition = new Vector3 (Mathf.Lerp (-currentRow * separation.x, currentRow * separation.x, currentAgentsPerRow / (float)currentRow), 0, -separation.y * currentRow + zLookAhead);

			currentAgentsPerRow++;
			if (currentAgentsPerRow > currentRow) {
				currentAgentsPerRow = 0;
				currentRow++;
			}

			return targetPosition;
		} else {
			// The wedge is not filled
			return new Vector3 (separation.x * (index % 2 == 0 ? -1 : 1) * (((index - 1) / 2) + 1), 0, -separation.y * (((index - 1) / 2) + 1) + zLookAhead);
		}
	}

	public Vector3 GetDiamondPos (int index, float zLookAhead)
	{
		Vector3 targetPos;
		if (index < 3) { // form the diamond part
			targetPos = new Vector3 (separation.x * (index % 2 == 0 ? -1 : 1), 0, -separation.y + zLookAhead);
		} else { // form the back of the diamond. This is a tactical diamond so it is made for agents to cover themselves down hallways.
			if (backPositionOffset) {
				targetPos = new Vector3 (separation.x * (index % 2 == 0 ? -0.5f : 0.5f), 0, -separation.y * (((index - 1) / 2) + 1) + zLookAhead);
			} else {
				targetPos = new Vector3 (0, 0, -separation.y * (index - 1) + zLookAhead);
			}
		}
		return targetPos;



	}

	public Vector3 GetDiagonalPos (int index, float zLookAhead)
	{
		// Position at a diagonal relative to the leader
		return new Vector3 (separation.x * index * (right ? 1 : -1), 0, -separation.y * index + zLookAhead);


	}

	public Vector3 GetGridPos (int index, float zLookAhead)
	{

		var row = index % columns;
		var column = index / columns;

		return new Vector3 (separation.x * column, 0, -separation.y * row + zLookAhead);


	}

	public Vector3 GetLinePos (int index, float zLookAhead)
	{
		return new Vector3 (separation.x * index * (right ? 1 : -1), 0, zLookAhead);

	}

}

