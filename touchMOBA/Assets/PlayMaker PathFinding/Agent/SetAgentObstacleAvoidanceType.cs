// (c) Copyright HutongGames, LLC 2010-2014. All rights reserved.
//
// TODO: ERROR CHECK to warn the user if he has set too many of the available options

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.NavMeshAgent)]
	[Tooltip("Set the agent obstacle avoidance type of a NavMesh Agent. \nUse either a string or an int: range: no:(0), low:(1), medium:(2), good(3), high(4). \nNOTE: The Game Object must have a NavMeshAgent component attached.")]
	public class SetAgentObstacleAvoidanceType : FsmStateAction
	{
	
		[RequiredField]
		[Tooltip("The Game Object to work with. NOTE: The Game Object must have a NavMeshAgent component attached.")]
		[CheckForComponent(typeof(NavMeshAgent))]
		public FsmOwnerDefault gameObject;
		
		[Tooltip("The agent level of quality of avoidance")]
		public ObstacleAvoidanceType quality;
		
		[Tooltip("OR The agent level of quality of avoidance as a string. Range: no, low, medium, good, high]")]
		[UIHint(UIHint.Variable)]
		public FsmString orQualityFromString;	
		
		[Tooltip("OR The agent level of quality of avoidance as an int. Range: no:(0), low:(1), medium:(2), good(3), high(4))]")]
		[UIHint(UIHint.Variable)]
		public FsmInt orQualityFromInt;
		
		private NavMeshAgent _agent;
		
		private void _getAgent()
		{
			GameObject go = Fsm.GetOwnerDefaultTarget(gameObject);
			if (go == null) 
			{
				return;
			}
			
			_agent =  go.GetComponent<NavMeshAgent>();
		}	
		
		public override void Reset()
		{
			gameObject = null;
			quality = ObstacleAvoidanceType.NoObstacleAvoidance;
			orQualityFromString = null;
			orQualityFromInt = null;

		}

		public override void OnEnter()
		{
			_getAgent();
			
			DoSetObstacleAvoidanceType();

			Finish();		
		}
		
		void DoSetObstacleAvoidanceType()
		{
			if (_agent == null) 
			{
				return;
			}
			
			ObstacleAvoidanceType oat = quality;
			
			
			if (! orQualityFromInt.IsNone)
			{
				switch( orQualityFromInt.Value)
				{
					case 0:
						oat = ObstacleAvoidanceType.NoObstacleAvoidance;
						break;
					case 1:
						oat = ObstacleAvoidanceType.LowQualityObstacleAvoidance;
						break;	
					case 2:
						oat = ObstacleAvoidanceType.MedQualityObstacleAvoidance;
						break;
					case 3:
						oat = ObstacleAvoidanceType.GoodQualityObstacleAvoidance;
						break;
					case 4:
						oat = ObstacleAvoidanceType.HighQualityObstacleAvoidance;
						break;
				}
			}else if (!orQualityFromString.IsNone){
				
				switch( orQualityFromString.Value )
				{
					case "no":
						oat = ObstacleAvoidanceType.NoObstacleAvoidance;
						break;
					case "low":
						oat = ObstacleAvoidanceType.LowQualityObstacleAvoidance;
						break;	
					case "medium":
						oat = ObstacleAvoidanceType.MedQualityObstacleAvoidance;
						break;
					case "good":
						oat = ObstacleAvoidanceType.GoodQualityObstacleAvoidance;
						break;
					case "high":
						oat = ObstacleAvoidanceType.HighQualityObstacleAvoidance;
						break;
				}			
			}
			
			_agent.obstacleAvoidanceType = oat;
			
			
		}

	}
}