using UnityEngine;
using System.Collections;

public class Agents  {
	
	public int idAgent {get; set;}
	
	public GameObject go {get; set;}

	public bool isOccupied { get; set;}

    public bool isPos { get; set; }

    public bool isLeader { get; set; }

    public  Agents(int _idAgent ,GameObject _go, bool _isOccupied, bool _isPos , bool _isLeader)
	{
		idAgent = _idAgent;
		go = _go;
		isOccupied = _isOccupied;
        isPos = _isPos;
        isLeader = _isLeader;
    }
	
}
