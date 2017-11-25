// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	public abstract class UfpscFPPlayerEventHandlerBase : FsmStateAction
	{

		protected vp_FPPlayerEventHandler _player;

		protected bool Init(FsmOwnerDefault gameObject)
		{
			GameObject go = Fsm.GetOwnerDefaultTarget(gameObject);
			if (go == null)
			{
				return false;
			}
			
			_player = go.GetComponent<vp_FPPlayerEventHandler>();
			
			if (_player == null)
			{
				return false;
			}
			
			return true;
		}
	}
}