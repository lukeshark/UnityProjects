// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("Ultimate FPS Camera")]
	[Tooltip("gets the Active state of the Dead activity")]
	public class UfpscIsDeadActive : UfpscFPPlayerEventHandlerBase
	{
		[RequiredField]
		[CheckForComponent(typeof(vp_FPPlayerEventHandler))]
		public FsmOwnerDefault gameObject;
		
		
		[ActionSection("Result")]
		
		[UIHint(UIHint.Variable)]
		public FsmBool active;
		
		public FsmEvent activeEvent;
		
		public FsmEvent notActiveEvent;
		
		public override void Reset()
		{
			gameObject = null;
			activeEvent =null;
			notActiveEvent = null;
		}

		public override void OnEnter()
		{
			if (!base.Init(gameObject))
			{
				Finish();
				return;
			}
			
			CheckActive();
			
			Finish();
		}

		void CheckActive()
		{
			bool _active = _player.Dead.Active;
			
			active.Value = _active;
			if (_active)
			{
				Fsm.Event(activeEvent);	
			}else{
				Fsm.Event(notActiveEvent);
			}
		}

	}
}