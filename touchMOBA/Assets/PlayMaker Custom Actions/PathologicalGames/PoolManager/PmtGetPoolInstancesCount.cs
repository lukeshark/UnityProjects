// (c) Copyright HutongGames, LLC 2010-2012. All rights reserved.

using UnityEngine;
using PathologicalGames;


namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("Path-o-logical/Pool Manager 2")]
	[Tooltip("Get the number of spawned instances in the pool.")]
	[HelpUrl("https://hutonggames.fogbugz.com/default.asp?W847")]
	public class PmtGetPoolInstancesCount : FsmStateAction
	{
		
		[RequiredField]
		[Tooltip("The name of the pool")]
		public FsmString poolName;
		
		[RequiredField]
		[UIHint(UIHint.Variable)]
		[Tooltip("The number of spawned instances in the pool.")]
		public FsmInt instancesCount;
	
		[Tooltip("Trigger this event if something went wrong")]
		public FsmEvent failureEvent;
		
		[ActionSection("")] 
		
		[Tooltip("Repeat every frame.")]
		public bool everyFrame;
		
		private SpawnPool _pool;
		
		public override void Reset()
		{
			poolName = null;
			instancesCount = null;
			everyFrame = false;
		}

		public override void OnEnter()
		{
			doGetPoolInstancesCount();
			
			if (!everyFrame)
			{
				Finish();
			}
		}
		
		public override void  OnUpdate()
		{
 			 doGetPoolInstancesCount();
		}
		
		void doGetPoolInstancesCount()
		{
			if (poolName.Value == "")
			{
				return;
			}
			
				//check and  if the pool exists
			if (! PoolManager.Pools.TryGetValue (poolName.Value, out _pool)) {
				LogWarning(string.Format("Pool {0} doesn't exists",poolName.Value));
				if (failureEvent !=null)
				{
					Fsm.Event(failureEvent);
				}
				return;
			}
			
			instancesCount.Value =	_pool.Count;	
		}
		
	}
}

