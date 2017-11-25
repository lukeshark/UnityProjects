// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("Ultimate FPS Camera")]
	[Tooltip("Sets the player weapon")]
	public class UfpscSetWeaponByName : FsmStateAction
	{
		[RequiredField]
		[CheckForComponent(typeof(vp_FPPlayerEventHandler))]
		public FsmOwnerDefault gameObject;
		
		[Tooltip("The weapon Name")]
		[RequiredField]
		public FsmString weaponName;
		
		vp_FPPlayerEventHandler _player;
		
		public override void Reset()
		{
			gameObject = null;
			weaponName = null;
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
			
			setWeapon();
		}

		void setWeapon()
		{
			_player.SetWeaponByName.Try(weaponName.Value);		
		}

	}
}