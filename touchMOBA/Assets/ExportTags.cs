using UnityEngine;
using System.Collections;
using UnityEditor;

public class ExportTags : MonoBehaviour {

    [MenuItem("Asset Store Tools/Export package with tags and physics layers")]
    public static void ExportPackage()
    {
        string[] projectContent = new string[] { "Assets", "ProjectSettings/TagManager.asset" };
        AssetDatabase.ExportPackage(projectContent, "Assets/TagManager.unitypackage", ExportPackageOptions.Interactive | ExportPackageOptions.Recurse | ExportPackageOptions.IncludeDependencies);
        Debug.Log("Project Exported");
    }
}
