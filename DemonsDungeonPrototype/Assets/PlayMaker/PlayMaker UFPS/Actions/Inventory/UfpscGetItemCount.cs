// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("Ultimate FPS Camera")]
	[Tooltip("gets an item count")]
	public class UfpscGetItemCount : FsmStateAction
	{
		[RequiredField]
		[CheckForComponent(typeof(vp_FPPlayerEventHandler))]
		public FsmOwnerDefault gameObject;
		
		[Tooltip("Name of the item")]
		public FsmString itemName;
		
		[ActionSection("Result")]
		
		[Tooltip("The item count")]
		[UIHint(UIHint.Variable)]
		public FsmInt itemCount;
		
		public bool everyFrame;
		
		vp_FPPlayerEventHandler _player;
		
		public override void Reset()
		{
			gameObject = null;
			itemName = null;
			itemCount =null;
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
			
			getItemCount();
			
			if (!everyFrame)
			{
				Finish();
			}
		}
		public override void OnUpdate()
		{
			getItemCount();
		}
		
		void getItemCount()
		{
			itemCount.Value =  _player.GetItemCount.Send(itemName.Value);
		}

	}
}