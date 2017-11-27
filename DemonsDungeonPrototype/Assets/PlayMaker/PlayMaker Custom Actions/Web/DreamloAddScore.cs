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
	public class DreamloAddScore : FsmStateAction
	
    {
		[RequiredField]
		[Tooltip("Place Private Code from the Dreamlo website. Be Sure The Code is correct, this will not give an error when wrong")]

		[ActionSection("Code")]
		
		public FsmString privateCode;

		[ActionSection("Data")]
		
		[RequiredField]
		[Tooltip("Place Player Name Here. Player Name Must be urlfriendly (if player name exists it will change to the highest score and not add the name a 2nd time)")]
		public FsmString PlayerName;
		
		[RequiredField]
		[Tooltip("Place Player Score Here")]
		public FsmInt PlayerScore;
		
		[Tooltip("Place Player Seconds left, Seconds done or any int value you need (level for example)")]
		public FsmInt Seconds;
		
		[Tooltip("Place any String here for example surname or rank (like Sergeant for example)")]
		public FsmString Text;
		
		[ActionSection("Error")]

		public FsmEvent isError;
		[Tooltip("Where any errors thrown will be stored. Set this to a variable, or leave it blank.")]
		
		public FsmString errorMessage = "";
		
		private WWW www;
		
        private Coroutine routine;
		private string webUrl;

        public override void Reset()
        {
			privateCode = null;
			PlayerName = null;
			PlayerScore = new FsmInt() {UseVariable=true};
			Seconds = new FsmInt() {UseVariable=true};
			Text = new FsmString() {UseVariable=true};
			errorMessage = "";
        }

        public override void OnEnter()
        {
			if (Text != null)
			{
				if (Seconds == null)
				{
					Seconds = 0;
				}
			webUrl = "http://dreamlo.com/lb/" + privateCode.Value + "/add/" + PlayerName.Value + "/" + PlayerScore.Value + "/" + Seconds.Value + "/" + Text.Value;
			routine = StartCoroutine(UploadNewHighscore());
			}
			if (Seconds != null & Text == null)
			{
			webUrl = "http://dreamlo.com/lb/" + privateCode.Value + "/add/" + PlayerName.Value + "/" + PlayerScore.Value + "/" + Seconds.Value;
			routine = StartCoroutine(UploadNewHighscore());
			}
			if (Text == null & Seconds == null)
			{
			webUrl = "http://dreamlo.com/lb/" + privateCode.Value + "/add/" + PlayerName.Value + "/" + PlayerScore.Value;
			routine = StartCoroutine(UploadNewHighscore());
			}
        }

						
		

        private IEnumerator UploadNewHighscore()
        {
			www = new WWW(webUrl);
            yield return www;
			
			if (string.IsNullOrEmpty(www.error))
			{
				errorMessage = www.error;
				Finish();	
			}
			else
			{
				errorMessage = www.error;
				Fsm.Event(isError);
			}
			
            
        }

        public override void OnExit()
        {
            StopCoroutine(routine);
        }
    }
}