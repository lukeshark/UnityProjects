// (c) Copyright HutongGames, LLC 2010-2016. All rights reserved.
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("Photon")]
	[Tooltip("Get the inRoom status of the PhotonNetwork Friends list. Make sure to listen to PHOTON / FRIEND LIST UPDATED before using this action. Use PhotonNetworkFindFriends to request the list from the server")]
	[HelpUrl("")]
	public class PhotonNetworkGetFriendsinRoomStatus : FsmStateAction
	{

		[Tooltip("The list of friends in Room status. True for is in a Room, false otherwise")]
		[UIHint(UIHint.Variable)]
		[ArrayEditor(VariableType.Bool)]
		public FsmArray friendsInRoomStatus;

		[Tooltip("The list of friends id currently in a room (busy to play)")]
		[UIHint(UIHint.Variable)]
		[ArrayEditor(VariableType.String)]
		public FsmArray inRoomFriends;

		[Tooltip("The list of friends id currently in lobby (waiting to play)")]
		[UIHint(UIHint.Variable)]
		[ArrayEditor(VariableType.String)]
		public FsmArray inLobbyFriends;

		[Tooltip("Event Sent if FriendList was available")]
		public FsmEvent successEvent;

		[Tooltip("Event Sent if FriendList was not available, in which case you need to use PhotonNetworkFindFriends action")]
		public FsmEvent failureEvent;

		public override void Reset()
		{
			friendsInRoomStatus = null;
			inRoomFriends = null;
			inLobbyFriends = null;
		}
		
		public override void OnEnter()
		{
			if (PhotonNetwork.Friends==null)
			{
				Fsm.Event(failureEvent);
				Finish();
				return;
			}
			GetInRoomStatus();
			GetFriendsInRoom();
			GetFriendsInLobby();

			Fsm.Event(successEvent);

			Finish();
		}


		void GetInRoomStatus()
		{
			
			if (friendsInRoomStatus.IsNone)
			{
				return;
			}
			
			bool[] _result = new bool[PhotonNetwork.Friends.Count];
			int i =0;
			foreach(FriendInfo _friendInfo in PhotonNetwork.Friends)
			{
				_result[i] = _friendInfo.IsInRoom;
				i++;
			}
			
			friendsInRoomStatus.Resize(_result.Length);
			friendsInRoomStatus.boolValues = _result;
		}



		void GetFriendsInLobby()
		{
			
			if (inLobbyFriends.IsNone)
			{
				return;
			}
			
			List<string> _result = new List<string>();
			
			foreach(FriendInfo _friendInfo in PhotonNetwork.Friends)
			{
				if (_friendInfo.IsOnline && !_friendInfo.IsInRoom)
				{
					_result.Add(_friendInfo.Name);
				}
				
			}
			inLobbyFriends.Resize(_result.Count);
			inLobbyFriends.Values = _result.ToArray();
		}

		void GetFriendsInRoom()
		{
			
			if (inRoomFriends.IsNone)
			{
				return;
			}
			
			List<string> _result = new List<string>();
			
			foreach(FriendInfo _friendInfo in PhotonNetwork.Friends)
			{
				if (_friendInfo.IsInRoom)
				{
					_result.Add(_friendInfo.Name);
				}
				
			}
			inRoomFriends.Resize(_result.Count);
			inRoomFriends.Values = _result.ToArray();
		}
		
	}
}
