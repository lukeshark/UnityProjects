using UnityEngine;
#if UNITY_5_5_OR_NEWER
using UnityEngine.AI;
#endif
namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory ("Agents AI")]
	[Tooltip ("Resume the nav mesh agent")]
	public class AgentResume : FsmStateAction
	{
		private NavMeshAgent agent;
		private bool error;

 
		public override void OnEnter ()
		{
			agent = Owner.GetComponent<NavMeshAgent> ();
			if (agent != null) {
				    agent.Resume ();
			}
		}
	}

}