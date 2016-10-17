using UnityEngine;
using System.Collections;

public class SunEvents : MonoBehaviour
{
    public static bool isSun = false;
	public delegate void SunHandler(string name);

    public static event SunHandler OnSunUp;
    public static event SunHandler OnSunDown;


	public static void SunUp(string name)
    {

        if (OnSunUp != null)
        {
            OnSunUp(name);
        }
    }
	public static void SunDown(string name)
    {

        if (OnSunDown != null)
        {
            OnSunDown(name);
        }
    }
    public void OnGUI() {

        if (GUI.Button(new Rect(20, 20, 200, 30), "SUN")) {

            if (isSun)
            {
                isSun = false;
	            SunDown("2");
            }
            else
            {
                isSun = true;
	            SunUp("1");

            }

        }
        if (GUI.Button(new Rect(20, 50, 200, 30), "Noti"))
        {
            // Post Notification with data:
            Hashtable hashtable = new Hashtable();
            hashtable.Add("attribute1", 1000);
            hashtable.Add("attribute2", 55);
            NotificationCenter.DefaultCenter.PostNotification(this, "OnBumperCollision" ,hashtable);

        }
    }
	public string Name(string name){
		
		return name;
		
	}
}
