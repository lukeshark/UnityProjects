using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class dreamloLeaderBoard : MonoBehaviour {
	
	string dreamloWebserviceURL = "http://dreamlo.com/lb/";
	
	public string privateCode = "";
	public string publicCode = "";
	
	string highScores = "";
	
	////////////////////////////////////////////////////////////////////////////////////////////////
	
	// A player named Carmine got a score of 100. If the same name is added twice, we use the higher score.
 	// http://dreamlo.com/lb/(your super secret very long code)/add/Carmine/100

	// A player named Carmine got a score of 1000 in 90 seconds.
 	// http://dreamlo.com/lb/(your super secret very long code)/add/Carmine/1000/90
	
	// A player named Carmine got a score of 1000 in 90 seconds and is Awesome.
 	// http://dreamlo.com/lb/(your super secret very long code)/add/Carmine/1000/90/Awesome
	
	////////////////////////////////////////////////////////////////////////////////////////////////
	
	
	public struct Score {
		public string playerName;
		public int score;
		public int seconds;
		public string shortText;
		public string dateString;
	}
	
	void Start()
	{
		this.highScores = "";
	}
	
	public static dreamloLeaderBoard GetSceneDreamloLeaderboard()
	{
		GameObject go = GameObject.Find("dreamloPrefab");
		
		if (go == null) 
		{
			return null;
		}
		return go.GetComponent<dreamloLeaderBoard>();
	}


	public void AddScore(string playerName, int totalScore, Action onFinish)
	{
		StartCoroutine(AddScoreWithPipe(playerName, totalScore, onFinish));
	}
	
	public void AddScore(string playerName, int totalScore, int totalSeconds, Action onFinish)
	{
		StartCoroutine(AddScoreWithPipe(playerName, totalScore, totalSeconds, onFinish));
	}
	

	public void AddScore(string playerName, int totalScore, int totalSeconds, string shortText, Action onFinish)
	{
		StartCoroutine(AddScoreWithPipe(playerName, totalScore, totalSeconds, shortText, onFinish));
	}
	
	// This function saves a trip to the server. Adds the score and retrieves results in one trip.
	IEnumerator AddScoreWithPipe(string playerName, int totalScore, Action onFinish)
	{
		playerName = Clean(playerName);
		
		WWW www = new WWW(dreamloWebserviceURL + privateCode + "/add-pipe/" + WWW.EscapeURL(playerName) + "/" + totalScore.ToString());
		yield return www;
		highScores = www.text;

		Debug.Log (highScores);
		onFinish();
	}
	
	IEnumerator AddScoreWithPipe(string playerName, int totalScore, int totalSeconds, Action onFinish)
	{
		playerName = Clean(playerName);
		
		WWW www = new WWW(dreamloWebserviceURL + privateCode + "/add-pipe/" + WWW.EscapeURL(playerName) + "/" + totalScore.ToString()+ "/" + totalSeconds.ToString());
		yield return www;
		highScores = www.text;
		onFinish();
	}
	
	IEnumerator AddScoreWithPipe(string playerName, int totalScore, int totalSeconds, string shortText, Action onFinish)
	{
		playerName = Clean(playerName);
		shortText = Clean(shortText);
		
		WWW www = new WWW(dreamloWebserviceURL + privateCode + "/add-pipe/" + WWW.EscapeURL(playerName) + "/" + totalScore.ToString() + "/" + totalSeconds.ToString()+ "/" + shortText);
		yield return www;
		highScores = www.text;
		onFinish();
	}
	
	IEnumerator GetScores()
	{
		highScores = "";
		WWW www = new WWW(dreamloWebserviceURL +  publicCode  + "/pipe");
		yield return www;
		highScores = www.text;
	}

	IEnumerator GetScores(Action<string> onFinish)
	{
		highScores = "";
		WWW www = new WWW(dreamloWebserviceURL +  publicCode  + "/pipe");
		yield return www;
		highScores = www.text;
		onFinish(www.text);
	}
	
	IEnumerator GetSingleScore(string playerName)
	{
		highScores = "";
		WWW www = new WWW(dreamloWebserviceURL +  publicCode  + "/pipe-get/" + WWW.EscapeURL(playerName));
		yield return www;
		highScores = www.text;
	}

	IEnumerator GetSingleScore(string playerName, Action<string> onFinish)
	{
		highScores = "";
		WWW www = new WWW(dreamloWebserviceURL +  publicCode  + "/pipe-get/" + WWW.EscapeURL(playerName));
		yield return www;
		highScores = www.text;

		if(onFinish != null)
			onFinish(www.text);
	}
	
	public void LoadScores()
	{
		StartCoroutine(GetScores());
	}

	public void LoadScores(Action<string> onFinish)
	{
		StartCoroutine(GetScores(onFinish));
	}

	public void LoadScore(string playerName, Action<string> onFinish)
	{
		StartCoroutine(GetSingleScore(playerName, onFinish));
	}

	
	public string[] ToStringArray()
	{
		if (this.highScores == null) return null;
		if (this.highScores == "") return null;
		
		string[] rows = this.highScores.Split(new char[] {'\n'}, System.StringSplitOptions.RemoveEmptyEntries);
		return rows;
	}
	

	public List<Score> ToListLowToHigh()
	{
		Score[] scoreList = this.ToScoreArray();
		
		if (scoreList == null) return new List<Score>();
		
		List<Score> genericList = new List<Score>(scoreList);
			
		genericList.Sort((x, y) => x.score.CompareTo(y.score));
		
		return genericList;
	}
	
	public List<Score> ToListHighToLow()
	{
		Score[] scoreList = this.ToScoreArray();
		
		if (scoreList == null) return new List<Score>();

		List<Score> genericList = new List<Score>(scoreList);
			
		genericList.Sort((x, y) => y.score.CompareTo(x.score));
		
		return genericList;
	}
	
	public Score[] ToScoreArray()
	{
		string[] rows = ToStringArray();
		if (rows == null) return null;
		
		int rowcount = rows.Length;
		
		if (rowcount <= 0) return null;
		
		Score[] scoreList = new Score[rowcount];
		
		for (int i = 0; i < rowcount; i++)
		{
			string[] values = rows[i].Split(new char[] {'|'}, System.StringSplitOptions.None);
			
			Score current = new Score();
			current.playerName = values[0];
			current.score = 0;
			current.seconds = 0;
			current.shortText = "";
			current.dateString = "";
			if (values.Length > 1) current.score = CheckInt(values[1]);
			if (values.Length > 2) current.seconds = CheckInt(values[2]);
			if (values.Length > 3) current.shortText = values[3];
			if (values.Length > 4) current.dateString = values[4];
			scoreList[i] = current;
		}
		
		return scoreList;
	}
	
	
	
	// Keep pipe and slash out of names
	
	string Clean(string s)
	{
		s = s.Replace("/", "");
		s = s.Replace("|", "");
		return s;
		
	}
	
	int CheckInt(string s)
	{
		int x = 0;
		
		int.TryParse(s, out x);
		return x;
	}
	
}
