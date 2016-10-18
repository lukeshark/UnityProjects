using UnityEngine;
using System.Collections;

public class TakeDamage : MonoBehaviour {


	private PlayMakerFSM fsm;
	public string NameOfVariableThatReceiveTheDamage;
	public string NameOfFsm;
	public string NameOfEventThatRunOnReceiveDamage;

	void Awake() {

		fsm = GetComponent<PlayMakerFSM> ();
	}


	public virtual void Damage(float damage)
	{

		var tempFSM = PlayMakerFSM.FindFsmOnGameObject (this.gameObject,NameOfFsm);
			
		tempFSM.FsmVariables.FindFsmFloat (NameOfVariableThatReceiveTheDamage).Value = damage;
		tempFSM.SendEvent (NameOfEventThatRunOnReceiveDamage);
	}
}



