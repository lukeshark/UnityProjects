using UnityEngine;
using UnityEditor;

public class MenuManager
{
    [MenuItem("Easy AI/Create/Formation Manager")]
    private static void CreateFormationManager()
    {

        var exist = GameObject.Find("Formation Manager");
        if (!exist)
        {
            GameObject temp = new GameObject();
            temp.name = "Formation Manager";
            temp.transform.position = Vector3.zero;
            temp.AddComponent<FormationManager>();

        }
	    else
	    {
		    Debug.Log("Formation Manager => Already exist.");	
        }
        

    }
	[MenuItem("Easy AI/Create/Agent")]
	private static void CreateAgent(){
		
		var  temp = GameObject.CreatePrimitive(PrimitiveType.Capsule);
		temp.name = "Agent";
		temp.transform.position = Vector3.zero;
		temp.AddComponent<NavMeshAgent>();
		temp.AddComponent<AgentScript>();
		var _agentScript = temp.GetComponent<AgentScript>();
 
		
	}
	[MenuItem("Component/easy AI/Patrol")]
	private static void AddPatrolStcript()
	{
		
		
	}


}
