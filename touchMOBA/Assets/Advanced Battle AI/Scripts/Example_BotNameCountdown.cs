// SIMPLE EXAMPLE THAT ADDS A COUNT-DOWN TIMER ON TOP OF A BOT
// Depends on the DisplayBotName add-on (source available on the AI Addons folder)

// This shows you how to interact with the AI BOT FSMs and extend functionality thru scripting.

// COPYRIGHT 2014-2015 AIBOTSYSTEM.com

using UnityEngine;
using System.Collections;
using HutongGames.PlayMaker;

public class Example_BotNameCountdown : MonoBehaviour {

	// fsms
  	private PlayMakerFSM[] pmFSMs;
	private PlayMakerFSM FSM_Identity;

	// fsm vars
	private FsmString botname;

	private int currentCount = 1000;	// starting count; we're going to count down from 1000

	private string newBotName = "";


	// Use this for initialization
	void Start () {
		// fetch all the FSMs so we can loop through each and find the one we want:
		pmFSMs =  gameObject.GetComponents<PlayMakerFSM>();

		if (pmFSMs != null){
			foreach (PlayMakerFSM tempFSM in pmFSMs) {
				// we need the IDENTITY Fsm so we can manipulate the bot's name tag:
				if (tempFSM.FsmName == "Identity"){
					
					FSM_Identity = tempFSM;	// get our specific FSM
					botname = FSM_Identity.FsmVariables.GetFsmString("charSTAT_Name"); // get the bot name variable

					InvokeRepeating("updateBotName", 2, 1);
				}
	        }
		}

	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void updateBotName(){

		currentCount--;
		newBotName = "Count: " + currentCount;

		// update the name in the FSM, which the other add-on "DisplayBotName" will update automatically:
		botname.Value = newBotName;
	}
}
