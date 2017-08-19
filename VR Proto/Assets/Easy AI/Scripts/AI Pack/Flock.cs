using UnityEngine;
using System.Collections;
namespace AxlPlay {
//Flock around the scene using the Unity NavMesh.
[AddComponentMenu("Easy AI/Flock")]

public class Flock : MonoBehaviour
{
    [Tooltip("Agents less than this distance apart are neighbors")]
    public float neighborDistance = 100f;
    [Tooltip("How far the agent should look ahead when determine its pathfinding destination")]
    public float lookAheadDistance = 5f;
    [Tooltip("The greater the alignmentWeight is the more likely it is that the agents will be facing the same direction")]
    public float alignmentWeight = 0.4f;
    [Tooltip("The greater the cohesionWeight is the more likely it is that the agents will be moving towards a common position")]
    public float cohesionWeight = 0.5f;
    [Tooltip("The greater the separationWeight is the more likely it is that the agents will be separated")]
    public float separationWeight = 0.6f;
    [Tooltip("All of the agents")]
    public GameObject[] agents = null;
    [Tooltip("The speed of the agents")]
    public float speed = 10;
    [Tooltip("The angular speed of the agents")]
    public float angularSpeed = 120;

    // A cache of the NavMeshAgents
    private UnityEngine.AI.NavMeshAgent[] navMeshAgents;
    protected Transform[] transforms;

    void Start()
    {
        navMeshAgents = new UnityEngine.AI.NavMeshAgent[agents.Length];
        transforms = new Transform[agents.Length];
        for (int i = 0; i < agents.Length; ++i)
        {
            transforms[i] = agents[i].transform;
            navMeshAgents[i] = agents[i].GetComponent<UnityEngine.AI.NavMeshAgent>();
            navMeshAgents[i].speed = speed;
            navMeshAgents[i].angularSpeed = angularSpeed;
            navMeshAgents[i].Resume();
        }
    }
    void Update()
    {
        // Determine a destination for each agent
        for (int i = 0; i < agents.Length; ++i)
        {
            Vector3 alignment, cohesion, separation;
            // determineFlockAttributes will determine which direction to head, which common position to move toward, and how far apart each agent is from one another,
            DetermineFlockParameters(i, out alignment, out cohesion, out separation);
            // Weigh each parameter to give one more of an influence than another
            var velocity = alignment * alignmentWeight + cohesion * cohesionWeight + separation * separationWeight;
            // Set the destination based on the velocity multiplied by the look ahead distance
            if (!SetDestination(i, transforms[i].position + velocity * lookAheadDistance))
            {
                // Go the opposite direction if the destination is invalid
                velocity *= -1;
                SetDestination(i, transforms[i].position + velocity * lookAheadDistance);
            }
        }
    }
    private void DetermineFlockParameters(int index, out Vector3 alignment, out Vector3 cohesion, out Vector3 separation)
    {
        alignment = cohesion = separation = Vector3.zero;
        int neighborCount = 0;
        var agentTransform = transforms[index];
        // Loop through each agent to determine the alignment, cohesion, and separation
        for (int i = 0; i < agents.Length; ++i)
        {
            // The agent can't compare against itself
            if (index != i)
            {
                // Only determine the parameters if the other agent is its neighbor
                if (Vector3.Magnitude(transforms[i].position - agentTransform.position) < neighborDistance)
                {
                    // This agent is the neighbor of the original agent so add the alignment, cohesion, and separation
                    alignment += Velocity(i);
                    cohesion += transforms[i].position;
                    separation += transforms[i].position - agentTransform.position;
                    neighborCount++;
                }
            }
        }

        // Don't move if there are no neighbors
        if (neighborCount == 0)
        {
            return;
        }
        // Normalize all of the values
        alignment = (alignment / neighborCount).normalized;
        cohesion = ((cohesion / neighborCount) - agentTransform.position).normalized;
        separation = ((separation / neighborCount) * -1).normalized;
    }

    bool SetDestination(int index, Vector3 target)
    {
        if (navMeshAgents[index].destination == target)
        {
            return true;
        }
        return navMeshAgents[index].SetDestination(target);
    }
    Vector3 Velocity(int index)
    {
        return navMeshAgents[index].velocity;
    }
}
}
