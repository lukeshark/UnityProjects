using UnityEngine;
using System.Collections;
using UnityEditor;

[InitializeOnLoad]
public static class LayerUtils
{
	static LayerUtils()
	{
		CreateLayer();
	}
   
    static void CreateLayer()
    {
       //  https://forum.unity3d.com/threads/adding-layer-by-script.41970/reply?quote=2274824
	    SerializedObject tagManager = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset")[0]);
	    SerializedProperty layers = tagManager.FindProperty("layers");
	    bool ExistLayer = false;
	    //Debug.Log(ExistLayer) ;
	    for (int i = 8; i < layers.arraySize; i++)
	    {
		    SerializedProperty layerSP = layers.GetArrayElementAtIndex(i);
		    
        	//Debug.Log(layerSP.stringValue);
		    if (layerSP.stringValue == "Shootable")
		    {
			    ExistLayer = true;
			    break;
		    }
		    
	    }
	    for (int j = 8; j < layers.arraySize; j++)
	    {
		    SerializedProperty layerSP = layers.GetArrayElementAtIndex(j);
		    if (layerSP.stringValue == "" && !ExistLayer)
		    {
			    layerSP.stringValue = "Shootable";
			    tagManager.ApplyModifiedProperties();
			    
			    break;
		    }
	    }
	    
	    //Debug.Log(layers.arraySize);
    }
}
