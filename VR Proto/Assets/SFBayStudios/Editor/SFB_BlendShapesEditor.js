#pragma strict

@CustomEditor (SFB_BlendShapes)							// Will attach this to the inspector window of v1_CopyThisToThat
public class SFB_BlendShapesEditor extends Editor
{
    function OnInspectorGUI()
    {
    	var myScript 	: SFB_BlendShapes	= target;		// A reference to the script
		

		EditorGUILayout.BeginHorizontal ();
		var newPresetObject;
		newPresetObject		= EditorGUILayout.ObjectField(newPresetObject, TextAsset, false, GUILayout.Height(30));
		EditorGUILayout.EndHorizontal ();

		if (newPresetObject)
		{
			myScript.presetObjects.Add(newPresetObject);
			newPresetObject	= null;
		}

		if (myScript.presetObjects.Count == 0)
			EditorGUILayout.HelpBox("Drop Preset Objects into the field above.", MessageType.Info);
		else
		{
			for (var p : int; p < myScript.presetObjects.Count; p++){
				EditorGUILayout.BeginHorizontal ();
				GUILayout.Label(myScript.presetObjects[p].name + " (" + p + ")");
				GUI.color 			= Color.green;
				if(GUILayout.Button("Load Preset", GUILayout.Width(100)))
		        {
		        	myScript.SFB_BS_ImportPresetFile(myScript.presetObjects[p]);
		        }
		        GUI.color 			= Color.red;
		        if(GUILayout.Button("Remove", GUILayout.Width(80)))
		        {
		        	myScript.SFB_BS_RemovePreset(p);
		        }
		        GUI.color 			= Color.white;
				EditorGUILayout.EndHorizontal ();
			}
		}




		EditorGUILayout.HelpBox("SCRIPTING INDIVIDUAL VALUES\nCall SetSelectedShape(shapeIDNumber : int) first\nCall SetValueUI(newValue : float) to set value\nID # is listed next to Blend Shape name\n\nLOADING PRESET FILES\nCall SFB_BS_ImportPreset(presetID : int) to load a preset file", MessageType.Info);


		EditorGUILayout.BeginHorizontal ();
			Undo.RecordObject (myScript, "Change Preset Name");
        	myScript.presetName = EditorGUILayout.TextField("Preset File Name: ", myScript.presetName);
		EditorGUILayout.EndHorizontal ();
		EditorGUILayout.BeginHorizontal ();
			GUI.color 			= Color.green;
			if(GUILayout.Button("Export Values as Preset", GUILayout.Width(150)))
	        {
	        	myScript.SFB_BS_ExportPreset();
	        }
	        GUI.color 			= Color.white;
	        GUILayout.Space (50);
			if(GUILayout.Button("Include All", GUILayout.Width(100)))
	        {
	        	myScript.SFB_BS_TogglePresets(true);
	        }
			if(GUILayout.Button("Exclude All", GUILayout.Width(100)))
	        {
	        	myScript.SFB_BS_TogglePresets(false);
	        }
        EditorGUILayout.EndHorizontal ();
		

		if (myScript.blendShapeObjects.Count > 0)
		{
			for (var o : int; o < myScript.blendShapeObjects.Count; o++){
				var blendShapeObject	: SFB_BlendShapeObject	= myScript.blendShapeObjects[o];
				var blendShapeMesh		: Mesh					= blendShapeObject.object;
				if (blendShapeObject.primaryShapes > 0)
				{
					myScript.blendShapeObjects[o].expandedInspector = EditorGUILayout.Foldout(myScript.blendShapeObjects[o].expandedInspector, blendShapeMesh.name);
					if (!myScript.blendShapeObjects[o].expandedInspector)
					{

					}
					else
					{
						if (myScript.blendShapeObjects[o].blendShapes.Count != 0)
						{
							//var blendShapeObject	: SFB_BlendShapeObject	= myScript.blendShapeObjects[o];
							//var blendShapeMesh		: Mesh					= blendShapeObject.object;
							//var hasShownGroup		: boolean				= false;
							for (var i : int; i < blendShapeObject.blendShapes.Count; i++){
								var blendShapeData	: SFB_BlendShape		= blendShapeObject.blendShapes[i];
								if (blendShapeData.isVisible)
								{
									/*if (!hasShownGroup)
									{
										hasShownGroup = true;
										GUILayout.Space (5);
										EditorGUILayout.BeginHorizontal ();
											GUILayout.Label ("--" + blendShapeMesh.name + "--");
										EditorGUILayout.EndHorizontal ();
									}*/
								 	EditorGUILayout.BeginHorizontal ();
								 		Undo.RecordObject (myScript, "Toggle Preset Export");
								 		blendShapeData.presetExport = EditorGUILayout.Toggle(blendShapeData.presetExport, GUILayout.Width(18));
								 		var displayName	= blendShapeData.name;
								 		if (blendShapeData.isPlus)
								 		{
								 			displayName	= displayName.Replace("Plus", "");
								 			displayName	= displayName.Replace("plus", "");
								 		}
						    			GUILayout.Label (displayName + " (" + blendShapeData.inspectorID + ")", GUILayout.Width(200));
						    			if (blendShapeData.isPlus)
						    			{
						    				var minusShapeObject	: int			= myScript.GetMinusShapeObject(blendShapeData.name);
						    				var minusShapeID		: int			= myScript.GetMinusShapeID(blendShapeData.name);
						    				var minusShapeData	: SFB_BlendShape	= myScript.blendShapeObjects[minusShapeObject].blendShapes[minusShapeID];
						    				var newValue 							= EditorGUILayout.Slider(blendShapeData.sliderValue,-100, 100);
						    				//Debug.Log ("Names: " + blendShapeData.name + " | " + minusShapeData.name);
											blendShapeData.sliderValue	= newValue;
											minusShapeData.sliderValue	= -newValue;
						    			}
						    			else
						    				blendShapeData.sliderValue = EditorGUILayout.Slider(blendShapeData.sliderValue,1, 100);
									EditorGUILayout.EndHorizontal ();

									if (blendShapeData.sliderValue != blendShapeData.value)
									{
										Undo.RecordObject (myScript, "Update Blendshape Value");
										myScript.SetValue(o, i, blendShapeData.id, blendShapeData.sliderValue);
										if (blendShapeData.isPlus)
										{
											Undo.RecordObject (myScript, "Update Blendshape Value");
											myScript.SetValue(minusShapeObject, minusShapeID, minusShapeData.id, minusShapeData.sliderValue);
										}
									}
								}
							}
						}
					}
				}
			}
		}
		GUI.color 			= Color.green;
        if(GUILayout.Button("Reload Blend Shapes"))				
        {
            myScript.ReloadBlendShapes();			
        }

        GUI.color 			= Color.white;
        if (myScript.wireframeHidden)
        {
	        if(GUILayout.Button("Turn On Wireframe"))				
	            myScript.SFB_BS_HideWireframe(false);			
		}
		else
		{
	        if(GUILayout.Button("Turn Off Wireframe"))				
	            myScript.SFB_BS_HideWireframe(true);			
		}

        GUI.color 			= Color.red;
        if(GUILayout.Button("Reset Values"))				
        {
            myScript.SFB_BS_ResetAll();						
        }
        GUI.color 			= Color.white;
        DrawDefaultInspector();								// Draw the normal inspector first
    }

}

