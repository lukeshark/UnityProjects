// (c) Copyright HutongGames, LLC 2010-2012. All rights reserved.

using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using PathologicalGames;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("Path-o-logical/Pool Manager 2")]
	[Tooltip("Check if a PrefabPool in this Pool exists already.")]
	[HelpUrl("https://hutonggames.fogbugz.com/default.asp?W840")]
	public class PmtCheckIfPrefabPoolExists : FsmStateAction
	{
		[ActionSection("Set up")]
		
		[RequiredField]
		[Tooltip("Pool Name.")]
		public FsmString poolName;

		[RequiredField]
		[Tooltip("Prefab")]
		public FsmGameObject prefab;
		
		[ActionSection("result")]
		
		[Tooltip("Trigger this event is the pool is already set with the 'prefab'.")]
		public FsmEvent alreadyExistsEvent;
		
		[Tooltip("Trigger this event if the pool doesn't contains prefabPool with 'prefab'")]
		public FsmEvent doesntExistsEvent;
		
		[Tooltip("Trigger this event if something went wrong")]
		public FsmEvent failureEvent;
		
		private SpawnPool _pool;
		
		public override void Reset ()
		{
			poolName = null;
			prefab = null;
			
			alreadyExistsEvent = null;
			doesntExistsEvent = null;
			failureEvent = null;
		}

		public override void  OnEnter ()
		{
			CheckPrefabPool ();
			
			Finish ();
		}

		void CheckPrefabPool ()
		{
			if (poolName.Value == "") {
				LogWarning("Pool name not set");
				if (failureEvent !=null)
				{
					Fsm.Event(failureEvent);
				}
				return;
			}
			
			//check and  if the pool does not exists
			if (! PoolManager.Pools.TryGetValue (poolName.Value, out _pool)) {
				LogWarning(string.Format("Pool {0} doesn't exists",poolName.Value));
				if (failureEvent !=null)
				{
					Fsm.Event(failureEvent);
				}
				return;
			}
			
			bool isAlreadyPool = _pool.GetPrefab(prefab.Value) == null ? false : true;
			
			if (alreadyExistsEvent != null && isAlreadyPool) {
				Fsm.Event (alreadyExistsEvent);
				return;
			}
			
			if (doesntExistsEvent !=null)
			{
				Fsm.Event(doesntExistsEvent);
				return;
			}
		}
		
	}
}