using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using NodeEditorFramework;
using NodeEditorFramework.Utilities;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace AxlPlay{
	[Node (false, "Utils/Get Gameobject")]
public class GetGameObjectNode : BaseEasyAINode 
{
	public const string ID = "GetGameObjectNode";
	public override string GetID { get { return ID; } }
	
	public GameObject targetGameObject;

    public string tagStr;
    public string nameStr;

    public override Node Create (Vector2 pos) 
	{
        GetGameObjectNode node = CreateInstance<GetGameObjectNode> ();
 
		node.rect = new Rect (pos.x, pos.y, 260, 120);
		node.name = "Get GameObject";
		
		node.CreateInput ("Value", "Action");
 
		node.CreateOutput ("Output val", "Action");
		node.myType = type.action;
		node.titleColor = Color.red;
		// dropdown index choice
		node.choiceIndex = new int[1];
	 
		// index from fsmVariables
		node.internalVar = new string[1];
		
		// bool for button selected
		node.useInternalVar = new bool[1];
		
 
		return node;
	}
	
	protected override void NodeGUI () 
	{
		#if UNITY_EDITOR
            internalVar = new string[1];

            GUILayout.Label ("Get Game Object!");
				
		    GUILayout.BeginVertical();

            GUILayout.Space(10);

            tagStr = EditorGUILayout.TagField("Tag for Objects:", tagStr);
            GUILayout.Space(5);
            nameStr = EditorGUILayout.TextField("Name for Gameobject:", nameStr);
            GUILayout.Space(5);


            GUILayout.EndVertical();
            GUILayout.BeginHorizontal();
         
            // to prevent is delete the variable from inspector
            if (choiceIndex[0] >= fsmVars.Count)
                {

                    choiceIndex[0] = 0;
                }
                EditorGUILayout.LabelField("Set Game Object to:", GUILayout.Width(120));

            if (fsmVars.Count > 0)
            {
                // set to array with the variable type

                var results = (from x in fsmVars
                               where x.VariableType == "gameobject"
                               select x.key).ToArray();



                if (results.Length > 0)
                {
                    choiceIndex[0] = EditorGUILayout.Popup(choiceIndex[0], results, GUILayout.Height(10), GUILayout.Width(110));
 
                   internalVar[0] = results[choiceIndex[0]];
                }

            }
            //else
            //{

            //    GUILayout.Space(120);
            //}

            useInternalVar[0] = true;
            GUI.color = Color.white;
            GUILayout.EndHorizontal();
			#endif


        }
	public override void OnStart(EasyAIBlackboard data)
	{
		base.OnStart(data);
            	 
	}
	public override BaseEasyAINode.Task OnUpdate(EasyAIBlackboard data)
	{
		base.OnUpdate(data);

            if (!string.IsNullOrEmpty(nameStr))
            {
                targetGameObject = GameObject.Find(nameStr);
            }
            else if (!string.IsNullOrEmpty(tagStr))
            {
                
                targetGameObject = GameObject.FindGameObjectWithTag(tagStr);
            }
           

            if (targetGameObject != null)
            {
                data.Add(internalVar[0], targetGameObject);
                return Task.Success;
            }
            else
            {
                return Task.Running;
            }
		
	}
	 
 
	
	
}
}