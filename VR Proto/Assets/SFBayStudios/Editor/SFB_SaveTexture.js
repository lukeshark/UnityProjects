#pragma strict

@MenuItem ("Window/Save Texture %#t")
static function SaveTexture () {
	if (Selection.activeObject)
	{
		if (Selection.activeObject.GetType() == UnityEditor.SubstanceArchive)
		{
			// Check to make sure other script is in the right spot.
			//if (AssetDatabase.ValidateMoveAsset("Assets/Standard Assets/Editor/SFB_Exporter.cs", "Assets/Standard Assets/Editor/SFB_Exporter.cs") != "Trying to move asset to location it came from Assets/Standard Assets/Editor/SFB_Exporter.cs")
			//	EditorUtility.DisplayDialog("SFB_Exporter.cs is not in the right place!", "Please move SFB_Exporter.cs into Assets/Stanard Assets/Editor/", "Got it!");
			//else
			//{
				var substance 				: SubstanceArchive		= Selection.activeObject as SubstanceArchive;
				var substancePath			: String				= AssetDatabase.GetAssetPath( substance.GetInstanceID() );
				var substanceImporter		: SubstanceImporter		= AssetImporter.GetAtPath( substancePath ) as SubstanceImporter;
				var substanceMaterialCount	: int					= substanceImporter.GetMaterialCount();
				var substanceMaterials		: ProceduralMaterial[]	= substanceImporter.GetMaterials();
				var basePath				: String				= substancePath.Replace( "/" + substance.name + ".sbsar", "" );
				var code 					: String 				= System.DateTime.Now.ToString("yy_MM_dd_hh_mm_ss");
				AssetDatabase.CreateFolder(basePath, substance.name + "_" + code);
				AssetDatabase.ImportAsset(basePath + "/" + substance.name + "_" + code);
				SFB_Exporter.SFB_Export(substanceImporter, substanceMaterials, basePath, substance, code);
			//}
		}
	}
}

    @MenuItem ("Window/Save Texture", true)
    static function ValidateSaveTexture() {
        return Selection.activeObject.GetType() == UnityEditor.SubstanceArchive;
    }

