// (c) Copyright HutongGames, LLC 2010-2015. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("Photon")]
	[Tooltip("Attempts to remove all current expected users from the server's Slot Reservation list.")]
	//[HelpUrl("https://hutonggames.fogbugz.com/default.asp?W915")]
	public class PhotonNetworkRoomClearExpectedUsers : FsmStateAction
	{
		[Tooltip("Send this event if the action was executed")]
		public FsmEvent successEvent;
		
		[Tooltip("Send this event if the action was not executed, likely because we are not in a room.")]
		public FsmEvent failureEvent;

		public override void OnEnter()
		{
			if (PhotonNetwork.room == null)
			{
				Fsm.Event(failureEvent);
			}

			PhotonNetwork.room.ClearExpectedUsers();

			Fsm.Event(successEvent);

			Finish();
		}
	}
}