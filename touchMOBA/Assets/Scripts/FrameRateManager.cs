using UnityEngine;
 using System.Collections;
 
 public class FrameRateManager : MonoBehaviour {
	 
	 
	 void Awake () {
		 QualitySettings.vSyncCount = 2;  // VSync must be disabled
		 Application.targetFrameRate = 30;
	 }
 }