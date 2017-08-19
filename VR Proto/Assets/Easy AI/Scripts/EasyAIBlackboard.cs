using System.Collections.Generic;
using UnityEngine;
using System;
namespace AxlPlay
{
    public class EasyAIBlackboard
    {
        public Dictionary<string, object> blackboard;
        public static Dictionary<string, object> globalBlackboard;

        public EasyAIBlackboard()
        {
            blackboard = new Dictionary<string, object>();
            globalBlackboard = new Dictionary<string, object>();


        }
        public EasyAIBlackboard(List<blackboard> data, List<blackboard_go> go)
        {
            blackboard = new Dictionary<string, object>();
            globalBlackboard = new Dictionary<string, object>();

            foreach (var item in data)
            {

                var key = item.key;
                var value = (object)item.getValue();

                if (item.VariableType == "gameobject")
                {
                    foreach (var _itemgo in go)
                    {
                        if (_itemgo.key == key) {

                            value = _itemgo.value;
                            break;
                        }
                    }
                    Add(key, value);

                }
                else {
                    Add(key, value);
                }
 
            }

        }

        public void Add(string key, object obj)
        {
            if (!blackboard.ContainsKey(key))
            {
                blackboard.Add(key, obj);
            }
            else
            {
                blackboard[key] = obj;
            }
        }

        public object Get(string key)
        {
            object obj;
            blackboard.TryGetValue(key, out obj);
            return obj;
        }

        //Global blackboard
        public void AddGlobal(string key, object obj)
        {
            if (!globalBlackboard.ContainsKey(key))
            {
                globalBlackboard.Add(key, obj);
            }
            else
            {
                globalBlackboard[key] = obj;
            }
        }

        public object GetGlobal(string key)
        {
            object obj;
            globalBlackboard.TryGetValue(key, out obj);
            return obj;
        }
    }
}

