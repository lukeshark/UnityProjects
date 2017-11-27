using UnityEngine;
using System.Collections;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("Dreamlo")]
	[Tooltip("Gets a score from Dreamlo leaderboards")]
	public class GetSingleScore : FsmStateAction
	{
		[RequiredField]
		[UIHint(UIHint.Variable)]
		[Tooltip("Where to store the scores")]
		public FsmInt score;

		[RequiredField]
		[Tooltip("What username should be we get a score for")]
		public FsmString username;

		public override void OnEnter()
		{
			dreamloLeaderBoard.GetSceneDreamloLeaderboard ().LoadScore (username.Value, OnFinish);
		}

		public void OnFinish(string dreamLoScore) {

		
			string output = dreamLoScore.Split(new char[] { '|', '|' })[1];
			score.Value = int.Parse(output);
			Finish();
		}
	}

}