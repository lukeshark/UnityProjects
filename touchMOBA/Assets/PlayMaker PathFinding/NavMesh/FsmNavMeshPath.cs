// (c) Copyright HutongGames, LLC 2010-2014. All rights reserved.
//
// TODO: implement FsmNavMeshPath properly in NavMeshCalculatePath and NaMeshCalculatePathBetweenGameObject.
// this is currently very much under progress, not sure if this is the right way to go about this. maybe too advanced and should be left to user to implement this?

using UnityEngine;
using System.Collections;

public class FsmNavMeshPath : MonoBehaviour {
	
	//Corner points of path
	public Vector3[] corners;
	/*
	{
		get { 
			if (path== null)
			{
			 return null;
			}
			return path.corners;
		}
	}
	*/
	
	public NavMeshPathStatus status
	{
		get
		{ 
			if (path== null)
			{
			 return NavMeshPathStatus.PathInvalid;
			}	
		return path.status;
		}
	}

	public NavMeshPath path;
	
	// Use this for initialization
	void Start () {
		
	}
	
	void ClearCorners()
	{
		path.ClearCorners();
	}
	
	public string GetStatusString()
	{
		if (path ==null){
			return "n/a";
		}else{
			return path.status.ToString();
		}
	}
	
}