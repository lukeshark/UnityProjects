using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace HutongGames.PlayMaker.Ecosystem.Utils
{


	public class LinkerData : ScriptableObject
	{

		static Type[] _unlinkedTyped = new Type[]{
			typeof(UnityEngine.Collider2D)
		};


		public bool debug = true;
		public bool LinkContentUpdateDone = false;
		public TextAsset Asset;
		public string AssetPath;

		public LinkerData self;

		// TODO: make it serializable...
		public Dictionary<string,List<string>> linkerEntries = new Dictionary<string, List<string>>();


		static public bool DebugAll
		{
			get{
				if (instance==null)
				{
					return true;
				}
				return instance.debug;
			}
		}

		static private LinkerData _instance_;


		static public LinkerData instance
		{
			get{
				if (_instance_==null)
				{
					return null;
				}
				return _instance_.self;
			}

			set{
				_instance_ = value;
			}

		}

		public static void RegisterClassDependancy(Type type,string typeName)
		{

			if (type == null)
			{
				Debug.LogWarning("LinkerData RegisterClassDependancy with no type");
				return;
			}

			if (instance ==null)
			{
				Debug.LogWarning("LinkerData is missing an instance, please create one first in your assets from the create menu: Assets/Create/PlayMaker/Create Linker Wizard");
				return;
			}

			//Check for unlinked dependancy...
			foreach(Type _type in _unlinkedTyped)
			{
				if (type.IsSubclassOf(_type))
				{
					instance.RegisterLinkerEntry(_type.Namespace,_type.AssemblyQualifiedName);
				}
			}

			instance.RegisterLinkerEntry(type.Assembly.FullName,typeName);
		}

		public static void RegisterClassDependancy(string assemblyName,string typeName)
		{
			if (instance ==null)
			{
				Debug.LogWarning("LinkerData is missing an instance, please create one first in your assets from the create menu: Assets/Create/PlayMaker/Create Linker Wizard");
				return;
			}

			instance.RegisterLinkerEntry(assemblyName,typeName);
		}

		public void RegisterLinkerEntry(string assemblyName,string typeName)
		{
			if (instance ==null)
			{
				Debug.LogWarning("LinkerData is missing an instance, please create one first in your assets from the create menu: Assets/Create/PlayMaker/Create Linker Wizard");
				return;
			}

			if (string.IsNullOrEmpty(assemblyName))
			{
				Debug.LogError("LinkerEntry missing <color=blue>assemblyName</color>");
				return;
			}
			if (string.IsNullOrEmpty(typeName))
			{
				Debug.LogError("LinkerEntry missing <color=blue>typeName</color>");
				return;
			}

			// clean up assembly
			if (assemblyName.Contains(","))
			{
				assemblyName = assemblyName.Split(","[0])[0];
			}

			// clean up typeName
			if (typeName.Contains(","))
			{
				typeName = typeName.Split(","[0])[0];
			}

			if (!linkerEntries.ContainsKey(assemblyName))
			{
				linkerEntries.Add(assemblyName,new List<string>());
			}

			linkerEntries[assemblyName].Add(typeName);
		}


		void OnEnable() {
			LinkerData._instance_ = this;
			self = this;
		}

	}
}
