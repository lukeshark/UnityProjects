using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace HutongGames.PlayMakerEditor
{
    /// <summary>
    /// Adds Playmaker defines to project
    /// Other tools can now use #if PLAYMAKER
    /// Package as source code so user can remove or modify
    /// </summary>
    [InitializeOnLoad]
    public class PlayMakerDefines
    {
        static PlayMakerDefines()
        {
            AddScriptingDefineSymbolToAllTargets("PLAYMAKER");

            AddScriptingDefineSymbolToAllTargets("PLAYMAKER_1_8");
            AddScriptingDefineSymbolToAllTargets("PLAYMAKER_1_8_5");
            AddScriptingDefineSymbolToAllTargets("PLAYMAKER_1_8_5_OR_NEWER");
            AddScriptingDefineSymbolToAllTargets("PLAYMAKER_1_8_OR_NEWER");
            
            RemoveScriptingDefineSymbolFromAllTargets("PLAYMAKER_1_8_0");
            RemoveScriptingDefineSymbolFromAllTargets("PLAYMAKER_1_8_1");
            RemoveScriptingDefineSymbolFromAllTargets("PLAYMAKER_1_8_2");
            RemoveScriptingDefineSymbolFromAllTargets("PLAYMAKER_1_8_3");
            RemoveScriptingDefineSymbolFromAllTargets("PLAYMAKER_1_8_4");
        }

        public static void AddScriptingDefineSymbolToAllTargets(string defineSymbol)
        {
            foreach (BuildTargetGroup group in Enum.GetValues(typeof(BuildTargetGroup)))
            {
                if (!IsValidBuildTargetGroup(group)) continue;

                var defineSymbols = PlayerSettings.GetScriptingDefineSymbolsForGroup(group).Split(';').Select(d => d.Trim()).ToList();
                if (!defineSymbols.Contains(defineSymbol))
                {
                    defineSymbols.Add(defineSymbol);
                    try
                    {
                        PlayerSettings.SetScriptingDefineSymbolsForGroup(group, string.Join(";", defineSymbols.ToArray()));
                    }
                    catch (Exception)
                    {
                        Debug.Log("Could not set PLAYMAKER defines for build target group: " + group);
                        throw;
                    }
                    
                }
            }
        }

        public static void RemoveScriptingDefineSymbolFromAllTargets(string defineSymbol)
        {
            foreach (BuildTargetGroup group in Enum.GetValues(typeof(BuildTargetGroup)))
            {
                if (!IsValidBuildTargetGroup(group)) continue;

                var defineSymbols = PlayerSettings.GetScriptingDefineSymbolsForGroup(group).Split(';').Select(d => d.Trim()).ToList();
                if (defineSymbols.Contains(defineSymbol))
                {
                    defineSymbols.Remove(defineSymbol);
                    PlayerSettings.SetScriptingDefineSymbolsForGroup(group, string.Join(";", defineSymbols.ToArray()));
                }
            }
        }

        private static bool IsValidBuildTargetGroup(BuildTargetGroup group)
        {
            if (group == BuildTargetGroup.Unknown || IsObsolete(group)) return false;

            // Checking Obsolete attribute should be enough, 
            // but sometimes Unity versions are missing attributes
            // so keeping these checks around just in case:

#if UNITY_5_3_0 // Unity 5.3.0 had tvOS in enum but throws error if used
            if ((int)(object)group == 25) return false;
#endif

#if UNITY_5_4 || UNITY_5_5 // Unity 5.4+ doesn't like Wp8 and Blackberry any more
            if ((int)(object)group == 15) return false;
            if ((int)(object)group == 16) return false;
#endif

#if UNITY_5_6 // Unity 5.6+ removed build target 27?
            if ((int)(object)group == 27) return false;
#endif

            return true;
        }

        private static bool IsObsolete(Enum value)
        {
            var enumInt = (int)(object)value;
            if (enumInt == 4 || enumInt == 14) return false;

            var field = value.GetType().GetField(value.ToString());
            var attributes = (ObsoleteAttribute[])field.GetCustomAttributes(typeof(ObsoleteAttribute), false);
            return attributes.Length > 0;
        }

        /* NOTE: IsObsolete is complicated by the definition of BuildTargetGroup enum. 
         * E.g., in Unity 5.4:
         * 
          public enum BuildTargetGroup
          {
            Unknown = 0,
            Standalone = 1,
            [Obsolete("WebPlayer was removed in 5.4, consider using WebGL")] WebPlayer = 2,
            iOS = 4,
            [Obsolete("Use iOS instead (UnityUpgradable) -> iOS", true)] iPhone = 4,
            PS3 = 5,
            XBOX360 = 6,
            Android = 7,
            WebGL = 13,
            [Obsolete("Use WSA instead")] Metro = 14,
            WSA = 14,
            [Obsolete("Use WSA instead")] WP8 = 15,
            [Obsolete("BlackBerry has been removed as of 5.4")] BlackBerry = 16,
            Tizen = 17,
            PSP2 = 18,
            PS4 = 19,
            PSM = 20,
            XboxOne = 21,
            SamsungTV = 22,
            Nintendo3DS = 23,
            WiiU = 24,
            tvOS = 25,
          }
  
         */
    }
}

