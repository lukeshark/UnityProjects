// (c) Copyright HutongGames, LLC 2010-2015. All rights reserved.

using UnityEngine;
using System.Collections;

using ExitGames.Client.Photon;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("Photon")]
	[Tooltip("Joins any available room but will fail if none is currently available.\n" +
	 	"Optionnally define expected custom properties to match, max Players and matchmkaing mode: http://doc.exitgames.com/photon-cloud/MatchmakingAndLobby/#cat-reference")]
	[HelpUrl("https://hutonggames.fogbugz.com/default.asp?W913")]
	public class PhotonNetworkJoinRandomRoom : FsmStateAction
	{
	
		// I redefine it here, cause the original Photon MatchMakingMode enum crates errors if used as a public var within a Custom Action
		public enum PhotonMatchMakingMode {FillRoom,SerialMatching,RandomMatching}
		
		[Tooltip("Max Player in rooms to filter. Leave to 0 if you don't want to filter by players numbers in rooms\n" +
			"-- FillRoom (Default): Fills up rooms (oldest first) to get players together as fast as possible.Makes most sense with MaxPlayers > 0 and games that can only start with more players.\n" +
			"-- SerialMatching: Distributes players across available rooms sequentially but takes filter into account. Without filter, rooms get players evenly distributed.\n" +
			"-- RandomMatching: Joins a (fully) random room. Expected properties must match but aside from this, any available room might be selected.")]
	 	public PhotonMatchMakingMode matchMakingMode;
		
		[Tooltip("Max Player in rooms to filter. Leave to 0 if you don't want to filter by players numbers in rooms")]
		public FsmInt maxPlayer;
		

		[ActionSection("Expected room properties")]
		
		[Tooltip("room properties to filter rooms before picking a random one.")]
		[CompoundArray("Custom Properties", "property", "value")]
		public FsmString[] customPropertyKeys;
		[Tooltip("Values related to the properties")]
		public FsmVar[] customPropertiesValues;
		
		
		[ActionSection("Lobby properties")]
		
		[Tooltip("LobbyFilters. Leave to none to ingore it")]
		[UIHint(UIHint.Variable)]
		public FsmString sqlLobbyFilter;
		
		
		public override void Reset()
		{
			matchMakingMode = PhotonMatchMakingMode.RandomMatching;
			
			maxPlayer = new FsmInt() {UseVariable=true};
			
			//typedLobby = TypedLobby.Default;
				
			sqlLobbyFilter = new FsmString() {UseVariable=true};
			
			customPropertyKeys = new FsmString[0];
			customPropertiesValues = new FsmVar[0];
		}
		

		public override void OnEnter()
		{
			bool withExpections = false;
			int _maxPlayer = 0;
			
			
			ExitGames.Client.Photon.Hashtable _prop = new ExitGames.Client.Photon.Hashtable();
			
			if ( (! maxPlayer.IsNone) || maxPlayer.Value>0)
			{
				_maxPlayer = maxPlayer.Value;
				withExpections = true;
			}
			
			if (customPropertyKeys.Length>0)
			{
				withExpections =  true;
			}
			
			if (matchMakingMode != PhotonMatchMakingMode.FillRoom )
			{
				withExpections =  true;
			}
			
			if (withExpections)
			{

				MatchmakingMode _mode = MatchmakingMode.FillRoom;
				if (matchMakingMode == PhotonMatchMakingMode.RandomMatching)
				{
					_mode = MatchmakingMode.RandomMatching;
				}else if (matchMakingMode == PhotonMatchMakingMode.SerialMatching)
				{
					_mode = MatchmakingMode.SerialMatching;
				}
				
				PhotonNetwork.JoinRandomRoom(_prop,(byte)_maxPlayer,_mode,TypedLobby.Default,sqlLobbyFilter.Value);
			}else{
					PhotonNetwork.JoinRandomRoom();
			}
		
			
			Finish();
		}
	}
}