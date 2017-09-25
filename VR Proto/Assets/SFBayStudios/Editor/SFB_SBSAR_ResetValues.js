#pragma strict
public class SFB_SBSAR_ResetValues extends EditorWindow {
	static var SFB_SBSAR_totalObjects		: float		= 0.0;		// Total objects selected
	static var SFB_SBSAR_currentObject		: float		= 0.0;		// Current object count we're on
	static var SFB_SBSAR_currentName		: String;				// Name of the current object
	static var SFB_SBSAR_currentPercent		: float		= 0.0;		// Percent complete
	static var SFB_SBSAR_allComplete		: boolean	= true;		// Are we completely done?

	// Add menu item to the Window menu
	@MenuItem ("Window/SFBayStudios/Reset Substance(s)")
	static function Init () {
		SFB_SBSAR_totalObjects	= Selection.objects.Length;				// Set total objects
		SFB_SBSAR_currentObject	= 0;									// Set current object
		SFB_SBSAR_allComplete	= false;								// Set not complete

		// For each selected object, if it's a .sbsar file, reset the value
		for (var x : int = 0; x < Selection.objects.Length; x++)
		{ 
			SFB_SBSAR_currentName			= Selection.objects[x].name;							// Set the currentName for progress bar display
			SFB_SBSAR_currentObject++;																// Increase current object count
			SFB_SBSAR_currentPercent		= SFB_SBSAR_currentObject / SFB_SBSAR_totalObjects;

			EditorUtility.DisplayProgressBar("Resetting Substances", "(" + SFB_SBSAR_currentObject + " of " + SFB_SBSAR_totalObjects + ") Working on " + SFB_SBSAR_currentName, SFB_SBSAR_currentPercent);

			// If the object type is correct...
			if (Selection.objects[x].GetType() == UnityEditor.SubstanceArchive)
			{
				var substance										= Selection.objects[x];												// Simpler variable
				var substancePath			: String				= AssetDatabase.GetAssetPath( substance.GetInstanceID() );			// Path to the file
				var substanceImporter		: SubstanceImporter		= AssetImporter.GetAtPath( substancePath ) as SubstanceImporter;	// SubstanceImporter object with the selected object
				var substanceMaterials		: ProceduralMaterial[]	= substanceImporter.GetMaterials();									// Array of materials inside the object
				// For each material, reset it
				for (var s : int; s < substanceMaterials.Length; s++){
					Debug.Log("Resetting " + substanceMaterials[s].name);		// Console confirmation
					substanceImporter.ResetMaterial(substanceMaterials[s]);		// Actual reset
				}
			}
		}
		SFB_SBSAR_allComplete	= true;		// This is set at the end.
		EditorUtility.ClearProgressBar();
	}
}