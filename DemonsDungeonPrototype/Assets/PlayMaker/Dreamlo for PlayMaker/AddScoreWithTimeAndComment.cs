using UnityEngine;
using System.Collections;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("Dreamlo")]
	[Tooltip("Add score to Dreamlo leaderboards")]
	public class AddScoreWithTimeAndComment : FsmStateAction
	{
		[RequiredField]
		[Tooltip("The players username")]
		public FsmString username;

		[RequiredField]
		[Tooltip("The score to submit")]
		public FsmInt score;

		[RequiredField]
		[Tooltip("The time to submit")]
		public FsmInt seconds;

		[RequiredField]
		[Tooltip("The comment to submit")]
		public FsmString comment;


		public override void OnEnter()
		{
			dreamloLeaderBoard.GetSceneDreamloLeaderboard ().AddScore (username.Value, score.Value, seconds.Value, comment.Value, OnFinish);
		}

		public void OnFinish() {
			Finish();
		}
	}
}