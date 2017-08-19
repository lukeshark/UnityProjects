using UnityEngine;
using System.Collections;

namespace AxlPlay {
public class FXDeactive : MonoBehaviour {

	
	void OnEnable(){
		
		StartCoroutine("deactive");
	}
	IEnumerator deactive(){
		
		yield return new WaitForSeconds(3f);
		
		gameObject.SetActive(false);
	}
	
	 
}
}
