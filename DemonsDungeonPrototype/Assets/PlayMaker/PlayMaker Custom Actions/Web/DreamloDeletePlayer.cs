// (c) Copyright HutongGames, LLC 2010-2016. All rights reserved.
// Made by Djaydino http://www.jinxtergames.com
// Works with http://dreamlo.com/
/*--- __ECO__ __PLAYMAKER__ __ACTION__ ---*/

using UnityEngine;
using System.Collections;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("Web")]
	[Tooltip("Saves highscore to Dreamlo website")]
	public class DreamloDeletePlayer : FsmStateAction
	
    {
		
		[ActionSection("Code")]
		
		[RequiredField]
		[Tooltip("Place Private Code from the Dreamlo website. Be Sure The Code is correct, this will not give an error when wrong")]
		public FsmString privateCode;
		
		[ActionSection("Data")]

		[RequiredField]
		[Tooltip("Place Player Name Here. Player Name Must be urlfriendly (if player name exists it will change to the highest score and not add the name a 2nd time)")]
		public FsmString PlayerName;
		
		[ActionSection("Error")]

		public FsmEvent errorEvent;
		[Tooltip("Where any errors thrown will be stored. Set this to a variable, or leave it blank.")]
		
		public FsmString errorMessage = "";
		
		private WWW www;
		
        private Coroutine routine;
		private string webUrl;

        public override void Reset()
        {
			privateCode = null;
			PlayerName = null;
			errorMessage = "";
        }

        public override void OnEnter()
        {
			webUrl = "http://dreamlo.com/lb/" + privateCode.Value + "/delete/" + PlayerName.Value;
			routine = StartCoroutine(UploadNewHighscore());
		}		

        private IEnumerator UploadNewHighscore()
        {
			www = new WWW(webUrl);
            yield return www;
			
			if (string.IsNullOrEmpty(www.error))
			{
				Finish();	
			}
			else
			{
				errorMessage = www.error;
				Fsm.Event(errorEvent);
			}
			
            
        }

        public override void OnExit()
        {
            StopCoroutine(routine);
        }
    }
}