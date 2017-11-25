// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("Ultimate FPS Camera")]
	[Tooltip("Sets the player health")]
	public class UfpscSetHealth : FsmStateAction
	{
		[RequiredField]
		[CheckForComponent(typeof(vp_FPPlayerEventHandler))]
		public FsmOwnerDefault gameObject;
		
		[ActionSection("Result")]
		
		[Tooltip("The new health")]
		public FsmFloat health;
		
		
		vp_FPPlayerEventHandler _player;
		
		public override void Reset()
		{
			gameObject = null;
			health = null;
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
			
			setHealth();
		}

		void setHealth()
		{
			_player.Health.Set(health.Value);
		}

	}
}