// (c) Copyright HutongGames, LLC 2010-2012. All rights reserved.

using System;
using UnityEngine;
using PathologicalGames;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("Path-o-logical/Pool Manager 2")]
	[Tooltip("Creates a new GameObject with a SpawnPool Component which registers itself " +
		"with the PoolManager.Pools dictionary. The SpawnPool can then be accessed " +
		"directly via the return value of this function or by via the PoolManager.Pools " +
		"dictionary using a 'key' (string : the name of the pool, SpawnPool.poolName).")]
	[HelpUrl("https://hutonggames.fogbugz.com/default.asp?W841")]
	public class PmtCreatePool : FsmStateAction
	{
		[ActionSection("Setup")]
		
		[RequiredField]
		[Tooltip("Pool Name. The work 'Pool' will be added to this name.")]
		public FsmString poolName;
		
		[Tooltip("If TRUE, survive when loading a new scene")]
		public FsmBool dontDestroyOnLoad;
		
		[Tooltip("If TRUE, log pool activity")]
		public FsmBool logMessages;		
	
		[ActionSection("Result")]
		
		[Tooltip("The Pool")]
		[UIHint(UIHint.Variable)]
		[ObjectType(typeof(SpawnPool))]
		public FsmObject pool;	
		
		[Tooltip("The Pool GameObject.")]
		[UIHint(UIHint.Variable)]
		public FsmGameObject poolGameObject;	
		
		[Tooltip("Will be true if a pool with that name exists already")]
		[UIHint(UIHint.Variable)]
		public FsmBool poolExistsAlready;
		
		[Tooltip("Event sent if a pool with that name exists already")]
		public FsmEvent poolExistsAlreadyEvent;
		
		[Tooltip("Event sent if the pool is created")]
		public FsmEvent poolCreatedEvent;
		
		[Tooltip("Event sent if the pool creation failed")]
		public FsmEvent poolCreationFailedEvent;
			
		
		public override void Reset()
		{
			poolName = null;
			
			dontDestroyOnLoad = false;
			logMessages = false;
			
			pool = null;
			poolGameObject = null;
			
			poolExistsAlready = null;
			poolExistsAlreadyEvent = null;
			poolCreatedEvent = null;
			poolCreationFailedEvent = null;
		}

		public override void  OnEnter()
		{
			_createPool();
			Finish();
		}
		
		void _createPool()
		{
			
			if(poolName.Value == ""){
				return;
			}
			
			SpawnPool _pool;
			bool _exists = PoolManager.Pools.TryGetValue(poolName.Value,out _pool);
			
			// check if the pool already exists
			if (!_exists)
			{
				_pool = PoolManager.Pools.Create(poolName.Value);	
			}
			
			if (_pool == null)
			{
				if (poolCreationFailedEvent !=null)
				{
					Fsm.Event(poolCreationFailedEvent);
				}
				return;		
			}
			
			pool.Value = _pool;
			if (dontDestroyOnLoad !=null){
				_pool.dontDestroyOnLoad = dontDestroyOnLoad.Value;
			}
			
			if(logMessages !=null){
				_pool.logMessages = logMessages.Value;
			}
			
			poolGameObject.Value = _pool.group.gameObject;
			
			if (_exists && poolExistsAlreadyEvent !=null)
			{
				Fsm.Event(poolExistsAlreadyEvent);
				return;
			}
			
			if (poolCreatedEvent!=null)
			{
				Fsm.Event(poolCreatedEvent);	
			}
		}
		
	}
}