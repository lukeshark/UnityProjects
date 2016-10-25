// (c) Copyright HutongGames, LLC 2010-2016. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("Photon")]
	[Tooltip("Remove all buffered RPCs from server that were sent by targetPlayer. Can only be called on local player (for 'self') or Master Client (for anyone).")]
	public class PhotonNetworkRemoveRpc : FsmStateAction
	{
		[Tooltip("The Player Id. Leave to none to target self")]
		public FsmInt playerId;

		[Tooltip("Send this event if the action was executed")]
		public FsmEvent successEvent;
		
		[Tooltip("Send this event if the action was not executed, likely because we are not on the master and photonPlayer doesn't point to self or player id is wrong.")]
		public FsmEvent failureEvent;

		public override void Reset()
		{
			playerId = new FsmInt() {UseVariable = true};
		}
		
		public override void OnEnter()
		{
			DoRemoveRp();
			
			Finish();
		}
		
		void DoRemoveRp()
		{
			if (playerId.IsNone)
			{
				PhotonNetwork.RemoveRPCs(PhotonNetwork.player);

			}else{

				if (!PhotonNetwork.isMasterClient)
				{
					Fsm.Event(failureEvent);
					return;
				}


				PhotonPlayer _target =	PhotonPlayer.Find(playerId.Value);
				if ( _target ==null)
				{
					LogError("Player Id '"+playerId.Value+"' not found");
					Fsm.Event(failureEvent);
					return;
				}

				PhotonNetwork.RemoveRPCs(_target);

			}

			Fsm.Event(successEvent);
		}
	}
}
