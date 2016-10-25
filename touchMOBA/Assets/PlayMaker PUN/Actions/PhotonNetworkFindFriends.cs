// (c) Copyright HutongGames, LLC 2010-2016. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("Photon")]
	[Tooltip("Requests the rooms and online status for a list of friends. Watch for PHOTON / FRIEND LIST UPDATED to start getting friends data")]
	[HelpUrl("")]
	public class PhotonNetworkFindFriends : FsmStateAction
	{
		[RequiredField]
		[Tooltip("The list of friends ")]
		[UIHint(UIHint.Variable)]
		[ArrayEditor(VariableType.String)]
		public FsmArray friendlist;

		[Tooltip("Event sent if call failed, likely because it can only be requested if in lobby, not within a room")]
		public FsmEvent failureEvent;


		public override void Reset()
		{
			friendlist = null;
		}
		
		public override void OnEnter()
		{

			if (PhotonNetwork.inRoom)
			{
				Fsm.Event(failureEvent);
			}else{
				PhotonNetwork.FindFriends(friendlist.stringValues);
			}

			Finish();
		}

	}
}