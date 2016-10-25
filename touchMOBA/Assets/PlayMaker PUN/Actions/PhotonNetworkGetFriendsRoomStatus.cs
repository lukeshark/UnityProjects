// (c) Copyright HutongGames, LLC 2010-2016. All rights reserved.
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("Photon")]
	[Tooltip("Get the room names for each friend of the PhotonNetwork Friends list. Make sure to listen to PHOTON / FRIEND LIST UPDATED before using this action. Use PhotonNetworkFindFriends to request the list from the server")]
	[HelpUrl("")]
	public class PhotonNetworkGetFriendsRoomStatus : FsmStateAction
	{

		[Tooltip("The list of friends room name status.")]
		[UIHint(UIHint.Variable)]
		[ArrayEditor(VariableType.String)]
		public FsmArray friendsRoomStatus;

		[Tooltip("Event Sent if FriendList was available")]
		public FsmEvent successEvent;

		[Tooltip("Event Sent if FriendList was not available, in which case you need to use PhotonNetworkFindFriends action")]
		public FsmEvent failureEvent;

		public override void Reset()
		{
			friendsRoomStatus = null;
		}
		
		public override void OnEnter()
		{
			if (PhotonNetwork.Friends==null)
			{
				Fsm.Event(failureEvent);
				Finish();
				return;
			}

			GetRoomStatus();

			Fsm.Event(successEvent);

			Finish();
		}


		void GetRoomStatus()
		{
			
			if (friendsRoomStatus.IsNone)
			{
				return;
			}
			
			string[] _result = new string[PhotonNetwork.Friends.Count];
			int i =0;
			foreach(FriendInfo _friendInfo in PhotonNetwork.Friends)
			{
				_result[i] = _friendInfo.Room;
				i++;
			}
			
			friendsRoomStatus.Resize(_result.Length);
			friendsRoomStatus.Values = _result;
		}

		
	}
}
