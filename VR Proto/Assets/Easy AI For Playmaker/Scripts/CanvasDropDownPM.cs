using UnityEngine;
using System.Collections;
using UnityEngine.UI;
namespace  AxlPlay{
public class CanvasDropDownPM : MonoBehaviour {
	
	public FormationManagerPM _formation;
	private Dropdown dropDwn;
	private GameObject[] agents;
	private GameObject[] spawnPoints;
	private int _paramereter;
	
	void Awake ()
	{
		dropDwn = GetComponent<Dropdown> ();
	}
	void Start ()
	{
		agents = GameObject.FindGameObjectsWithTag (_formation.selectedTag);
		
	}
	public void OnValueChanged ()
	{
		dropDwn.RefreshShownValue ();
		
		agents = GameObject.FindGameObjectsWithTag (_formation.selectedTag);
		spawnPoints = GameObject.FindGameObjectsWithTag ("SpawnPoints");
		
		
		if (agents.Length > 0 && spawnPoints.Length > 0) {
			
			for (int i = 0; i < agents.Length; i++) {
				agents [i].transform.position = spawnPoints [i].transform.position;
				
				
			}
			
		}
		
		switch (dropDwn.value) {
		//Semicircle
		case 0:
			_formation.formation = FormationManagerPM.Formations.SemiCircle;
			break;
		//Wedge
		case 1:
			_formation.formation = FormationManagerPM.Formations.Wedge;
			
			break;
		// V
		case 2:
			_formation.formation = FormationManagerPM.Formations.V;
			
			break;
		// Circle
		case 3:
			_formation.formation = FormationManagerPM.Formations.Circle;
			
			break;
		// Triangle
		case 4:
			_formation.formation = FormationManagerPM.Formations.Triangle;
			
			break;
			
			
		}
		
		
	}
}
}
