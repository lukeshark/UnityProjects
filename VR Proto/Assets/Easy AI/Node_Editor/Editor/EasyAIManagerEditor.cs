using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Linq;
namespace AxlPlay
{
    [CustomEditor(typeof(EasyAIManager))]

    public class EasyAIManagerEditor : Editor
    {
        public float colorTime;
        public int colorType;
        public GameObject temp1;

        public int blackboardCount;

        EasyAIManager _target;

        public void OnEnable()
        {
            
            EditorApplication.update += Update;
        }
        public void OnDisable()
        {
            EditorApplication.update -= Update;
        }
        void Update()
        {

            if (Application.isPlaying)
            {

                colorTime = colorTime + 1;



                if (colorTime < 40)
                {
                    colorType = 1;

                }
                if (colorTime > 80) colorTime = 0;
                if (colorTime > 40)
                {

                    colorType = 2;

                }

            }
        }
        public override void OnInspectorGUI()
        {
           

            _target = (EasyAIManager)target;
            _target.canvas = EditorGUILayout.ObjectField("Canvas:", _target.canvas, typeof(EasyAICanvas), true, GUILayout.Width(280)) as EasyAICanvas;
            _target.showVars = EditorGUILayout.Toggle("Show Vars:", _target.showVars);
            _target.showDebugger = EditorGUILayout.Toggle("Show Debugger:", _target.showDebugger);
            _target.debugConsole = EditorGUILayout.Toggle("Debug to Console:", _target.debugConsole);


            if (_target.showDebugger)
            {
                GUILayout.BeginVertical("box");
                GUILayout.Label("Action:");
                if (_target.curNode != null)
                {



                    GUIStyle GS = new GUIStyle();
                    GS.normal.background = MakeTex(2, 2, Color.green);

                    GUIStyle GSred = new GUIStyle();
                    GSred.normal.background = MakeTex(2, 2, Color.red);

                    GUIStyle GSwhite = new GUIStyle();
                    GSwhite.normal.background = MakeTex(2, 2, Color.white);



                    GUILayout.Space(5f);

                    if (colorType == 1)
                        GUILayout.Label(_target.curNode.name, GS);
                    else if (colorType == 2)
                        GUILayout.Label(_target.curNode.name, GSwhite);

                    // show conditionals playing;
                    if (_target.curNode.Outputs[0].connections.Count > 0)
                    {
                        for (int x = 0; x < _target.curNode.Outputs[0].connections.Count; x++)
                        {

                            var temp = (BaseEasyAINode)_target.curNode.Outputs[0].connections[x].body;

                            GUILayout.Label("Conditionals:");
                            if (temp.myType == BaseEasyAINode.type.conditional)
                            {
                                GUILayout.Label(temp.name, GSred);
                            }

                        }

                    }

                }


                GUILayout.EndVertical();
            }

    
            if (_target.canvas != null && _target.showVars == true && Application.isPlaying == false)
            {


                GUILayout.BeginVertical("box");
                GUILayout.Label("Blackboard Variables:");

                // check blackboar and listis equal


                if (_target.canvas.reloadVar ) {
                    _target.listGO.Clear();
                    _target.canvas.reloadVar = false;
                  

                }
                // int coungGO = 0;
                //foreach (var item in _target.canvas.blackboard)
                //{
                //    if (item.VariableType == "gameobject") {
                //        coungGO++;
                //    }
                //}
                //if (coungGO != _target.listGO.Count) {
                //    Debug.Log("CLEAR");
                //    _target.listGO.Clear();
                //}



                for (int i = 0; i < _target.canvas.blackboard.Count; i++)
                {


                    if (_target.canvas.blackboard[i].VariableType == "gameobject")
                    {

                        bool exist = false;
                        for (int z = 0; z < _target.listGO.Count; z++)
                        {

                            if (_target.canvas.blackboard[i].key == _target.listGO[z].key)
                            {
                                exist = true;
                            }
                             

                        }

                        if (!exist)
                        {
                            blackboard_go t = new blackboard_go();
                            t.key = _target.canvas.blackboard[i].key;
                            t.index = i;
                            temp1 = null;
                            t.value = temp1;
                            _target.listGO.Add(t);
                            
                        }


                        int indexPos = 0;
                        for (int z = 0; z < _target.listGO.Count; z++)
                        {
                            if (_target.listGO[z].key == _target.canvas.blackboard[i].key)
                            {
                                indexPos = z;
                                break;
                            }
                        }

                    
                            GUILayout.Label(_target.listGO[indexPos].key, GUILayout.Width(100));

                            _target.listGO[indexPos].value = EditorGUILayout.ObjectField("", _target.listGO[indexPos].value, typeof(GameObject), true, GUILayout.Width(180)) as GameObject;

                      

                    }
                    else if (_target.canvas.blackboard[i].VariableType == "int")
                    {

                        GUILayout.Label(_target.canvas.blackboard[i].key, GUILayout.Width(100));

                        int temp = (int)_target.canvas.blackboard[i].getValue();

                        temp = EditorGUILayout.IntField("", temp, GUILayout.Width(180));

                        if (temp != (int)_target.canvas.blackboard[i].getValue())
                        {

                            _target.canvas.blackboard[i].setValue(temp);

                        }

                    }


                } // end for
                GUILayout.EndVertical();
                /// All variables when application is playing
            }
            else if (_target.canvas != null && _target.showVars == true && Application.isPlaying == true && _target.data.blackboard != null)
            {


                GUILayout.BeginVertical("box");
                GUILayout.Label("Blackboard Variables:");


                for (int i = 0; i < _target.data.blackboard.Count; i++)
                {


                    if (_target.data.blackboard.ElementAt(i).Value.GetType() == typeof(GameObject))
                    {

                        GUILayout.Label(_target.data.blackboard.ElementAt(i).Key, GUILayout.Width(100));

                        var temp = (GameObject)_target.data.blackboard.ElementAt(i).Value;
                        temp = EditorGUILayout.ObjectField("", temp, typeof(GameObject), true, GUILayout.Width(180)) as GameObject;

                        if (temp != (GameObject)_target.data.blackboard.ElementAt(i).Value)
                        {
                            _target.data.blackboard[_target.data.blackboard.ElementAt(i).Key] = temp;

                        }

                    }

                }

                GUILayout.EndVertical();

            }
            if (GUI.changed)
                EditorUtility.SetDirty(target);

            //DrawDefaultInspector();

        }
        private Texture2D MakeTex(int width, int height, Color col)
        {
            Color[] pix = new Color[width * height];
            for (int i = 0; i < pix.Length; ++i)
            {
                pix[i] = col;
            }
            Texture2D result = new Texture2D(width, height);
            result.SetPixels(pix);
            result.Apply();
            return result;
        }

    }
}