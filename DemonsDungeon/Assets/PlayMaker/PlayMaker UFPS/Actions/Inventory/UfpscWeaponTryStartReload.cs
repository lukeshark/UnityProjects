// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("Ultimate FPS Camera")]
	[Tooltip("Tries to start the Reload procedure the current Weapon")]
	public class UfpscWeaponTryStartReload : UfpscFPPlayerEventHandlerBase
	{
		[RequiredField]
		[CheckForComponent(typeof(vp_FPPlayerEventHandler))]
		public FsmOwnerDefault gameObject;
		
		[ActionSection("Result")]
		
		[UIHint(UIHint.Variable)]
		public FsmBool success;
		
		public FsmEvent successEvent;
		
		public FsmEvent failureEvent;
		
		public override void Reset()
		{
			gameObject = null;
			successEvent =null;
			failureEvent = null;
		}

		public override void OnEnter()
		{
			if (!base.Init(gameObject))
			{
				Finish();
				return;
			}
			
			TryStart();
			
			Finish();
		}

		void TryStart()
		{
			bool _success = _player.Reload.TryStart();
			
			success.Value = _success;
			if (_success)
			{
				Fsm.Event(successEvent);	
			}else{
				Fsm.Event(failureEvent);
			}
		}

	}
}