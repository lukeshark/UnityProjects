using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CanvasDropDownPM : MonoBehaviour {

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
		agents = GameObject.FindGameObjectsWithTag (FormationManagerPM.current.selectedTag);
		
	}
	public void OnValueChanged ()
	{
		dropDwn.RefreshShownValue ();
		
		agents = GameObject.FindGameObjectsWithTag (FormationManagerPM.current.selectedTag);
		spawnPoints = GameObject.FindGameObjectsWithTag ("SpawnPoints");
		
		
		if (agents.Length > 0 && spawnPoints.Length > 0) {
			
			for (int i = 0; i < agents.Length; i++) {
				agents [i].transform.position = spawnPoints [i].transform.position;
				
				
			}
			
		}
		
		switch (dropDwn.value) {
		//Semicircle
		case 0:
			FormationManagerPM.current.formation = FormationManagerPM.Formations.SemiCircle;
			break;
		//Wedge
		case 1:
			FormationManagerPM.current.formation = FormationManagerPM.Formations.Wedge;
			
			break;
		// V
		case 2:
			FormationManagerPM.current.formation = FormationManagerPM.Formations.V;
			
			break;
		// Circle
		case 3:
			FormationManagerPM.current.formation = FormationManagerPM.Formations.Circle;
			
			break;
		// Triangle
		case 4:
			FormationManagerPM.current.formation = FormationManagerPM.Formations.Triangle;
			
			break;
			
			
		}
		
		
	}
}
