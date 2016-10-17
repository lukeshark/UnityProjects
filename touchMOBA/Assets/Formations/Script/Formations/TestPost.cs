using UnityEngine;
using System.Collections;

public class TestPost : MonoBehaviour
{

	void OnGUI ()
	{
		if (GUI.Button (new Rect (20, 100, 200, 30), "test")) {
			// Post Notification with data:
			Hashtable _hashtable = new Hashtable ();
			_hashtable.Add ("test1", "Hello!");
			_hashtable.Add ("test2", 123);
			NotificationCenter.DefaultCenter.PostNotification (this, "AgentInPosition", _hashtable);
			NotificationCenter.DefaultCenter.PostNotification (this, "AgentInPosition");
		}
	}
}
