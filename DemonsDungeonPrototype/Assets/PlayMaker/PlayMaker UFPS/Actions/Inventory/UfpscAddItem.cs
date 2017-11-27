// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("Ultimate FPS Camera")]
	[Tooltip("Adds and Item to the player inventory")]
	public class UfpscAddItem : UfpscFPPlayerEventHandlerBase
	{
		[RequiredField]
		[CheckForComponent(typeof(vp_FPPlayerEventHandler))]
		public FsmOwnerDefault gameObject;
		
		[Tooltip("Item")]
		[RequiredField]
		public FsmObject item;
		
		[Tooltip("Ammo quantity to add")]
		[RequiredField]
		public FsmInt quantity;
		
		[ActionSection("Result")]
		
		[UIHint(UIHint.Variable)]
		public FsmBool success;
		
		public FsmEvent successEvent;
		
		public FsmEvent failureEvent;
		
		public override void Reset()
		{
			gameObject = null;
			item = null;
			quantity = 1;
			
			success = null;
			successEvent = null;
			failureEvent = null;
		}

		public override void OnEnter()
		{
			if (!base.Init(gameObject))
			{
				Finish();
				return;
			}
			
			addItem();
		}

		void addItem()
		{
						bool _success =	_player.AddItem.Try(new object[] { item.Value, quantity.Value });
			
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