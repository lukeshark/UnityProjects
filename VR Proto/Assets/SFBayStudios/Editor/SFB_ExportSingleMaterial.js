#pragma strict

class ExportSingleMaterialWindow extends EditorWindow {
	//var exporterScript 			: SFB_massExporter;
	var groupName				: String;
	var removeHeight			: boolean	= true;
	var removeEmissive			: boolean	= true;
	var setNormalMapMode		: boolean	= true;					// If true, will make sure normal map is set to "Normal Map" mode

	var overrideNameEnabled		: boolean	= false;
	var overrideMaterialName	: String	= "Custom Material Name";

	var previousGroupEnabled	: boolean	= false;
	var previousGroupName		: String;
	var copyNormal				: boolean	= false;
	var copyAO					: boolean	= false;
	var copyMetalRough			: boolean	= false;
	var copyHeight				: boolean	= false;
	var copyEmissive			: boolean	= false;
	var selectedMaterial 		: ProceduralMaterial;
	var canExport				: boolean	= false;
	var mergeWithGroup			: boolean	= true;					// If false, will create a numerical new group if the group name conflicts.
	
	// Add menu named "My Window" to the Window menu
	@MenuItem ("Window/SFBayStudios/Export Single Material %#e")
	static function Init () {
		// Get existing open window or if none, make a new one:		
		var window = ScriptableObject.CreateInstance.<ExportSingleMaterialWindow>();

		if (Selection.activeObject)
		{
			if (Selection.activeObject.GetType() == UnityEngine.ProceduralMaterial)
			{
				window.selectedMaterial	= Selection.activeObject;
			}
		}

		if (EditorPrefs.HasKey("groupName"))
			window.groupName					= EditorPrefs.GetString("groupName");
		if (EditorPrefs.HasKey("previousGroupName"))
			window.previousGroupName			= EditorPrefs.GetString("previousGroupName");
		if (EditorPrefs.HasKey("previousGroupEnabled"))
			window.previousGroupEnabled			= EditorPrefs.GetBool("previousGroupEnabled");
		if (EditorPrefs.HasKey("removeEmissive"))
			window.removeEmissive				= EditorPrefs.GetBool("removeEmissive");
		if (EditorPrefs.HasKey("removeHeight"))
			window.removeHeight					= EditorPrefs.GetBool("removeHeight");
		if (EditorPrefs.HasKey("useNormalFromPrevious"))
			window.copyNormal					= EditorPrefs.GetBool("useNormalFromPrevious");
		if (EditorPrefs.HasKey("useAOFromPrevious"))
			window.copyAO						= EditorPrefs.GetBool("useAOFromPrevious");
		if (EditorPrefs.HasKey("useMetalRoughFromPrevious"))
			window.copyMetalRough				= EditorPrefs.GetBool("useMetalRoughFromPrevious");
		if (EditorPrefs.HasKey("useHeightFromPrevious"))
			window.copyHeight					= EditorPrefs.GetBool("useHeightFromPrevious");
		if (EditorPrefs.HasKey("useEmissiveFromPrevious"))
			window.copyEmissive					= EditorPrefs.GetBool("useEmissiveFromPrevious");
		if (EditorPrefs.HasKey("setNormalMapMode"))
			window.setNormalMapMode				= EditorPrefs.GetBool("setNormalMapMode");
		if (EditorPrefs.HasKey("mergeWithGroup"))
			window.mergeWithGroup				= EditorPrefs.GetBool("mergeWithGroup");
		if (EditorPrefs.HasKey("overrideMaterialName"))
			window.overrideMaterialName			= EditorPrefs.GetString("overrideMaterialName");
		if (EditorPrefs.HasKey("overrideNameEnabled"))
			window.overrideNameEnabled			= EditorPrefs.GetBool("overrideNameEnabled");

		window.Show();
	}
	
	function OnGUI () {
		GUILayout.Label ("Base Settings", EditorStyles.boldLabel);
			selectedMaterial		= EditorGUILayout.ObjectField(selectedMaterial, ProceduralMaterial, true);
			groupName				= EditorGUILayout.TextField ("Group Name", groupName);
			removeEmissive			= EditorGUILayout.Toggle ("Remove Emissive", removeEmissive);
			removeHeight			= EditorGUILayout.Toggle ("Remove Height", removeHeight);
			setNormalMapMode		= EditorGUILayout.Toggle ("Set Normal Map Mode", setNormalMapMode);
			mergeWithGroup			= EditorGUILayout.Toggle ("Merge With Group Name", mergeWithGroup);

			overrideNameEnabled		= EditorGUILayout.BeginToggleGroup ("Override Material Name?", overrideNameEnabled);
			overrideMaterialName	= EditorGUILayout.TextField ("Material Name", overrideMaterialName);
			EditorGUILayout.EndToggleGroup ();

			previousGroupEnabled	= EditorGUILayout.BeginToggleGroup ("Copy Previous Group?", previousGroupEnabled);
			previousGroupName		= EditorGUILayout.TextField ("Previous Group Name", previousGroupName);
			copyNormal				= EditorGUILayout.Toggle ("Use Normal From Previous", copyNormal);
			copyAO					= EditorGUILayout.Toggle ("Use AO From Previous", copyAO);
			copyMetalRough			= EditorGUILayout.Toggle ("Use Metal/Rough From Previous", copyMetalRough);
			copyHeight				= EditorGUILayout.Toggle ("Use Height From Previous", copyHeight);
			copyEmissive			= EditorGUILayout.Toggle ("Use Emissive From Previous", copyEmissive);
			EditorGUILayout.EndToggleGroup ();

			if(GUILayout.Button("Export Material") && selectedMaterial)				// If we click the button with the name "Copy Settings"
	        {
				Debug.Log ("Export Material!");
				var substancePath			: String				= AssetDatabase.GetAssetPath( selectedMaterial.GetInstanceID() );
				var substanceImporter		: SubstanceImporter		= AssetImporter.GetAtPath( substancePath ) as SubstanceImporter;

				var exporterScript : SFB_massExporter = SFB_massExporter();

				exporterScript.groupName					= groupName;
				exporterScript.previousGroupName			= previousGroupName;
				exporterScript.removeEmissive				= removeEmissive;
				exporterScript.removeHeight					= removeHeight;
				exporterScript.useNormalFromPrevious		= copyNormal;
				exporterScript.useAOFromPrevious			= copyAO;
				exporterScript.useMetalRoughFromPrevious	= copyMetalRough;
				exporterScript.useHeightFromPrevious		= copyHeight;
				exporterScript.useEmissiveFromPrevious		= copyEmissive;
				exporterScript.setNormalMapMode				= setNormalMapMode;
				exporterScript.mergeWithGroup				= mergeWithGroup;
				exporterScript.SetOverrideName(overrideNameEnabled, overrideMaterialName);

				EditorPrefs.SetString("groupName", groupName);
				EditorPrefs.SetString("previousGroupName", previousGroupName);
				EditorPrefs.SetBool("previousGroupEnabled", previousGroupEnabled);
				EditorPrefs.SetBool("removeEmissive", removeEmissive);
				EditorPrefs.SetBool("removeHeight", removeHeight);
				EditorPrefs.SetBool("useNormalFromPrevious", copyNormal);
				EditorPrefs.SetBool("useAOFromPrevious", copyAO);
				EditorPrefs.SetBool("useMetalRoughFromPrevious", copyMetalRough);
				EditorPrefs.SetBool("useHeightFromPrevious", copyHeight);
				EditorPrefs.SetBool("useEmissiveFromPrevious", copyEmissive);
				EditorPrefs.SetBool("setNormalMapMode", setNormalMapMode);
				EditorPrefs.SetBool("mergeWithGroup", mergeWithGroup);
				EditorPrefs.SetBool("overrideNameEnabled", overrideNameEnabled);
				EditorPrefs.SetString("overrideMaterialName", overrideMaterialName);

				exporterScript.ExportSubstance(substanceImporter, selectedMaterial);
	        }
	}
}