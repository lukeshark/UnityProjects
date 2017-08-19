using UnityEngine;
using System.Collections;
using UnityEditor;

namespace AxlPlay {
public class MenuManagerPlayMaker {
	
	[MenuItem("Tools/Easy AI/Create/Formation Manager PlayMaker")]
	private static void CreateFormationManager()
	{
		
		var exist = GameObject.Find("Formation Manager PlayMaker");
		if (!exist)
		{
			GameObject temp = new GameObject();
			temp.name = "Formation Manager PlayMaker";
			temp.transform.position = Vector3.zero;
			temp.AddComponent<FormationManagerPM>();
			
		}
		else
		{
			Debug.Log("Formation Manager PlayMaker => Already exist.");	
		}
		
		
	}

	 
}
}
