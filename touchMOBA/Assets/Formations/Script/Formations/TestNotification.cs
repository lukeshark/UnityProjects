using UnityEngine;
using System.Collections;

public class TestNotification : MonoBehaviour
{

	// Use this for initialization
	void Start ()
	{
		NotificationCenter.DefaultCenter.AddObserver (this, "AgentInPosition");
		NotificationCenter.DefaultCenter.RemoveObserver (this, "AgentInPosition");

		NotificationCenter.DefaultCenter.PostNotification (this, "AgentInPosition");
	}

	void AgentInPosition (NotificationCenter.Notification note)
	{
		// reuse data if it exists
		if (note.data != null) {
			Debug.Log ("Sender=" + note.sender.name);
			Debug.Log ("Data:");
			foreach (DictionaryEntry entry in note.data) {
				Debug.Log (" " + entry.Key + "=" + entry.Value);
			}
		} else {
			Debug.Log ("data is null.");
		}
	}
}
