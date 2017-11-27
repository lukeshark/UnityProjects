// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("Ultimate FPS Camera")]
	[Tooltip("Remove Item from Inventory")]
	public class UfpscRemoveItem : FsmStateAction
	{
		[RequiredField]
		[CheckForComponent(typeof(vp_FPPlayerEventHandler))]
		public FsmOwnerDefault gameObject;
		
		[Tooltip("Name of the item to remove")]
		public FsmString itemName;
		
		
		public override void Reset()
		{
			gameObject = null;
			itemName = null;
		}

		public override void OnEnter()
		{
			RemoveItem();
			Finish();
		}

		void RemoveItem()
		{

			GameObject go = Fsm.GetOwnerDefaultTarget(gameObject);
			if (go == null)
			{
				return;
			}
			
			vp_FPPlayerEventHandler _player = go.GetComponent<vp_FPPlayerEventHandler>();
			
			if (_player == null)
			{
				return;
			}
			
			_player.RemoveItem.Try(new object[] { itemName.Value });
		}

	}
}