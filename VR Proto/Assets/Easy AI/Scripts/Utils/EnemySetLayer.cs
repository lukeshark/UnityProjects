using UnityEngine;
using System.Collections;
namespace  AxlPlay{
public class EnemySetLayer : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
		 
		
		if (1 << LayerMask.NameToLayer("Shootable") > 0){
			gameObject.layer = LayerMask.NameToLayer("Shootable");
		}
		else
		{
			Debug.Log("Please assign layer => Shootable to the gameobject: " + gameObject.name);
			
		}
		 
	}
	
	 
}
}