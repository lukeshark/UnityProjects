// (c) Copyright HutongGames, LLC 2010-2012. All rights reserved.

using UnityEngine;
using PathologicalGames;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("Path-o-logical/Pool Manager 2")]
	[Tooltip("If the passed transform is managed by the SpawnPool, it will be deactivated and made available to be spawned again.")]
	[HelpUrl("https://hutonggames.fogbugz.com/default.asp?W843")]
	public class PmtDeSpawn : FsmStateAction
	{
		[RequiredField] 
		[Tooltip("Pool name")]
		public FsmString poolName;

		[RequiredField]
		[Tooltip("GameObject or prefab to deSpawn")]
		public FsmGameObject gameObject;
		
		[Tooltip("Will parent the instance back under its pool's group before calling Despawn().")]
		public FsmBool reParent;
		
		[Tooltip("delay")]
		public FsmFloat delay;
	
		[ActionSection(" ")]
		
		[Tooltip("Trigger this event if something went wrong")]
		public FsmEvent failureEvent;
		
		private SpawnPool _pool;
		
		public override void Reset()
		{
			poolName = null;
			gameObject = null;
			reParent = false;
			delay = 0;
		}

		public override void OnEnter()
		{
			DoDeSpawn();
			Finish();
		}

		void DoDeSpawn()
		{
			if (poolName.Value == "")
			{
				return;
			}
			
			if (gameObject.Value == null)
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
			
			
			if (reParent.Value)
			{
				gameObject.Value.transform.parent = _pool.group;
			}
			
			if (delay.Value >0){
				_pool.Despawn(gameObject.Value.transform, delay.Value);
			}else{
				_pool.Despawn(gameObject.Value.transform);
			}
			
		}
	}
}

