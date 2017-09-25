#pragma strict

// This script will extend the editor window for SFB_AudioClipArrayCombiner making it prettier and a bit more easy to use.

@CustomEditor (SFB_AudioClipArrayCombiner)							// Will attach this to the inspector window of this script
public class SFB_AudioClipArrayCombinerEditor extends Editor
{
    function OnInspectorGUI()
    {
    	var titleTexture : Texture = Resources.Load("titleAudioClipCombiner");
    	var myScript : SFB_AudioClipArrayCombiner = target as SFB_AudioClipArrayCombiner;							// A reference to the script

    	GUILayout.Label(titleTexture, GUILayout.Width(Screen.width - 50), GUILayout.Height((Screen.width) * .15));


    	DrawDefaultInspector();	

    	GUI.color = Color.green;
        if(GUILayout.Button("Export Clip Combinations"))									// If this button is clicked...
            myScript.SaveNow();															// Call the function
		GUI.color = Color.white;

    	/*
    	if (myScript.outputName == "" || myScript.outputName == null)												// If outputName is null or empty...
    		myScript.outputName = "Combined Clips";																	// Make it "Custom Export" by default
    	myScript.outputName = EditorGUILayout.TextField("Output Name: ", myScript.outputName);						// Custom Name field
    	var newLength : int = EditorGUILayout.IntField("Clip Layers: ", myScript.audioLayers.Length);			// Number of layers (array entries)
    	if (newLength != myScript.audioLayers.Length)
    		myScript.NewLength(newLength);
    	for (var i : int; i < myScript.audioLayers.Length; i++){
    		myScript.audioLayers[i].expanded = EditorGUILayout.Foldout(myScript.audioLayers[i].expanded, myScript.audioLayers[i].name);
    		if (myScript.audioLayers[i].expanded)
    		{
    			myScript.outputName = EditorGUILayout.TextField("Output Name: ", myScript.outputName);
    		}
    	}


        if(GUILayout.Button("Export Clips"))						// If we click the button with the name "Copy Settings"
        {
            myScript.SaveNow();										// Call the function
        }



		// In general users will likely not care about the details.  For those intersted in whats going on under
        // the hood, we allow them to toggle the defualt inspector data on or off.
		myScript.displayInspector = EditorGUILayout.Toggle("Show Script Data", myScript.displayInspector);
		if (myScript.displayInspector)
        	DrawDefaultInspector();																// Draws the default Inspector look from the target script
        */
	}
}