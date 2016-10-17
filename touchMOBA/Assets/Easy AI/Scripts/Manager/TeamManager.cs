using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TeamManager : MonoBehaviour
{
    public static TeamManager current;

    public Text txt;

    public GameObject[] team1;

    public GameObject[] team2;
    public GameObject blood;
    private List<GameObject> pooledBlood;
    private int total_team1;
    private int total_team2;
    void Awake()
	{
        if (current == null)
        {
            current = this;
        }

    }
    // Use this for initialization
    void Start()
    {
        pooledBlood = new List<GameObject>();

        for (int i = 0; i < 10; i++)
        {

            GameObject obj = (GameObject)Instantiate(blood);
            obj.SetActive(false);
            pooledBlood.Add(obj);

        }
        total_team1 = team1.Length;
        total_team2 = team2.Length;

    }
	public void seeTarget(GameObject go){

        if (go == null)
        {
            txt.text = "Not Target";
        }
        else
        {
            txt.text = go.name;
        }

		
		
	}
    public GameObject getBloodPooled()
    {
        for (int i = 0; i < pooledBlood.Count; i++)
        {

            if (!pooledBlood[i].activeInHierarchy)
            {

                return pooledBlood[i];
            }
        }
        return null;

    }
    public GameObject FindClosestTargetTeam1 (Vector3 position)
    {

        // return nodes
        //    .OrderBy(o => (o.transform.position - position).sqrMagnitude)
        //   .FirstOrDefault();

        GameObject closest = null;
        float distance = Mathf.Infinity;

        foreach (GameObject go in team1)
        {

            if (go.activeInHierarchy)
            {
           
                    Vector3 diff = go.transform.position - position;
                    float curDistance = diff.sqrMagnitude;

                    if (curDistance < distance)
                    {
                        closest = go;

                        distance = curDistance;
                    }
             
            } // end if activeInHierarchy
        }

        return closest;
    }
    public GameObject FindClosestTargetTeam(Vector3 position , string tag)
    {

        // return nodes
        //    .OrderBy(o => (o.transform.position - position).sqrMagnitude)
        //   .FirstOrDefault();

        GameObject closest = null;
        float distance = Mathf.Infinity;

        if (tag == "Team2")
        {
            foreach (GameObject go in team1)
            {
                if (go.activeInHierarchy)
                {

                    Vector3 diff = go.transform.position - position;
                    float curDistance = diff.sqrMagnitude;

                    if (curDistance < distance)
                    {
                        closest = go;

                        distance = curDistance;
                    }

                } // end 1if activeInHierarchy
            }
        }
        else if (tag == "Team1") {
            foreach (GameObject go in team2)
            {

                if (go.activeInHierarchy)
                {

                    Vector3 diff = go.transform.position - position;
                    float curDistance = diff.sqrMagnitude;

                    if (curDistance < distance)
                    {
                        closest = go;

                        distance = curDistance;
                    }

                } // end if activeInHierarchy
            }

        }

        return closest;
    }
    public void Team1Dead() {

        total_team1 = total_team1 - 1;
        if (total_team1 <= 0) {
            txt.color = Color.blue;
            txt.text = "Blue WIN!!!";
        }

    }
    public void Team2Dead()
    {

        total_team2 = total_team2 - 1;
        if (total_team2 <= 0)
        {
            txt.color = Color.green;
            txt.text = "Green WIN!!!";
        }

    }
    public void Restart() {

        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }

}
