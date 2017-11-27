// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("Ultimate FPS Camera")]
	[Tooltip("gets the player health")]
	public class UfpscGetHealth : FsmStateAction
	{
		[RequiredField]
		[CheckForComponent(typeof(vp_FPPlayerEventHandler))]
		public FsmOwnerDefault gameObject;
		
		public FsmFloat multiplier;
		
		[ActionSection("Result")]
		
		[Tooltip("The health")]
		[UIHint(UIHint.Variable)]
		public FsmFloat health;
		
		public bool everyFrame;
		
		vp_FPPlayerEventHandler _player;
		
		public override void Reset()
		{
			gameObject = null;
			multiplier = 100f;
			health = null;
			everyFrame = false;
		}

		public override void OnEnter()
		{
			GameObject go = Fsm.GetOwnerDefaultTarget(gameObject);
			if (go == null)
			{
				Finish();
				return;
			}
			
			_player = go.GetComponent<vp_FPPlayerEventHandler>();
			
			if (_player == null)
			{
				Finish();
				return;
			}
			
			getHealth();
			
			if (!everyFrame)
			{
				Finish();
			}
		}
		public override void OnUpdate()
		{
			getHealth();
		}
		
		void getHealth()
		{
			health.Value =  _player.Health.Get() * multiplier.Value;
		}

	}
}