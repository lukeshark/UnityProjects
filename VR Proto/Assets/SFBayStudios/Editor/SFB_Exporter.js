import UnityEditorInternal;
import System.Collections.Generic;
import System.IO;
import System.Reflection;

public class SFB_Exporter{

	public static function SFB_Export_One(substanceImporter : SubstanceImporter, substanceMaterial : ProceduralMaterial) : void {
		var substanceImporterType : System.Type = SubstanceImporter;
		var exportBitmaps : MethodInfo = substanceImporterType.GetMethod ("ExportBitmaps", BindingFlags.Instance | BindingFlags.NonPublic);
		exportBitmaps.Invoke( substanceImporter, [ substanceMaterial ] );
	}
	
	public static function SFB_Export (substanceImporter : SubstanceImporter, substanceMaterials : ProceduralMaterial[], basePath : String, substance : SubstanceArchive, code : String) : void {
		var substanceImporterType : System.Type = SubstanceImporter;
		var exportBitmaps : MethodInfo = substanceImporterType.GetMethod( "ExportBitmaps", BindingFlags.Instance | BindingFlags.NonPublic );

		if ( ! Directory.Exists( "EXPORT_HERE" ) ) {
			Directory.CreateDirectory( "EXPORT_HERE" );
		}

		for (var substanceMaterial : ProceduralMaterial in substanceMaterials) {
			var generateAllOutputs : boolean = substanceImporter.GetGenerateAllOutputs (substanceMaterial);
			
			if (! Directory.Exists (basePath + "/" + substance.name + "_" + code + "/" + substanceMaterial.name)) {
				AssetDatabase.CreateFolder (basePath + "/" + substance.name + "_" + code, substanceMaterial.name);
				
				AssetDatabase.ImportAsset (basePath + "/" + substance.name + "_" + code + "/" + substanceMaterial.name);
			}
			
			var materialPath : String = basePath + "/" + substance.name + "_" + code + "/" + substanceMaterial.name + "/";
			var newMaterial : Material = new Material (substanceMaterial.shader);
			newMaterial.CopyPropertiesFromMaterial (substanceMaterial);
			AssetDatabase.CreateAsset (newMaterial, materialPath + substanceMaterial.name + ".mat");
			AssetDatabase.ImportAsset (materialPath + substanceMaterial.name + ".mat");
			substanceImporter.SetGenerateAllOutputs (substanceMaterial, true);
			exportBitmaps.Invoke (substanceImporter, [ substanceMaterial ]);
			
			if (! generateAllOutputs) {
				substanceImporter.SetGenerateAllOutputs (substanceMaterial, false);
			}
			
			var exportedTextures : String[] = Directory.GetFiles ("EXPORT_HERE");

			if (exportedTextures.Length > 0) {
				if (exportedTextures.Length > 0)
					for (var exportedTexture : String in exportedTextures) {
						File.Move (exportedTexture, materialPath + exportedTexture.Replace ("EXPORT_HERE", ""));
					}
				
				AssetDatabase.Refresh ();
				
				var propertyCount : int = ShaderUtil.GetPropertyCount (newMaterial.shader);
				var materialTextures : Texture[] = substanceMaterial.GetGeneratedTextures ();
				
				if ((materialTextures.Length <= 0) || (propertyCount <= 0)) {
					continue;
				}
				
				for (var materialTexture : ProceduralTexture in materialTextures) {
					var newTexturePath : String = materialPath + materialTexture.name + ".tga";		
					var newTextureAsset : Texture = AssetDatabase.LoadAssetAtPath (newTexturePath, typeof(Texture)) as Texture;

					for (var i : int = 0; i < propertyCount; i++) {
						if (ShaderUtil.GetPropertyType (newMaterial.shader, i) == ShaderUtil.ShaderPropertyType.TexEnv) {
							var propertyName : String = ShaderUtil.GetPropertyName (newMaterial.shader, i);
							if (newMaterial.GetTexture (propertyName) != null
								&& newMaterial.GetTexture (propertyName).name == newTextureAsset.name) {
								//Debug.Log ("Assigning: " + newTextureAsset.name + " | " + propertyName);
								newMaterial.SetTexture (propertyName, newTextureAsset);
							}
						}
					}
					if (materialTexture.GetProceduralOutputType () == ProceduralOutputType.Normal) {
						var textureImporter : TextureImporter = AssetImporter.GetAtPath (newTexturePath) as TextureImporter;
						
						textureImporter.textureType = TextureImporterType.NormalMap;
						
						AssetDatabase.ImportAsset (newTexturePath);
					}
				}
			} else {
				if (Directory.Exists (basePath + "/" + substance.name + "_" + code))
					AssetDatabase.DeleteAsset(basePath + "/" + substance.name + "_" + code);
				EditorUtility.DisplayDialog ("You must choose \"EXPORT_HERE\"", "Please make sure you select the \"EXPORT_HERE\" directory, at the same level in your project as the main Assets folder.", "Got it!");
			}
		}
		if ( Directory.Exists( "EXPORT_HERE" ) ) {
			Directory.Delete( "EXPORT_HERE" );
		}
	}

}