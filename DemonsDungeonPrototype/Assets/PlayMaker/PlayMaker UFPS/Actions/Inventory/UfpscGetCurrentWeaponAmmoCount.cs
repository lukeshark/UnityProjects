// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("Ultimate FPS Camera")]
	[Tooltip("gets the current weapon's ammo count")]
	public class UfpscGetCurrentWeaponAmmoCount : FsmStateAction
	{
		[RequiredField]
		[CheckForComponent(typeof(vp_FPPlayerEventHandler))]
		public FsmOwnerDefault gameObject;

		[ActionSection("Result")]
		
		[Tooltip("The current weapon Ammo count")]
		[UIHint(UIHint.Variable)]
		public FsmInt ammoCount;
		
		public bool everyFrame;
		
		vp_FPPlayerEventHandler _player;
		
		public override void Reset()
		{
			gameObject = null;
			ammoCount =null;
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
			
			getAmmoCount();
			
			if (!everyFrame)
			{
				Finish();
			}
		}
		public override void OnUpdate()
		{
			getAmmoCount();
		}
		
		void getAmmoCount()
		{
			ammoCount.Value = _player.CurrentWeaponAmmoCount.Get();
		}

	}
}