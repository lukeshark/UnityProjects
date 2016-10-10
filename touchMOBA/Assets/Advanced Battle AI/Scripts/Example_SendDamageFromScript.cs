/*
	(C) 2014-2015 AIBOTSYSTEM.com

	EXAMPLE SCRIPT: Sending Damage from C#
	Description: This sample script sends Damage to the bot loaded into the botGameObject slot.
	This shows you how to integrate our A.I. with existing systems, if you decide to choose the scripting way.

	It doesn't use anything you don't already have: It simply uses the PlayMaker API to send variables
	back and forth, and call the "TakeDmg" FSM event that is attached to all the bots, found in their HealthAndDamage FSM.

	Note: This sample script doesn't do any error checking -- we kept it as simple and light as possible
	so you can understand it faster, then customize it to your own needs. So if any variables are missing,
	or the bot prefab you're using is broken, this will throw errors.  Using the given bot template in the 
	/ AI Structure / folder shouldn't give any errors with this.


*/

using UnityEngine;
using System.Collections;
using HutongGames.PlayMaker;


public class Example_SendDamageFromScript : MonoBehaviour {


	public GameObject botGameObject = null;
	public float Damage = 50.0f;

  	private PlayMakerFSM[] pmFSMs;
	private PlayMakerFSM FSM_Health;
	private FsmFloat TakeDmgAmount;


	void Start () {
	
	}
	

	void Update () {
		// PRESS "D" to send damage to the bot...
		if (Input.GetKeyDown("d")){
			SendDamage();
		}
	}



	void SendDamage () {
			pmFSMs =  botGameObject.GetComponents<PlayMakerFSM>();

			if (pmFSMs != null){
				foreach (PlayMakerFSM tempFSM in pmFSMs) {
					if (tempFSM.FsmName == "HealthAndDamage"){
						FSM_Health = tempFSM;	// get our Health FSM
						TakeDmgAmount = FSM_Health.FsmVariables.GetFsmFloat("takeDamageAmount");
	                    TakeDmgAmount.Value = Damage; // LOAD DAMAGE AMOUNT
	                    FSM_Health.SendEvent("TakeDmg");
					} // end if
	           	} // end foreach
			} // end if
	} // end function	



}
