// (c) Copyright HutongGames, LLC 2010-2012. All rights reserved.

using UnityEngine;
using PathologicalGames;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("Path-o-logical/Pool Manager 2")]
	[Tooltip("Despawns all active instances in this SpawnPool")]
	[HelpUrl("https://hutonggames.fogbugz.com/default.asp?W1452")]
	public class PmtDeSpawnAll : FsmStateAction
	{
		[RequiredField] 
		[Tooltip("Pool name")]
		public FsmString poolName;
	
		[ActionSection(" ")]
		
		[Tooltip("Trigger this event if something went wrong")]
		public FsmEvent failureEvent;
		
		private SpawnPool _pool;
		
		public override void Reset()
		{
			poolName = null;
		}

		public override void OnEnter()
		{
			DoDeSpawnAll();
			Finish();
		}

		void DoDeSpawnAll()
		{
			if (poolName.Value == "")
			{
				return;
			}

			//check and  if the pool does not exists
			if (! PoolManager.Pools.TryGetValue (poolName.Value, out _pool)) {
				LogWarning (string.Format("Pool {0} doesn't exists",poolName.Value));
				if (failureEvent !=null)
				{
					Fsm.Event(failureEvent);
				}
				return;
			}
	
			_pool.DespawnAll();
		}
	}
}

