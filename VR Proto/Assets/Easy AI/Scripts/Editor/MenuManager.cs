using UnityEngine;
using UnityEditor;

public class MenuManager
{
	[MenuItem ("Tools/Easy AI/Create/Formation Manager")]
	private static void CreateFormationManager ()
	{

		var exist = GameObject.Find ("Formation Manager");
		if (!exist) {
			GameObject temp = new GameObject ();
			temp.name = "Formation Manager";
			temp.transform.position = Vector3.zero;
			temp.AddComponent<FormationManager> ();

		} else {
			Debug.Log ("Formation Manager => Already exist.");	
		}
        

	}

	[MenuItem ("Tools/Easy AI/Create/Agent")]
	private static void CreateAgent ()
	{
		
		var temp = GameObject.CreatePrimitive (PrimitiveType.Capsule);
		temp.name = "Agent";
		temp.transform.position = Vector3.zero;
		temp.AddComponent<UnityEngine.AI.NavMeshAgent> ();
		temp.AddComponent<AgentScript> ();
 
		
	}

	//[MenuItem ("Component/easy AI/Patrol")]
	//private static void AddPatrolStcript ()
	//{
		
		
	//}


}
