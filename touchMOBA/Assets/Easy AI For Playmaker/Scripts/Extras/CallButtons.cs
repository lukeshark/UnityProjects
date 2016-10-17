using UnityEngine;
using System.Collections;
using HutongGames.PlayMaker;
public class CallButtons : MonoBehaviour {

	public PlayMakerFSM playmaker;



	public void CanSeeObject () {
		
		playmaker.Fsm.Event ("See");


	}
	public void CanHearObject () {

		playmaker.Fsm.Event ("Hear");


	}
	public void Flock () {

		playmaker.Fsm.Event ("Flock");


	}

	public void Cover () {

		playmaker.Fsm.Event ("Cover");


	}
	public void Flee () {

		playmaker.Fsm.Event ("Flee");


	}
	public void Pursue () {

		playmaker.Fsm.Event ("Pursue");


	}
	public void Wander () {

		playmaker.Fsm.Event ("Wander");


	}
	public void Patrol () {

		playmaker.Fsm.Event ("Patrol");


	}
	public void Evade () {

		playmaker.Fsm.Event ("Evade");


	}
	public void Search () {

		playmaker.Fsm.Event ("Search");


	}
}
