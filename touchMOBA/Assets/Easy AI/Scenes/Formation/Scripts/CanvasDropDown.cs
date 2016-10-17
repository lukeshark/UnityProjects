using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CanvasDropDown : MonoBehaviour
{
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
		agents = GameObject.FindGameObjectsWithTag (FormationManager.current.selectedTag);


	}

	void Update ()
	{
		if (agents.Length > 0) {
			for (int i = 0; i < agents.Length; i++) {
				if (agents [i].GetComponent<AgentScript> ().navMeshAgent.velocity.z <= 0.6f && agents [i].GetComponent<AgentScript> ().navMeshAgent.velocity.x <= 0.6f) {
					_paramereter = Animator.StringToHash ("Run");
					agents [i].GetComponent<Animator> ().SetBool (_paramereter, false);
				} else {
					_paramereter = Animator.StringToHash ("Run");
					agents [i].GetComponent<Animator> ().SetBool (_paramereter, true);
				}

			}

		}
	}

	public void OnValueChanged ()
	{
		dropDwn.RefreshShownValue ();

		agents = GameObject.FindGameObjectsWithTag (FormationManager.current.selectedTag);
		spawnPoints = GameObject.FindGameObjectsWithTag ("SpawnPoints");


		if (agents.Length > 0 && spawnPoints.Length > 0) {
	
			for (int i = 0; i < agents.Length; i++) {
				agents [i].transform.position = spawnPoints [i].transform.position;
		

			}

		}

		switch (dropDwn.value) {
		//Semicircle
		case 0:
			FormationManager.current.formation = FormationManager.Formations.SemiCircle;
			break;
		//Wedge
		case 1:
			FormationManager.current.formation = FormationManager.Formations.Wedge;

			break;
		// V
		case 2:
			FormationManager.current.formation = FormationManager.Formations.V;

			break;
		// Circle
		case 3:
			FormationManager.current.formation = FormationManager.Formations.Circle;

			break;
		// Triangle
		case 4:
			FormationManager.current.formation = FormationManager.Formations.Triangle;

			break;
		

		}


	}

}
