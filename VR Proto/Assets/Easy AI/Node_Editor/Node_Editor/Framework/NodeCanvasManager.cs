using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using UnityEngine;

using NodeEditorFramework.Utilities;

namespace NodeEditorFramework
{
	public class NodeCanvasManager
	{
		public static Dictionary<Type, NodeCanvasTypeData> CanvasTypes;
		private static Action<Type> _callBack;

		public static void GetAllCanvasTypes()
		{
			CanvasTypes = new Dictionary<Type, NodeCanvasTypeData>();

			IEnumerable<Assembly> scriptAssemblies = AppDomain.CurrentDomain.GetAssemblies()
				.Where((Assembly assembly) => assembly.FullName.Contains("Assembly"));
			foreach (Assembly assembly in scriptAssemblies)
			{
				foreach (Type type in assembly.GetTypes()
					.Where( T => T.IsClass && !T.IsAbstract && 
						(T != typeof(MainNodeCanvas) && T.IsSubclassOf (typeof(MainNodeCanvas))) &&
							T.GetCustomAttributes(typeof (NodeCanvasTypeAttribute), false).Length > 0))
				{
					object[] nodeAttributes = type.GetCustomAttributes(typeof (NodeCanvasTypeAttribute), false);
					NodeCanvasTypeAttribute attr = nodeAttributes[0] as NodeCanvasTypeAttribute;
					CanvasTypes.Add(type, new NodeCanvasTypeData() {CanvasType = type, DisplayString = attr.Name});
				}
			}
		}

		private static void unwrapTypeCallback(object userdata)
		{
			NodeCanvasTypeData data = (NodeCanvasTypeData)userdata;
			_callBack(data.CanvasType);
		}

		public static void FillCanvasTypeMenu(ref GenericMenu menu, Action<Type> newNodeCanvas)
		{
			_callBack = newNodeCanvas;
			foreach (KeyValuePair<Type, NodeCanvasTypeData> data in CanvasTypes)
			{
				if(data.Value.DisplayString == "Easy AI Canvas")
					menu.AddItem(new GUIContent(data.Value.DisplayString), false, unwrapTypeCallback, (object)data.Value);
			}
		}

		public static bool CheckCanvasCompability (string nodeID, MainNodeCanvas canvas) 
		{
			NodeData data = NodeTypes.getNodeData (nodeID);
			return data.limitToCanvasTypes == null || data.limitToCanvasTypes.Length == 0 || data.limitToCanvasTypes.Contains (canvas.GetType ());
		}

		public static NodeCanvasTypeData getCanvasTypeData (MainNodeCanvas canvas)
		{
			NodeCanvasTypeData data;
			CanvasTypes.TryGetValue (canvas.GetType (), out data);
			return data;
		}

		/// <summary>
		/// Converts the type of the canvas to the specified type.
		/// </summary>
		public static MainNodeCanvas ConvertCanvasType (MainNodeCanvas canvas, Type newType)
		{
			MainNodeCanvas convertedCanvas = canvas;
			if (canvas.GetType () != newType && newType != typeof(MainNodeCanvas) && newType.IsSubclassOf (typeof(MainNodeCanvas)))
			{
				canvas = NodeEditorSaveManager.CreateWorkingCopy (canvas, true);
				convertedCanvas = MainNodeCanvas.CreateCanvas(newType);
				convertedCanvas.nodes = canvas.nodes;
				convertedCanvas.editorStates = canvas.editorStates;
				convertedCanvas.Validate ();
			}
			return convertedCanvas;
		}
	}

	public struct NodeCanvasTypeData
	{
		public string DisplayString;
		public Type CanvasType;
	}

	public class NodeCanvasTypeAttribute : Attribute
	{
		public string Name;

		public NodeCanvasTypeAttribute(string displayName)
		{
			Name = displayName;
		}
	}
}