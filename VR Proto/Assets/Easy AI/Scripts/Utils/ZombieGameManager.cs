using UnityEngine;
using System.Collections.Generic;
namespace  AxlPlay{
public class ZombieGameManager : MonoBehaviour {
	
	public static ZombieGameManager current;
	
	
 	public GameObject poolBloodGameObject;
 
 	public int pooledAmount = 20;
 	public bool willGrow;
	
	public Transform[] spawnPoints;
	
	
	List<GameObject> pooledBloodGameObjects;
	 
	void Awake()
	{
		if (current == null)
			current = this;
		
	}
	
    // Use this for initialization
	void Start()
	{
		pooledBloodGameObjects = new List<GameObject>();
		
		for (int i = 0; i < pooledAmount; i++)
		{
			
			GameObject obj = (GameObject)Instantiate(poolBloodGameObject);
			obj.SetActive(false);
			pooledBloodGameObjects.Add(obj);
			
		}
	
	 
	}
	public GameObject getBloodPooledObject()
	{
		for (int i = 0; i < pooledBloodGameObjects.Count; i++)
		{
			
			if (!pooledBloodGameObjects[i].activeInHierarchy)
			{
				
				return pooledBloodGameObjects[i];
			}
		}
		
		if (willGrow)
		{
			GameObject obj = (GameObject)Instantiate(poolBloodGameObject);
			pooledBloodGameObjects.Add(obj);
			return obj;
			
		}
		
		return null;
		
	}
	public Transform getRandomPos(){
		
		int MyIndex = Random.Range(0,(spawnPoints.Length - 1));
		return spawnPoints[MyIndex];
		
	}
}
}
