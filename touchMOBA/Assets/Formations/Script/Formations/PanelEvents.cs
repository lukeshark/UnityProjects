using UnityEngine;
using System.Collections;

public class PanelEvents : MonoBehaviour {

    void Awake() {

	    SunEvents.OnSunUp += SunEvents_OnSunUp;
	    SunEvents.OnSunDown += SunEvents_OnSunDown;
    }

	private void SunEvents_OnSunDown(string name)
    {
	    Debug.Log("SunEvents_OnSunDown" +  name);
    }

	private void SunEvents_OnSunUp(string name)
    {
	    Debug.Log("SunEvents_OnSunUp" + name);
    }

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
