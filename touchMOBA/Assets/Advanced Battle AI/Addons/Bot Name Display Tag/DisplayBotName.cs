/**
	THIS SHOWS YOU HOW TO GET A BOT'S NAME FROM THE IDENTITY FSM MODULE
	AND DISPLAY IT AS A NAME TAG ON TOP OF HIS CHARACTER.

	Download free or purchase more add-ons from our website:
	www.AIBotSystem.com

**/

using UnityEngine;
using System.Collections;


//[RequireComponent (typeof (Collider))]
public class DisplayBotName : MonoBehaviour
{ 
	public Color displayColor = new Color (0.0f, 1.0f, 0.0f, 1.0f);
	private string botDisplayName = "<< Bot >>";	// default bot name, if the bot has no name
	public GUISkin customSkin = null;
	private string styleName = "Box";
	public Camera guiCamera = null;
	public float fadeDistance = 30.0f, hideDistance = 35.0f;
	private float maxViewAngle = 60.0f;	
	public bool autoUpdate = false;

	// fsms
  	private PlayMakerFSM[] botFSMArray;
	private PlayMakerFSM FSM_Identity; // this is the target FSM we want... retrieve Bot name from here

	private CapsuleCollider temp_collider;

	void Start (){

		if (guiCamera == null){
			guiCamera = Camera.main;
		}


		// get all FSMs of the bot:
		botFSMArray =  gameObject.GetComponents<PlayMakerFSM>();

		if (botFSMArray != null){
			foreach (PlayMakerFSM tempFSM in botFSMArray) {

				// Let's grab the FSM we want, and that is the "Identity" FSM (where the bot's name is stored)
				if (tempFSM.FsmName == "Identity"){
	
					FSM_Identity = tempFSM;

					// now let's get the name:
						botDisplayName = FSM_Identity.FsmVariables.GetFsmString("charSTAT_Name").Value;
					
						if (autoUpdate){
							InvokeRepeating("updateName", 1, 1);
						}						

				}
	        }
		}
	}


	void Reset ()
	// Fallback for the camera reference
	{
		if (guiCamera == null)
		{
			guiCamera = Camera.main;
			maxViewAngle = guiCamera.fieldOfView;
		}
	}
	
	
	public void SetLabel (string label)
	// Handle SetLabel messages sent to the GO
	{
		botDisplayName = label;
	}

	void updateName(){
		botDisplayName = FSM_Identity.FsmVariables.GetFsmString("charSTAT_Name").Value;
	}
	
	
	void OnGUI ()
	{
		
		temp_collider = gameObject.GetComponent<CapsuleCollider>();
		if (temp_collider != null){


					if (guiCamera != null){

							useGUILayout = false;
								// We're not using GUILayout, so don't spend processing on it
							
							if (Event.current.type != EventType.Repaint)
							// We are only interested in repaint events
							{
								return;
							}
							
							Vector3 worldPosition = GetComponent<Collider>().bounds.center + Vector3.up * GetComponent<Collider>().bounds.size.y * 0.4f;
								// Place the label on top of the collider
							float cameraDistance = (worldPosition - guiCamera.transform.position).magnitude;
							
							if (
								cameraDistance > hideDistance ||
								Vector3.Angle (
									guiCamera.transform.forward,
									worldPosition - guiCamera.transform.position
								) >
								maxViewAngle
							)
							// If the world position is outside of the field of view or further away than hideDistance, don't render the label
							{
								return;
							}
							

								GUI.color = displayColor;

							
							Vector2 position = guiCamera.WorldToScreenPoint (worldPosition);
							position = new Vector2 (position.x, Screen.height - position.y);
								// Get the GUI space position
								
							GUI.skin = customSkin;
								// Set the custom skin. If no custom skin is set (null), Unity will use the default skin
							
							string contents = string.IsNullOrEmpty (botDisplayName) ? gameObject.name : botDisplayName;
							
							Vector2 size = GUI.skin.GetStyle (styleName).CalcSize (new GUIContent (contents));
								// Get the content size with the selected style
							
							Rect rect = new Rect (position.x - size.x * 0.5f, position.y - size.y, size.x, size.y);
								// Construct a rect based on the calculated position and size
							
							GUI.skin.GetStyle (styleName).Draw (rect, contents, false, false, false, false);
								// Draw the label with the selected style
						}
		}

	}
}
