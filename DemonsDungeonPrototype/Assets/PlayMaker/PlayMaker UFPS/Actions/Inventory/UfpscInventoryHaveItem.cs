// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("Ultimate FPS Camera")]
	[Tooltip("Check if Inventory have a given item")]
	public class UfpscInventoryHaveItem : FsmStateAction
	{
		[RequiredField]
				[CheckForComponent(typeof(vp_FPInventory))]
		public FsmOwnerDefault gameObject;
		
		[Tooltip("Item Type")]
		[ObjectType(typeof(vp_ItemType))]
		public FsmObject itemType;


		[Tooltip("Id of the item")]
		public FsmInt itemId;
		
		[ActionSection("Result")]
		
		[Tooltip("Store the result")]
		public FsmBool hasItem;
		
		[Tooltip("Event sent if item is present in inventory")]
		public FsmEvent hasItemEvent;
		
		[Tooltip("Event sent if item not found in inventory")]
		public FsmEvent hasNotItemEvent;
		
		
		public override void Reset()
		{
			gameObject = null;
			itemType = null;
			itemId = null;
			hasItem = null;
			hasItemEvent = null;
			hasNotItemEvent = null;
		}

		public override void OnEnter()
		{
			bool _hasItem = GetHasItem();
			
			hasItem.Value = _hasItem;
			if (_hasItem)
			{
				Fsm.Event(hasItemEvent);
			}else{
				Fsm.Event(hasNotItemEvent);
			}
	
			Finish();
		}

		bool GetHasItem()
		{

			GameObject go = Fsm.GetOwnerDefaultTarget(gameObject);
			if (go == null)
			{
				return false;
			}
			
			vp_FPInventory _inv = go.GetComponent<vp_FPInventory> ();
			
			if (_inv == null)
			{
				return false;
			}
			

			return _inv.HaveItem(itemType.Value as vp_ItemType ,itemId.Value);
		}

	}
}