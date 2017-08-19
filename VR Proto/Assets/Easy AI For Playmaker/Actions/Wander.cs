using UnityEngine;
using AxlPlay;
#if UNITY_5_5_OR_NEWER
using UnityEngine.AI;
#endif


namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("Agents AI")]
    [Tooltip("Wander nav agent.")]
    public class Wander : FsmStateAction
    {
        [RequiredField]
        [CheckForComponent(typeof(NavMeshAgent))]

        public FsmOwnerDefault gameObject;
        [Tooltip("How far ahead of the current position to look ahead for a wander")]
        public FsmFloat wanderDistance = 20;
        [Tooltip("The amount that the agent rotates direction")]
        public FsmFloat wanderRate = 2;
	    [Tooltip("The speed of the agent.")]
	    public FsmFloat wanderSpeed = 2;
	 
        private GameObject go;
        private NavMeshAgent agent;
	    private NavMeshPath path;
	    private Vector3 startPos;
	    
        // Reset the public variables
        public override void Reset()
        {
            wanderDistance = 20;
	        wanderRate = 2;
        }
 
        public override void OnEnter()
	    {
		    go = Fsm.GetOwnerDefaultTarget(gameObject);
		    agent = go.GetComponent<NavMeshAgent>();
		    path = new NavMeshPath();	
		    
		    startPos = go.transform.position;
		    
		    SetDestination(Target());
		    
		    
        }
       
        public override void OnUpdate()
        {

            if (HasArrived())
            {
                SetDestination(Target());
            }

        }
        private Vector3 Target()
        {
           // // point in a new random direction and then multiply that by the wander distance
 	        	   
	        Vector3 point;
	        
	        for (int i = 0; i < 40; i++) {
	        	if (RandomPoint(go.transform.position, wanderDistance.Value, out point)) {
		        	//Debug.DrawRay(point, Vector3.up, Color.blue, 1.0f);
		        	//Debug.Log(i);
		        	return point;
	        	}	
	        }
	        //Debug.Log("startPos");
 	        return startPos;
	        
		}
	    bool RandomPoint(Vector3 center, float range, out Vector3 result) {
			    Vector3 randomPoint = center + Random.insideUnitSphere * range;
			    NavMeshHit hit;
			    if (NavMesh.SamplePosition(randomPoint, out hit, 1.0f, NavMesh.AllAreas)){
				    if (NavMesh.CalculatePath(go.transform.position, hit.position, NavMesh.AllAreas, path)){
					    
					    
					    switch (path.status)
					    {
					    case NavMeshPathStatus.PathComplete:
						    result = hit.position;
						    return true;
					    case NavMeshPathStatus.PathPartial:
						    result = Vector3.zero;
						    return false;
						    
						    
					    case NavMeshPathStatus.PathInvalid:
						    result = Vector3.zero;
						    return false;
						    
					    }
					    
				    }
			    }
		    result = Vector3.zero;
		    return false;
	    }
        private  bool SetDestination(Vector3 target)
        {
            if (agent.destination == target)
            {
                return true;
            }
            if (agent.SetDestination(target))
            {
                return true;
            }
            return false;
        }

        private bool HasArrived()
        {
            return !agent.pathPending && agent.remainingDistance <= agent.stoppingDistance + 0.001f;
        }
	    public Vector3 RandomNavSphere(Vector3 origin, float dist, int layermask) {
		    Vector3 randDirection = Random.insideUnitSphere * dist;
		    
		    randDirection += origin;
		    
		    NavMeshHit navHit;
		    
		    NavMesh.SamplePosition (randDirection, out navHit, dist, layermask);
		    
		    return navHit.position;
	    }
 

    }
}
