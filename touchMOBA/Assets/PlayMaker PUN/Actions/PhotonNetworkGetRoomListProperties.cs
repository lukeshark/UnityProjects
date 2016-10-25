// (c) Copyright HutongGames, LLC 2010-2016. All rights reserved.

using UnityEngine;
using System.Collections.Generic;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("Photon")]
	[Tooltip("Get lobby rooms properties.")]
	//[HelpUrl("https://hutonggames.fogbugz.com/default.asp?W900")]
	public class PhotonNetworkGetRoomListProperties : FsmStateAction
	{

		[ActionSection("Builtin Properties")]
		[Tooltip("All rooms' name")]
		[UIHint(UIHint.Variable)]
		[ArrayEditor(VariableType.String)]
		public FsmArray names;

		[Tooltip("All rooms' playerCount")]
		[UIHint(UIHint.Variable)]
		[ArrayEditor(VariableType.Int)]
		public FsmArray playerCounts;

		[Tooltip("All rooms' maxPlayers")]
		[UIHint(UIHint.Variable)]
		[ArrayEditor(VariableType.Int)]
		public FsmArray maxPlayers;

		[Tooltip("All rooms' open")]
		[UIHint(UIHint.Variable)]
		[ArrayEditor(VariableType.Bool)]
		public FsmArray opens;


		[Tooltip("Custom Properties you have assigned to rooms.")]
		[CompoundArray("Rooms Custom Properties", "property", "value")]
		public FsmString[] customPropertyKeys;
		[UIHint(UIHint.Variable)]
		public FsmArray[] customPropertiesValues;

		public FsmEvent notInLobbyEvent;

		private RoomInfo[] rooms;

		public override void Reset()
		{

			names = new FsmArray() {UseVariable=true};
			playerCounts = new FsmArray() {UseVariable=true};
			maxPlayers = new FsmArray() {UseVariable=true};
			opens = new FsmArray() {UseVariable=true};

			customPropertyKeys = null;
			customPropertiesValues = null;

			notInLobbyEvent = null;
		}

		
		public override void OnEnter()
		{

			if (!PhotonNetwork.insideLobby)
			{
				Fsm.Event(notInLobbyEvent);

				Finish ();
				return;
			}

			rooms = PhotonNetwork.GetRoomList();

			for(int x=0;x<customPropertiesValues.Length;x++)
			{
				if (! customPropertiesValues[x].IsNone) customPropertiesValues[x].Resize(rooms.Length);
			}



			if (!names.IsNone) names.Resize(rooms.Length);
			if (!playerCounts.IsNone) playerCounts.Resize(rooms.Length);
			if (!maxPlayers.IsNone) playerCounts.Resize(maxPlayers.Length);

			int i=0;
			
			foreach (RoomInfo room in rooms)
			{

				if (!names.IsNone) names.Set(i,room.name);
				if (!playerCounts.IsNone) playerCounts.Set(i,room.playerCount);
				if (!maxPlayers.IsNone) maxPlayers.Set(i,room.maxPlayers);

				// get the custom properties
				int k = 0;
				foreach(FsmString key in customPropertyKeys)
				{
					if (room.customProperties.ContainsKey(key.Value) && ! customPropertiesValues[k].IsNone)
					{
						customPropertiesValues[k].Set(i,room.customProperties[key.Value]);
					}
					k++;
				}


				i++;
			}

			if (!names.IsNone)  names.SaveChanges();
			if (!playerCounts.IsNone)  playerCounts.SaveChanges();
			if (!maxPlayers.IsNone)  maxPlayers.SaveChanges();

			for(int x=0;x<customPropertiesValues.Length;x++)
			{
				if (! customPropertiesValues[x].IsNone) customPropertiesValues[x].SaveChanges();
			}
		}
		
	}
}