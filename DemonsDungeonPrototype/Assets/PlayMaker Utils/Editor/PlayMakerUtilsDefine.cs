using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

using HutongGames.PlayMakerEditor;



/// <summary>
/// Adds Playmaker Utils defines to project
/// Other tools can now use #if PLAYMAKER_UTILS or PLAYMAKER_UTILS_X_X_OR_NEWER
/// </summary>
[InitializeOnLoad]
public class PlayMakerUtilsDefines
{
	static PlayMakerUtilsDefines()
	{
		PlayMakerDefines.AddScriptingDefineSymbolToAllTargets("PLAYMAKER_UTILS");
		PlayMakerDefines.AddScriptingDefineSymbolToAllTargets("PLAYMAKER_UTILS_1_4_OR_NEWER");
	}

}