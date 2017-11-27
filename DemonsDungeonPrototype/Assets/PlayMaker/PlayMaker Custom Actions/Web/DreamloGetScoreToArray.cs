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
	public class DreamloGetScoreToArray : FsmStateAction
	
    {		
		public enum SortType
		{
			ascending,
			descending
		}
		
		public enum SortBy
		{
			score,
			seconds,
			date
		}
		[ActionSection("Code")]
		[RequiredField]
		[Tooltip("Place Public Code from the Dreamlo website")]
		public FsmString publicCode;
		
		[ActionSection("Arrays")]

        [UIHint(UIHint.Variable)] 
		[ArrayEditor(VariableType.String)]
        [Tooltip("Store the value in a String Array")] 
        public FsmArray nameList;
		
		[ActionSection("")]
		
        [UIHint(UIHint.Variable)]
		[ArrayEditor(VariableType.Int)]
        [Tooltip("Store the value in a Int Array")] 
        public FsmArray scoreListAsInt;
		
        [UIHint(UIHint.Variable)]
		[ArrayEditor(VariableType.String)]
        [Tooltip("Store the value in a String Array")] 
        public FsmArray scoreListAsString;
		
		[ActionSection("")]
		
        [UIHint(UIHint.Variable)] 
		[ArrayEditor(VariableType.Int)]
        [Tooltip("Store the value in a Int Array")] 
        public FsmArray secondsListAsInt;
		
        [UIHint(UIHint.Variable)] 
		[ArrayEditor(VariableType.String)]
        [Tooltip("Store the value in a String Array")] 
        public FsmArray secondsListAsString;
		
		[ActionSection("")]
		
        [UIHint(UIHint.Variable)]
		[ArrayEditor(VariableType.String)]
        [Tooltip("Store the value in a String Array")] 
        public FsmArray textList;
		
		[ActionSection("")]

        [UIHint(UIHint.Variable)]
		[ArrayEditor(VariableType.String)]
        [Tooltip("Store the value in a String Array")] 
        public FsmArray dateAndTimeList;
		
		[ActionSection("Sort")]
		
		[Tooltip("here you can choose the size of your list for example 10 will give the top 10 scores, leave to none or 0 to get all scores")]
		public FsmInt ListSize;
		
		public SortType sortType;
		public SortBy sortBy;

		[ActionSection("Error")]
		
		public FsmEvent errorEvent;
		[Tooltip("Where any errors thrown will be stored. Set this to a variable, or leave it blank.")]
		
		public FsmString errorMessage = "";
		
		private WWW www;
		
		
        private Coroutine routine;
		private string webUrl;
		private string setSaveType;

        public override void Reset()
        {


			publicCode = null;
			nameList = null;
			scoreListAsInt = null;
			scoreListAsString = null;
			secondsListAsInt = null;
			secondsListAsString = null;
			textList = null;
			dateAndTimeList = null;
			ListSize = new FsmInt() {UseVariable=true};
			sortType = SortType.descending;
			sortBy = SortBy.score;
			errorMessage = "";
        }

        public override void OnEnter()
        {
			
			switch (sortBy)
			{
				case SortBy.score:
				break;
				
				case SortBy.seconds:
				setSaveType = setSaveType + "-seconds";
				break;
			
				case SortBy.date:
				setSaveType = setSaveType + "-date";
				break;
			}
		
			switch (sortType)
			{
				case SortType.descending:
				break;
			
				case SortType.ascending:
				setSaveType = setSaveType + "-asc";
				break;
			}
				setSaveType = "pipe" + setSaveType;
			
			if (ListSize == null || ListSize.Value == 0) 
			{
				webUrl = "http://dreamlo.com/lb/" + publicCode.Value + "/" + setSaveType;
				
			}
			else
			{
			webUrl = "http://dreamlo.com/lb/" + publicCode.Value + "/" + setSaveType + "/" + ListSize;

			}			
			routine = StartCoroutine(UploadNewHighscore());
        }
		
        private IEnumerator UploadNewHighscore()
        {
			www = new WWW(webUrl);
            yield return www;
			
			if (string.IsNullOrEmpty(www.error))
			{
			FormatHighscores(www.text);
			}
			else
			{
				errorMessage = www.error;
				Fsm.Event(errorEvent);
			}

        }
		
		void FormatHighscores(string textStream) 
		{
			string[] entries = textStream.Split(new char[] {'\n'}, System.StringSplitOptions.RemoveEmptyEntries);

			for (int i = 0; i <entries.Length; i ++) 
			{

				string[] entryInfo = entries[i].Split(new char[] {'|'});
				string username = entryInfo[0];
				nameList.Resize(nameList.Length+1);
				nameList.Set(nameList.Length-1, username);
				

				int scoreInt = int.Parse(entryInfo[1]);
				scoreListAsInt.Resize(scoreListAsInt.Length+1);
				scoreListAsInt.Set(scoreListAsInt.Length-1, scoreInt);

				string score = entryInfo[1];
				scoreListAsString.Resize(scoreListAsString.Length+1);
				scoreListAsString.Set(scoreListAsString.Length-1, score);

				int secondsInt = int.Parse(entryInfo[2]);
				secondsListAsInt.Resize(secondsListAsInt.Length+1);
				secondsListAsInt.Set(secondsListAsInt.Length-1, secondsInt);					

				string seconds = entryInfo[2];
				secondsListAsString.Resize(secondsListAsString.Length+1);
				secondsListAsString.Set(secondsListAsString.Length-1, seconds);

				
				string text = entryInfo[3];
				textList.Resize(textList.Length+1);
				textList.Set(textList.Length-1, text);

				string dateAndTime = entryInfo[4];
				dateAndTimeList.Resize(dateAndTimeList.Length+1);
				dateAndTimeList.Set(dateAndTimeList.Length-1, dateAndTime);	
			}
			Finish();
		}
		        public override void OnExit()
        {
            StopCoroutine(routine);
        }
	}	
}