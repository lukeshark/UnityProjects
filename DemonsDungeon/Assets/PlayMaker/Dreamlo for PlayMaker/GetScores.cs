using UnityEngine;
using System.Collections;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("Dreamlo")]
	[Tooltip("Gets all the scores from Dreamlo leaderboards")]
	public class GetScores : FsmStateAction
	{
		[RequiredField]
		[UIHint(UIHint.Variable)]
		[Tooltip("Where to store the scores")]
		public FsmString scores;
		
		public override void OnEnter()
		{
			dreamloLeaderBoard.GetSceneDreamloLeaderboard ().LoadScores (OnFinish);
		}

		public void OnFinish(string dreamLoScores) {

			scores.Value = dreamLoScores;
			Finish();
		}
	}

}