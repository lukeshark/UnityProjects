// (c) Copyright HutongGames, LLC 2010-2014. All rights reserved.

using System;
using System.Collections.Generic;

using UnityEngine;
using UnityEditor;

public class PlayMakerNavMeshMaskField {

	
	public List<string> layers;
	public List<int> layerNumbers;
	public string[] layerNames;
	public long lastUpdateTick;
	
	// http://answers.unity3d.com/questions/42996/how-to-create-layermask-field-in-a-custom-editorwi.html
	
	/** Displays a LayerMask field.
	 * \param showSpecial Use the Nothing and Everything selections
	 * \param selected Current LayerMask
	 * \version Unity 3.5 and up will use the EditorGUILayout.MaskField instead of a custom written one.
	 */
	public LayerMask LayerMaskField (string label, LayerMask selected, bool showSpecial) {
		
		//Unity 3.5 and up
		
		if (layers == null || (System.DateTime.Now.Ticks - lastUpdateTick > 10000000L && Event.current.type == EventType.Layout)) {
			lastUpdateTick = System.DateTime.Now.Ticks;
			if (layers == null) {
				layers = new List<string>();
				layerNumbers = new List<int>();
				layerNames = new string[4];
			} else {
				layers.Clear ();
				layerNumbers.Clear ();
			}
			
			string[] _layers = GameObjectUtility.GetNavMeshLayerNames();
			
			int emptyLayers = 0;
			for (int i=0;i<_layers.Length;i++) {
				string layerName = _layers[i];
				
				if (layerName != "") {
					
					for (;emptyLayers>0;emptyLayers--) layers.Add ("Layer "+(i-emptyLayers));
					layerNumbers.Add (i);
					layers.Add (layerName);
				} else {
					emptyLayers++;
				}
			}
			
			if (layerNames.Length != layers.Count) {
				layerNames = new string[layers.Count];
			}
			for (int i=0;i<layerNames.Length;i++) layerNames[i] = layers[i];
		}
		
		selected.value =  EditorGUILayout.MaskField (label,selected.value,layerNames);

		GUILayout.Label(" "+selected.value,GUILayout.ExpandWidth(false));

		return selected;
	}
	

}
