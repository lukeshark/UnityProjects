using System;
using System.IO;
using System.Linq;

using UnityEngine;
using UnityEditor;

using NodeEditorFramework;
using NodeEditorFramework.Utilities;

using GenericMenu = UnityEditor.GenericMenu;
using AxlPlay;

namespace NodeEditorFramework.Standard
{
	public class NodeEditorWindow : EditorWindow 
	{
		// Information about current instance
		private static NodeEditorWindow _editor;
		public static NodeEditorWindow editor { get { AssureEditor(); return _editor; } }
		public static void AssureEditor() { if (_editor == null) OpenNodeEditor(); }

		// Opened Canvas
		public NodeEditorUserCache canvasCache;

		// GUI
		private string sceneCanvasName = "";
		private Rect loadSceneUIPos;
		private Rect createCanvasUIPos;
		private Rect convertCanvasUIPos;
		private int sideWindowWidth = 400;
		private int toolbarHeight = 17;

		private Rect modalWindowRect = new Rect(20, 50, 250, 100);

		private bool showSideWindow;
		private bool showModalPanel;

		public Rect sideWindowRect { get { return new Rect (position.width - sideWindowWidth, toolbarHeight, sideWindowWidth, position.height); } }
		public Rect canvasWindowRect { get { return new Rect (0, 0, position.width - sideWindowWidth, position.height); } }
		
		#region Easy AI
		int choiceIndex = 0;
		string varName ="";
		Vector2 scrollPos;
		#endregion
		#region General 

		/// <summary>
		/// Opens the Node Editor window and loads the last session
		/// </summary>
		[MenuItem("Window/Easy AI Node Editor")]
		public static NodeEditorWindow OpenNodeEditor () 
		{
			_editor = GetWindow<NodeEditorWindow>();
			_editor.minSize = new Vector2(400, 200);
			NodeEditor.ReInit (false);

			Texture iconTexture = ResourceManager.LoadTexture (EditorGUIUtility.isProSkin? "Textures/Icon_Dark.png" : "Textures/Icon_Light.png");
			_editor.titleContent = new GUIContent ("Node Editor", iconTexture);

			return _editor;
		}

		[UnityEditor.Callbacks.OnOpenAsset(1)]
		private static bool AutoOpenCanvas(int instanceID, int line)
		{
			if (Selection.activeObject != null && Selection.activeObject is MainNodeCanvas)
			{
				string NodeCanvasPath = AssetDatabase.GetAssetPath(instanceID);
				NodeEditorWindow.OpenNodeEditor().canvasCache.LoadNodeCanvas(NodeCanvasPath);
				return true;
			}
			return false;
		}

		private void OnEnable()
		{
			_editor = this;
			NodeEditor.checkInit(false);

			NodeEditor.ClientRepaints -= Repaint;
			NodeEditor.ClientRepaints += Repaint;

			EditorLoadingControl.justLeftPlayMode -= NormalReInit;
			EditorLoadingControl.justLeftPlayMode += NormalReInit;
			// Here, both justLeftPlayMode and justOpenedNewScene have to act because of timing
			EditorLoadingControl.justOpenedNewScene -= NormalReInit;
			EditorLoadingControl.justOpenedNewScene += NormalReInit;

			SceneView.onSceneGUIDelegate -= OnSceneGUI;
			 ;

			// Setup Cache
			canvasCache = new NodeEditorUserCache(Path.GetDirectoryName(AssetDatabase.GetAssetPath(MonoScript.FromScriptableObject (this))));
			canvasCache.SetupCacheEvents();
			if (canvasCache.nodeCanvas.GetType () == typeof(MainNodeCanvas))
				ShowNotification(new GUIContent("The Canvas has no specific type. Please use the convert button to assign a type and re-save the canvas!"));
		}

		private void NormalReInit()
		{
			NodeEditor.ReInit(false);
		}

		private void OnDestroy()
		{
			NodeEditor.ClientRepaints -= Repaint;

			EditorLoadingControl.justLeftPlayMode -= NormalReInit;
			EditorLoadingControl.justOpenedNewScene -= NormalReInit;

			SceneView.onSceneGUIDelegate -= OnSceneGUI;

			// Clear Cache
			canvasCache.ClearCacheEvents ();
		}

		public void OnLostFocus () 
		{ // Save any changes made while focussing this window
			// Will also save before possible assembly reload, scene switch, etc. because these require focussing of a different window
			canvasCache.SaveCache ();
		}

		#endregion

		#region GUI

		private void OnSceneGUI(SceneView sceneview)
		{
			DrawSceneGUI();
		}

		private void DrawSceneGUI()
		{
			if (canvasCache.editorState.selectedNode != null)
				canvasCache.editorState.selectedNode.OnSceneGUI();
			SceneView.lastActiveSceneView.Repaint();
		}

		// Modal GUI.Window for save scene input
		void DoModalWindow(int unusedWindowID)
		{
			GUILayout.Label ("Scene Saving", NodeEditorGUI.nodeLabel);

			GUILayout.BeginHorizontal ();
			sceneCanvasName = GUILayout.TextField (sceneCanvasName, GUILayout.ExpandWidth (true));
			if (GUILayout.Button (new GUIContent ("Save to Scene", "Save the canvas to the Scene"), GUILayout.ExpandWidth (false)))
				canvasCache.SaveSceneNodeCanvas (sceneCanvasName);
			GUILayout.EndHorizontal ();

			if (GUILayout.Button("Close"))
				showModalPanel = false;
		}

		private void OnGUI()
		{
			// Initiation
			NodeEditor.checkInit(true);
			if (NodeEditor.InitiationError || canvasCache == null)
			{
				GUILayout.Label("Node Editor Initiation failed! Check console for more information!");
				return;
			}
			AssureEditor ();
			canvasCache.AssureCanvas ();

			// Specify the Canvas rect in the EditorState, currently disabled for dynamic sidebar resizing
			// canvasCache.editorState.canvasRect = canvasWindowRect;
			// If you want to use GetRect:
//			Rect canvasRect = GUILayoutUtility.GetRect (600, 600);
//			if (Event.current.type != EventType.Layout)
//				mainEditorState.canvasRect = canvasRect;
			NodeEditorGUI.StartNodeGUI ("NodeEditorWindow", true);
            // Draw Toolbar
            DrawToolbarGUI();

            // Perform drawing with error-handling
            try
			{
				NodeEditor.DrawCanvas (canvasCache.nodeCanvas, canvasCache.editorState);
			}
			catch (UnityException e)
			{ // on exceptions in drawing flush the canvas to avoid locking the ui.
				canvasCache.NewNodeCanvas ();
				NodeEditor.ReInit (true);
				Debug.LogError ("Unloaded Canvas due to an exception during the drawing phase!");
				Debug.LogException (e);
			}

			

			// Show Side Window
			if (showSideWindow)
			{
				// Draw Side Window
				sideWindowWidth = Math.Min(600, Math.Max(200, (int)(position.width / 5)));
				GUILayout.BeginArea(sideWindowRect, GUI.skin.box);
				DrawSideWindow();
				GUILayout.EndArea();

				canvasCache.editorState.canvasRect = new Rect(0, toolbarHeight, position.width - sideWindowWidth, position.height);
			}
			else
			{
				canvasCache.editorState.canvasRect = new Rect(0, toolbarHeight, position.width, position.height);
			}

			if (showModalPanel)
			{
				BeginWindows();
				modalWindowRect = GUILayout.Window(0, modalWindowRect, DoModalWindow, "Save to Scene");
				EndWindows();
			}
			
			// Draw Blackboard
		 
			//GUILayout.BeginArea(new Rect (0, 20,200,300), GUI.skin.box);
			//GUILayout.Label("Blackboard");
			
			
			//GUILayout.EndArea();

			NodeEditorGUI.EndNodeGUI();
		}

		private static void newCanvasTypeCallback(object userdata)
		{
			NodeCanvasTypeData data = (NodeCanvasTypeData)userdata;

			editor.canvasCache.NewNodeCanvas(data.CanvasType);
			MainNodeCanvas.CreateCanvas(data.CanvasType);
		}	

		protected void DrawToolbarGUI()
		{
			EditorGUILayout.BeginHorizontal("Toolbar");
			GUI.backgroundColor = new Color(1f, 1f, 1f, 0.5f);

			if (GUILayout.Button("File", EditorStyles.toolbarDropDown, GUILayout.Width(50)))
			{
				var menu = new GenericMenu();

				foreach (System.Collections.Generic.KeyValuePair<Type, NodeCanvasTypeData> data in NodeCanvasManager.CanvasTypes)
					menu.AddItem(new GUIContent("New Canvas/" + data.Value.DisplayString), false, newCanvasTypeCallback, data.Value);

				menu.AddSeparator("");                 
				menu.AddItem(new GUIContent("Load Canvas", "Loads an Specified Empty CanvasType"), false, LoadCanvas);
				menu.AddSeparator("");
				menu.AddItem(new GUIContent("Save Canvas"), false, SaveCanvas);
				menu.AddItem(new GUIContent("Save Canvas As"), false, SaveCanvasAs);
				menu.AddSeparator("");

				// Load from canvas
				foreach (string sceneSave in NodeEditorSaveManager.GetSceneSaves())
				{
					menu.AddItem(new GUIContent("Load Canvas from Scene/" + sceneSave), false, LoadSceneCanvasCallback, sceneSave);
				}

				// Save Canvas to Scene	            
				menu.AddItem( new GUIContent("Save Canvas to Scene"), false, () =>
					{
						showModalPanel = true;
						Debug.Log(showModalPanel);
					});

				menu.ShowAsContext();
			}

			if (GUILayout.Button("Debug", EditorStyles.toolbarDropDown, GUILayout.Width(50)))
			{
				var menu = new GenericMenu();

				// Toggles side panel
				menu.AddItem(new GUIContent("Sidebar"), showSideWindow, () => { showSideWindow = !showSideWindow; });

				menu.ShowAsContext();
			}

			GUILayout.Space(10);
			if (GUILayout.Button("Blackboard", EditorStyles.toolbarButton, GUILayout.Width(80)))
			{
				if (showSideWindow){
					
					showSideWindow = false;
					
				}else{
					showSideWindow = true;
					
				}
				 
				NodeEditor.ReInit (true);
			}
			GUILayout.FlexibleSpace();

			GUILayout.Label (new GUIContent ("" + canvasCache.nodeCanvas.saveName + " (" + (canvasCache.nodeCanvas.livesInScene? "Scene Save" : "Asset Save") + ")", "Opened Canvas path: " + canvasCache.nodeCanvas.savePath), "ToolbarButton");
			GUILayout.Label ("Type: " + canvasCache.typeData.DisplayString + "/" + canvasCache.nodeCanvas.GetType ().Name + "", "ToolbarButton");

			GUI.backgroundColor = new Color(1, 0.3f, 0.3f, 1);

			if (GUILayout.Button("Force Re-init", EditorStyles.toolbarButton, GUILayout.Width(80)))
			{
				NodeEditor.ReInit (true);
			}

			EditorGUILayout.EndHorizontal();
			GUI.backgroundColor = Color.white;
		}
		private void DrawSideWindow()
		{
			GUILayout.Label (new GUIContent ("" + canvasCache.nodeCanvas.saveName + " (" + (canvasCache.nodeCanvas.livesInScene? "Scene Save" : "Asset Save") + ")", "Opened Canvas path: " + canvasCache.nodeCanvas.savePath), NodeEditorGUI.nodeLabelBold);
			GUILayout.Label ("Type: " + canvasCache.typeData.DisplayString + "/" + canvasCache.nodeCanvas.GetType ().Name + "");

//			EditorGUILayout.ObjectField ("Loaded Canvas", canvasCache.nodeCanvas, typeof(NodeCanvas), false);
//			EditorGUILayout.ObjectField ("Loaded State", canvasCache.editorState, typeof(NodeEditorState), false);

			if (GUILayout.Button(new GUIContent("New Canvas", "Loads an Specified Empty CanvasType")))
			{
				NodeEditorFramework.Utilities.GenericMenu menu = new NodeEditorFramework.Utilities.GenericMenu();
				NodeCanvasManager.FillCanvasTypeMenu(ref menu, canvasCache.NewNodeCanvas);
				menu.Show(createCanvasUIPos.position, createCanvasUIPos.width);
			}
			if (Event.current.type == EventType.Repaint)
			{
				Rect popupPos = GUILayoutUtility.GetLastRect();
				createCanvasUIPos = new Rect(popupPos.x + 2, popupPos.yMax + 2, popupPos.width - 4, 0);
			}
			if (canvasCache.nodeCanvas.GetType () == typeof(MainNodeCanvas) && GUILayout.Button(new GUIContent("Convert Canvas", "Converts the current canvas to a new type.")))
			{
				NodeEditorFramework.Utilities.GenericMenu menu = new NodeEditorFramework.Utilities.GenericMenu();
				NodeCanvasManager.FillCanvasTypeMenu(ref menu, canvasCache.ConvertCanvasType);
				menu.Show(convertCanvasUIPos.position, convertCanvasUIPos.width);
			}
			if (Event.current.type == EventType.Repaint)
			{
				Rect popupPos = GUILayoutUtility.GetLastRect();
				convertCanvasUIPos = new Rect(popupPos.x + 2, popupPos.yMax + 2, popupPos.width - 4, 0);
			}

			if (GUILayout.Button(new GUIContent("Save Canvas", "Save the Canvas to the load location")))
			{
				SaveCanvas();
			}

			GUILayout.Space(6);

			GUILayout.Label ("Asset Saving", NodeEditorGUI.nodeLabel);

			if (GUILayout.Button(new GUIContent("Save Canvas As", "Save the canvas as an asset")))
			{
				SaveCanvasAs();
			}

			if (GUILayout.Button(new GUIContent("Load Canvas", "Load the Canvas from an asset")))
			{
				string panelPath = NodeEditor.editorPath + "Resources/Saves/";
				if (canvasCache.nodeCanvas != null && !string.IsNullOrEmpty(canvasCache.nodeCanvas.savePath))
					panelPath = canvasCache.nodeCanvas.savePath;
				string path = EditorUtility.OpenFilePanel("Load Node Canvas", panelPath, "asset");
				if (!path.Contains(Application.dataPath))
				{
					if (!string.IsNullOrEmpty(path))
						ShowNotification(new GUIContent("You should select an asset inside your project folder!"));
				}
				else
					canvasCache.LoadNodeCanvas (path);
				if (canvasCache.nodeCanvas.GetType () == typeof(MainNodeCanvas))
					ShowNotification(new GUIContent("The Canvas has no specific type. Please use the convert button to assign a type and re-save the canvas!"));
			}

			//GUILayout.Space(6);
			GUILayout.Label ("Scene Saving", NodeEditorGUI.nodeLabel);

			GUILayout.BeginHorizontal ();
			sceneCanvasName = GUILayout.TextField (sceneCanvasName, GUILayout.ExpandWidth (true));
			if (GUILayout.Button (new GUIContent ("Save to Scene", "Save the canvas to the Scene"), GUILayout.ExpandWidth (false)))
				canvasCache.SaveSceneNodeCanvas (sceneCanvasName);
			GUILayout.EndHorizontal ();

			if (GUILayout.Button (new GUIContent ("Load from Scene", "Load the canvas from the Scene"))) 
			{
				NodeEditorFramework.Utilities.GenericMenu menu = new NodeEditorFramework.Utilities.GenericMenu();
				foreach (string sceneSave in NodeEditorSaveManager.GetSceneSaves())
					menu.AddItem(new GUIContent(sceneSave), false, LoadSceneCanvasCallback, (object)sceneSave);
				menu.Show (loadSceneUIPos.position, loadSceneUIPos.width);
			}
			if (Event.current.type == EventType.Repaint)
			{
				Rect popupPos = GUILayoutUtility.GetLastRect ();
				loadSceneUIPos = new Rect (popupPos.x+2, popupPos.yMax+2, popupPos.width-4, 0);
			}

			////GUILayout.Space (6);
			//GUILayout.Label ("Utility/Debug", NodeEditorGUI.nodeLabel);

			//if (GUILayout.Button (new GUIContent ("Recalculate All", "Initiates complete recalculate. Usually does not need to be triggered manually.")))
			//	canvasCache.nodeCanvas.TraverseAll ();

			//if (GUILayout.Button ("Force Re-Init"))
			//	NodeEditor.ReInit (true);
			
			
			var temp = (EasyAICanvas)canvasCache.nodeCanvas;
			
			//if (GUILayout.Button ("DELETE")){
				
			//	temp.blackboard = new System.Collections.Generic.List<blackboard>();
  
			//}
			
			//if (GUILayout.Button ("TEST")){
				
				
			//	object o =  true;
				
			//	var tt = new blackboard();
				
			//	tt.key ="qqq";
			//	tt.setValue(o);
				
			//	temp.Add("qqq",o);
				 
				
			 
				
				
			//}
			//if (GUILayout.Button ("DATA")){
				
			 
			//	foreach (var item in temp.blackboard)
			//	{
					 
			//		Debug.Log(item.key);
			//		Debug.Log(item.getValue().GetType());
			//		Debug.Log(item.getValue());
					
 
					
					
			//	}
			 
			//} 
			

			//NodeEditorGUI.knobSize = EditorGUILayout.IntSlider (new GUIContent ("Handle Size", "The size of the Node Input/Output handles"), NodeEditorGUI.knobSize, 12, 20);
			canvasCache.editorState.zoom = EditorGUILayout.Slider (new GUIContent ("Zoom", "Use the Mousewheel. Seriously."), canvasCache.editorState.zoom, 0.6f, 3);
			//NodeEditorUserCache.cacheIntervalSec = EditorGUILayout.IntSlider (new GUIContent ("Cache Interval (Sec)", "The interval in seconds the canvas is temporarily saved into the cache as a precaution for crashes."), NodeEditorUserCache.cacheIntervalSec, 30, 300);

			//NodeEditorGUI.curveBaseDirection = EditorGUILayout.FloatField ("Curve Base Dir", NodeEditorGUI.curveBaseDirection);
			//NodeEditorGUI.curveBaseStart = EditorGUILayout.FloatField ("Curve Base Start", NodeEditorGUI.curveBaseStart);
			//NodeEditorGUI.curveDirectionScale = EditorGUILayout.FloatField ("Curve Dir Scale", NodeEditorGUI.curveDirectionScale);

			if (canvasCache.editorState.selectedNode != null && Event.current.type != EventType.Ignore)
				canvasCache.editorState.selectedNode.DrawNodePropertyEditor();
			
			// Variables Setting
			
			
			GUILayout.BeginVertical("box");
			
			GUILayout.Label("Variables: ",GUILayout.Height(20), GUILayout.Width(80));
				// scrolll variables
			scrollPos = EditorGUILayout.BeginScrollView(scrollPos,GUIStyle.none, GUI.skin.verticalScrollbar, GUILayout.Width (240), GUILayout.Height (250));
			
	
			GUILayout.BeginHorizontal();
			
			
			
			// check if var name exist
			bool exist = false;
			foreach (var item in temp.blackboard)
			{
				if (item.key == varName){
					
					exist = true;
				}
			}
			
			if (GUILayout.Button("Add",GUILayout.Height(15), GUILayout.Width(35))&& choiceIndex != 0 && !string.IsNullOrEmpty(varName) && !exist ){


                temp.reloadVar = true;
				switch (choiceIndex)
				{
					// String
				case 1: 
					temp.Add(varName,"");
					break;
					// Bool
				case 2:
					temp.Add(varName,false);
					break;
					// Color
				case 3:
					temp.Add(varName,Color.white);
					break;
					// Float
				case 4:
					temp.Add(varName,0f);
					break;
					// GameObject
				case 5:
					
					GameObject o = null;
					
					blackboard_gameobject p = new blackboard_gameobject();
					
					p.value = o;
					
					var tt = new blackboard(); 
					tt.key =varName;
					tt.VariableType = "gameobject";
					tt.value = JsonUtility.ToJson(p);
					
					temp.blackboard.Add(tt);
					
					 
					break;
					// Int
				case 6:
					temp.Add(varName,0);
					break;
					// Vector 2
				case 7:
					temp.Add(varName,Vector2.zero);
					break;
					// Vector3
				case 8:
					temp.Add(varName,Vector3.zero);
					break;
				case 9:
					
					Transform t1 = null;
					blackboard_transform t2 = new blackboard_transform();
					t2.value = t1;
					
					var tt1 = new blackboard(); 
					tt1.key = varName;
					tt1.VariableType = "transform";
					
					tt1.value = JsonUtility.ToJson(t2);
					
					temp.blackboard.Add(tt1);
					break;
					
				default:
					break;
				}
				varName ="";
				
				
				
			} // end if Add var
			
			varName  = GUILayout.TextField(varName,GUILayout.Width(110));
			
			choiceIndex  = EditorGUILayout.Popup(choiceIndex,DisplayNames,GUILayout.Height(10),GUILayout.Width(60) );
			
			GUILayout.EndHorizontal();
			// show vars
			
			for (int i = 0; i <  temp.blackboard.Count; i++) {
				GUILayout.BeginVertical("box");
				GUILayout.BeginHorizontal();
 		
				GUILayout.Label(temp.blackboard[i].key,GUILayout.Width(60));
				GUILayout.Label(temp.blackboard[i].VariableType,GUILayout.Width(75));
				
				GUILayout.Space(15f);
				if (GUILayout.Button("X",GUILayout.Height(15), GUILayout.Width(20))){

                    temp.reloadVar = true;

                    temp.blackboard.RemoveAt(i);
					
					
					EditorGUIUtility.ExitGUI();
				 
				}
				GUILayout.EndHorizontal();
				GUILayout.BeginVertical();
				if (temp.blackboard[i].VariableType == "bool"){
			 	
					bool temp1 = (bool)temp.blackboard[i].getValue();
			 	
					temp1  = EditorGUILayout.Toggle ("", temp1 ,GUILayout.Width(80));
					blackboard b2 = new blackboard ();
					
					b2.key = temp.blackboard[i].key;
					b2.setValue(temp1);
				 
					temp.blackboard[i] =  b2;
					
					
				} else if (temp.blackboard[i].VariableType == "float"){
			 	
					float temp1 = (float)temp.blackboard[i].getValue();
			 	
					temp1  = EditorGUILayout.FloatField ("", temp1 ,GUILayout.Width(160));
					blackboard b2 = new blackboard ();
					
					b2.key = temp.blackboard[i].key;
					b2.setValue(temp1);
				 
					temp.blackboard[i] =  b2;
					
					
				} else if (temp.blackboard[i].VariableType == "int"){
			 	
					int temp1 = (int)temp.blackboard[i].getValue();
			 	
					temp1  = EditorGUILayout.IntField ("", temp1 ,GUILayout.Width(160));
					blackboard b2 = new blackboard ();
					
					b2.key = temp.blackboard[i].key;
					b2.setValue(temp1);
				 
					temp.blackboard[i] =  b2;
	 
				} else if (temp.blackboard[i].VariableType == "string"){
			 	
					string temp1 = (string)temp.blackboard[i].getValue();
			 	
					temp1  = EditorGUILayout.TextField ("", temp1 ,GUILayout.Width(160));
					blackboard b2 = new blackboard ();
					
					b2.key = temp.blackboard[i].key;
					b2.setValue(temp1);
				 
					temp.blackboard[i] =  b2;
 
				} else if (temp.blackboard[i].VariableType == "gameobject"){
			 	
					GameObject temp1 = (GameObject)temp.blackboard[i].getValue();

                    //temp1  = EditorGUILayout.ObjectField ("", temp1 , typeof(GameObject), true,GUILayout.Width(160)) as GameObject;

                    EditorGUILayout.LabelField("SET THIS ON INSPECTOR", GUILayout.Width(160));

                    if (temp1 ==  null){
						
 						blackboard b2 = new blackboard ();
						blackboard_gameobject b = new blackboard_gameobject();	
						b.value =null;
						b2.key = temp.blackboard[i].key;
						b2.VariableType ="gameobject";
						b2.value =JsonUtility.ToJson(b);
 
                        temp.blackboard[i] =  b2;
						
					}else{
						blackboard b2 = new blackboard ();
						
						b2.key = temp.blackboard[i].key;
						b2.setValue(temp1);
                        temp.blackboard[i] =  b2;
					}
						
				} else if (temp.blackboard[i].VariableType == "vector2"){
			 	
					Vector2 temp1 = (Vector2)temp.blackboard[i].getValue();
			 	
					temp1  = EditorGUILayout.Vector2Field ("", temp1 , GUILayout.Width(160));
					blackboard b2 = new blackboard ();
					
					b2.key = temp.blackboard[i].key;
					b2.setValue(temp1);
				 
					temp.blackboard[i] =  b2;
					
		 
				} else if (temp.blackboard[i].VariableType == "vector3"){
			 	
					Vector3 temp1 = (Vector3)temp.blackboard[i].getValue();
			 	
					temp1  = EditorGUILayout.Vector3Field ("", temp1 , GUILayout.Width(160));
					blackboard b2 = new blackboard ();
					
					b2.key = temp.blackboard[i].key;
					b2.setValue(temp1);
				 
					temp.blackboard[i] =  b2;

				} else if (temp.blackboard[i].VariableType == "color"){
			 	
					Color temp1 = (Color)temp.blackboard[i].getValue();
			 	
					temp1  = EditorGUILayout.ColorField (temp1 ,GUILayout.Width(160) );
					
					blackboard b2 = new blackboard ();
					
					b2.key = temp.blackboard[i].key;
					b2.setValue(temp1);
				 
					temp.blackboard[i] =  b2;
		 
				} else if (temp.blackboard[i].VariableType == "transform"){
			 	
					Transform temp1 = (Transform)temp.blackboard[i].getValue();
			 	
					temp1  = EditorGUILayout.ObjectField ("", temp1 , typeof(Transform), true,GUILayout.Width(160)) as Transform;
					
					blackboard b2 = new blackboard ();
					
					b2.key = temp.blackboard[i].key;
					b2.setValue(temp1);
				 
					temp.blackboard[i] =  b2;		
					
				 }

				
				//GUILayout.Label(coreSystem.fsmVariables[i].GetValue().ToString(),GUILayout.Width(220));
				
				GUILayout.EndVertical();
				GUILayout.Space(5f);
				GUILayout.EndVertical();
				
				
				
				
			} // end for for (int i = 0; i <  temp.blackboard.Count; 
		 
			GUILayout.EndScrollView();
			GUILayout.EndVertical();	
			
			// pass variables to nodes
			// pass var name to all nodes
			foreach (BaseEasyAINode item in temp.nodes)
			{ 
				item.fsmVars = temp.blackboard;

			}
			
			
		}

		private void LoadCanvas()
		{
			string path = EditorUtility.OpenFilePanel("Load Node Canvas", NodeEditor.editorPath + "Resources/Saves/", "asset");
			if (!path.Contains(Application.dataPath))
			{
				if (!string.IsNullOrEmpty(path))
					ShowNotification(new GUIContent("You should select an asset inside your project folder!"));
			}
			else
				canvasCache.LoadNodeCanvas (path);
		}

		private void SaveCanvas()
		{
			string path = canvasCache.nodeCanvas.savePath;
			if (!string.IsNullOrEmpty (path))
			{
				if (path.StartsWith ("SCENE/"))
					canvasCache.SaveSceneNodeCanvas (path.Substring (6));
				else
					canvasCache.SaveNodeCanvas (path);
			}
			else
				ShowNotification (new GUIContent ("No save location found. Use 'Save As'!"));
		}

		private void SaveCanvasAs()
		{
			string panelPath = NodeEditor.editorPath + "Resources/Saves/";
			if (canvasCache.nodeCanvas != null && !string.IsNullOrEmpty(canvasCache.nodeCanvas.savePath))
				panelPath = canvasCache.nodeCanvas.savePath;
			string path = EditorUtility.SaveFilePanelInProject ("Save Node Canvas", "Node Canvas", "asset", "", panelPath);
			if (!string.IsNullOrEmpty (path))
				canvasCache.SaveNodeCanvas (path);
		}

		public void LoadSceneCanvasCallback (object canvas) 
		{
			canvasCache.LoadSceneNodeCanvas ((string)canvas);
		}
		public static string[] DisplayNames{
			get{
				return new string[10]{
					"None",
					"String",
					"Bool",
					"Color",
					"Float",
					"GameObject",
					"Int",
					"Vector2",
					"Vector3",
					"Tranform"
				};
			}
		}
		#endregion
	}
}