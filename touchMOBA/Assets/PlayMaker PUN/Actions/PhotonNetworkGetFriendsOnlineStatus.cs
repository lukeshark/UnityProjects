// (c) Copyright HutongGames, LLC 2010-2016. All rights reserved.
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("Photon")]
	[Tooltip("Get the online status of the PhotonNetwork Friends list. Make sure to listen to PHOTON / FRIEND LIST UPDATED before using this action. Use PhotonNetworkFindFriends to request the list from the server")]
	[HelpUrl("")]
	public class PhotonNetworkGetFriendsOnlineStatus : FsmStateAction
	{

		[Tooltip("The list of friends online status. true for online, false for offline")]
		[UIHint(UIHint.Variable)]
		[ArrayEditor(VariableType.Bool)]
		public FsmArray friendsOnlineStatus;

		[Tooltip("The list of friends id currently online")]
		[UIHint(UIHint.Variable)]
		[ArrayEditor(VariableType.String)]
		public FsmArray onlineFriends;

		[Tooltip("The list of friends id currently offline")]
		[UIHint(UIHint.Variable)]
		[ArrayEditor(VariableType.String)]
		public FsmArray offlineFriends;

		[Tooltip("Event Sent if FriendList was available")]
		public FsmEvent successEvent;

		[Tooltip("Event Sent if FriendList was not available, in which case you need to use PhotonNetworkFindFriends action")]
		public FsmEvent failureEvent;

		public override void Reset()
		{
			friendsOnlineStatus = null;

			onlineFriends = null;
			offlineFriends = null;
		}
		
		public override void OnEnter()
		{
			if (PhotonNetwork.Friends==null)
			{
				Fsm.Event(failureEvent);
				Finish();
				return;
			}

			GetOnlineStatus();
			GetOnlineFriends();
			GetOfflineFriends();
		
			Fsm.Event(successEvent);

			Finish();
		}

		void GetOnlineStatus()
		{

			if (friendsOnlineStatus.IsNone)
			{
				return;
			}

			bool[] _result = new bool[PhotonNetwork.Friends.Count];
			int i =0;
			foreach(FriendInfo _friendInfo in PhotonNetwork.Friends)
			{
				_result[i] = _friendInfo.IsOnline;
				i++;
			}

			friendsOnlineStatus.Resize(_result.Length);
			friendsOnlineStatus.boolValues = _result;
		}

		void GetOnlineFriends()
		{
			
			if (onlineFriends.IsNone)
			{
				return;
			}
			
			List<string> _result = new List<string>();
			
			foreach(FriendInfo _friendInfo in PhotonNetwork.Friends)
			{
				if (_friendInfo.IsOnline)
				{
					_result.Add(_friendInfo.Name);
				}
				
			}
			onlineFriends.Resize(_result.Count);
			onlineFriends.Values = _result.ToArray();
		}

		void GetOfflineFriends()
		{
			
			if (offlineFriends.IsNone)
			{
				return;
			}
			
			List<string> _result = new List<string>();
			
			foreach(FriendInfo _friendInfo in PhotonNetwork.Friends)
			{
				if (!_friendInfo.IsOnline)
				{
					_result.Add(_friendInfo.Name);
				}
				
			}
			offlineFriends.Resize(_result.Count);
			offlineFriends.Values = _result.ToArray();
		}
	}
}
