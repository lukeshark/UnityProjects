using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using NodeEditorFramework;
using System;
 
namespace AxlPlay{ 
[NodeCanvasType("Easy AI Canvas")]
public class EasyAICanvas : MainNodeCanvas
{
	#region BlackBoard
	//public List<blackboard> blackboard = new List<blackboard>();
	public List<blackboard> blackboard = new List<blackboard>();

    public bool reloadVar;
 
   public void Add(string _key, object _obj)
	{
		bool existe = false;
  
		for (int i = 0; i < blackboard.Count; i++) {
				
		
			var key = blackboard[i].key;
	
			if (_key  == key){
				
				existe = true;
 
				blackboard[i].key = key;
				blackboard[i].setValue(_obj);
 
				break;
			}
			
		}	
		
		if (!existe){
			
			blackboard b = new blackboard();
 
			b.key = _key;
			b.setValue(_obj);	
			blackboard.Add(b);
		}
		
		 
	}
	
	public object Get(string _key)
	{
	 
		foreach (var item in blackboard)
		{ 
		 
			if (item.key == _key){
				
				return (object)item.getValue();
			}
 		
		}	
		return null;
	 
	}
	public bool Remove (string _key)
	{
		for (int i = 0; i < blackboard.Count; i++)
		{
		 
			if (blackboard[i].key == _key)
			{
				blackboard.RemoveAt(i);
				return true;
			}
		}
		
		return false;
	}
	
		#endregion
	
	public BaseEasyAINode GetRootNode()
	{
		 
		foreach(Node node in this.nodes)
		{
			if(node.name == "Start")
			{
				return (BaseEasyAINode)node.Outputs[0].GetNodeAcrossConnection();
			}
		}
		return null;
	}
	
}
[Serializable]
 public class blackboard 
 {
 	public string key; 
 	public string VariableType;
 	public string value;
 	//public bool fsmbool;
 	//public float fsmfloat;
	//public int fsmint;
	//public string fsmstring;
	//public Transform fsmtransform; 
	//public GameObject fsmgo;
	 
	// public void setValue(bool value){ 
	//	 VariableType =  "Bool";
	//	 fsmbool = value;
	// }
	// public void setValue(float value){ 
	//	 VariableType =  "Float";
	//	 fsmfloat = value;
	//}
	//public void setValue(int value){ 
	//	 VariableType =  "Int32";
	//	 fsmint = value;
	//}
	// public void setValue(string _value){ 
 
	//	VariableType =  "String";
	//	 fsmstring =_value;
	//}
	// public void setValue(Transform _value){ 
 
	//	VariableType =  "Transform";
	//	 fsmtransform =_value;
	//}
	// public void setValue(GameObject value){
	//	 VariableType = "GameObject";
	//	 fsmgo = value;
	// }
	 public void setValue(object _value)
	 {
		 if (_value.GetType() == typeof(bool)){
		 	
		 	VariableType ="bool";
		 	blackboard_bool b = new blackboard_bool();
		  
			 b.value = (bool)_value;
			 
			 value = JsonUtility.ToJson(b);
 
		 }else  if (_value.GetType() == typeof(int)){
		 	
		 	VariableType ="int";
		 	blackboard_int b = new blackboard_int();
		  
			 b.value = (int)_value;
			 
			 value = JsonUtility.ToJson(b);
		 
		 }else if(_value.GetType() == typeof(float)){
		 	
		 	VariableType ="float";
		 	blackboard_float b = new blackboard_float();
		  
			 b.value = (float)_value;
			 
			 value = JsonUtility.ToJson(b);
			 
		 
		 }else if(_value.GetType() == typeof(string)){
		 	
		 	VariableType ="string";
		 	blackboard_string b = new blackboard_string();
		  
			 b.value = (string)_value;
			 
			 value = JsonUtility.ToJson(b);
		
		 }else if(_value.GetType() == typeof(Transform)){
		 	
		 	VariableType ="transform";
		 	blackboard_transform b = new blackboard_transform();
		  
			 b.value = (Transform)_value;
			 
			 value = JsonUtility.ToJson(b);
		
		 }else if(_value.GetType() == typeof(GameObject)){
		 	
		 	VariableType ="gameobject";
		 	blackboard_gameobject b = new blackboard_gameobject();
		  
			 b.value = (GameObject)_value;
			 
			 value = JsonUtility.ToJson(b);
			 
		 }else if(_value.GetType() == typeof(Color)){
		 	
		 	VariableType ="color";
		 	blackboard_color b = new blackboard_color();
		  
			 b.value = (Color)_value;
			 
			 value = JsonUtility.ToJson(b);

		 }else if(_value.GetType() == typeof(Vector3)){
		 	
		 	VariableType ="vector3";
		 	blackboard_vector3 b = new blackboard_vector3();
		  
			 b.value = (Vector3)_value;
			 
			 value = JsonUtility.ToJson(b);

		 }else if(_value.GetType() == typeof(Vector2)){
		 	
		 	VariableType ="vector2";
		 	blackboard_vector2 b = new blackboard_vector2();
		  
			 b.value = (Vector2)_value;
			 
			 value = JsonUtility.ToJson(b);
			 
		 }
			 
		 
	
	 }
	 public object getValue(){
	 
		switch (VariableType)
		{
			case "bool":
				blackboard_bool b = JsonUtility.FromJson<blackboard_bool>(value);
				
				return b.value; 
			
			case "int":
				blackboard_int b1 = JsonUtility.FromJson<blackboard_int>(value);
				
				return b1.value; 
			
			case "float":
				blackboard_float b2 = JsonUtility.FromJson<blackboard_float>(value);
				
				return b2.value; 
 
			case "string":
				blackboard_string b3 = JsonUtility.FromJson<blackboard_string>(value);
				
				return b3.value; 
			
			case "transform":
				blackboard_transform b4 = JsonUtility.FromJson<blackboard_transform>(value);
				
				return b4.value; 
			case "gameobject":
				blackboard_gameobject b5 = JsonUtility.FromJson<blackboard_gameobject>(value);
                    //Debug.Log(value);
                    //Debug.Log(b5.value);
				return b5.value; 
			case "color":
				blackboard_color b6 = JsonUtility.FromJson<blackboard_color>(value);
				
				return b6.value; 
			case "vector3":
				blackboard_vector3 b7 = JsonUtility.FromJson<blackboard_vector3>(value);
				
				return b7.value; 
			 
			case "vector2":
				blackboard_vector2 b8 = JsonUtility.FromJson<blackboard_vector2>(value);
				
				return b8.value; 
			 
			default:
				return null;
				
		}
	
	 }
 	 
 }
 
 [Serializable]
 public class blackboard_bool
 {
	 public bool value; 
 }
 [Serializable]
 public class blackboard_int
 {
	 public int value; 
 }
 [Serializable]
 public class blackboard_float
 {
	 public float value; 
 }
 [Serializable]
 public class blackboard_gameobject
 {
	 public GameObject value; 
 }
 [Serializable]
 public class blackboard_transform
 {
	 public Transform value; 
 }
  [Serializable]
 public class blackboard_string  
 {
	 public string value; 

 }
  [Serializable]
 public class blackboard_color 
 {
	 public Color value; 

 }
  [Serializable]
 public class blackboard_vector3 
 {
	 public Vector3 value; 

 }
  [Serializable]
 public class blackboard_vector2 
 {
	 public Vector2 value; 
	 
 }
[Serializable]
public class blackboard_go
{
    public string key;
    public int index;
    public GameObject value;
}
    [Serializable]
    public class UniDictionary<Key, Value>
    {
	    [SerializeField]
	    public List<Key> keys = new List<Key>();
	    
	    [SerializeField]
	    public List<Value> values = new List<Value>();
	    
	    public void Add(Key key, Value value)
	    {
		    if (keys.Contains(key))
			    return;
		    
		    keys.Add(key);
		    values.Add(value);
	    }
	    
	    public void Remove(Key key)
	    {
		    if (!keys.Contains(key))
			    return;
		    
		    int index = keys.IndexOf(key);
		    
		    keys.RemoveAt(index);
		    values.RemoveAt(index);
	    }
	    
	    public bool TryGetValue(Key key, out Value value)
	    {
		    if (keys.Count != values.Count)
		    {
			    keys.Clear();
			    values.Clear();
			    value = default(Value);
			    return false;
		    }
		    
		    if (!keys.Contains(key))
		    {
			    value = default(Value);
			    return false;
		    }
		    
		    int index = keys.IndexOf(key);
		    value = values[index];
		    
		    return true;
	    }
	    
	    public void ChangeValue(Key key, Value value)
	    {
		    if (!keys.Contains(key))
			    return;
		    
		    int index = keys.IndexOf(key);
		    
		    values[index] = value;
	    }
	    public void Test(){
	    	
	    	Debug.Log("x aqui " + values.Count);
	    	
		    for (int i = 0; i < values.Count; i++) {
			    
			    Debug.Log(keys[i].ToString());
			    Debug.Log(values[i].ToString());
		    }
	    }
    }
 
}
