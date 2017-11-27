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
	public class DreamloGetScoreToString : FsmStateAction
	
    {
		public enum SaveType
		{
			xml,
			json,
			pipe,
			quote
		}
		
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
		
		[ActionSection("Data")]
		
		[Tooltip("saves your list into this string)")]
		[UIHint(UIHint.Variable)]
		public FsmString highscoreList;
		
		[Tooltip("choose what type you wish to save into)")]
		public SaveType saveType;
		
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
			saveType = SaveType.xml;
			sortType = SortType.descending;
			sortBy = SortBy.score;
			publicCode = null;
			highscoreList = null;
			ListSize = null;
			errorMessage = "";
        }

        public override void OnEnter()
        {
			switch (saveType)
			{
				case SaveType.xml:
				setSaveType = "xml";
				break;
				
				case SaveType.json:
				setSaveType = "json";
				break;
				
				case SaveType.pipe:
				setSaveType = "pipe";
				break;
				
				case SaveType.quote:
				setSaveType = "quote";
				break;
			}
		
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
				highscoreList.Value = www.text;
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