// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("Ultimate FPS Camera")]
	[Tooltip("gets the current weapon's clip type")]
	public class UfpscGetCurrentWeaponClipType : FsmStateAction
	{
		[RequiredField]
		[CheckForComponent(typeof(vp_FPPlayerEventHandler))]
		public FsmOwnerDefault gameObject;

		[ActionSection("Result")]
		
		[Tooltip("The current weapon clip type")]
		[UIHint(UIHint.Variable)]
		public FsmString clipType;
		
		vp_FPPlayerEventHandler _player;
		
		public override void Reset()
		{
			gameObject = null;
			clipType = null;
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
			
			getClipType();

		}

		void getClipType()
		{
			clipType.Value = _player.CurrentWeaponClipType.Get();
		}

	}
}