using UnityEngine;
using System.Collections;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("Dreamlo")]
	[Tooltip("Add score to Dreamlo leaderboards")]
	public class AddScore : FsmStateAction
	{
		[RequiredField]
		[Tooltip("The players username")]
		public FsmString username;

		[RequiredField]
		[Tooltip("The score to submit")]
		public FsmInt score;

		public override void OnEnter()
		{
			dreamloLeaderBoard.GetSceneDreamloLeaderboard ().AddScore (username.Value, score.Value, OnFinish);
		}

		public void OnFinish() {
			Finish();
		}
	}
}