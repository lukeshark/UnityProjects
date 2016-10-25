// (c) Copyright HutongGames, LLC 2010-2016. All rights reserved.

using UnityEngine;
using System.Collections.Generic;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("Photon")]
	[Tooltip("Get Photon players properties.")]
	//[HelpUrl("https://hutonggames.fogbugz.com/default.asp?W900")]
	public class PhotonNetworkGetPlayersProperties : FsmStateAction
	{

		[Tooltip("If true, list only other players.")]
		public FsmBool otherPLayerOnly;

		[ActionSection("Builtin Properties")]
		[Tooltip("All players' name")]
		[UIHint(UIHint.Variable)]
		[ArrayEditor(VariableType.String)]
		public FsmArray names;

		[Tooltip("All players' ID")]
		[UIHint(UIHint.Variable)]
		[ArrayEditor(VariableType.Int)]
		public FsmArray ids;

		[Tooltip("All players' userId")]
		[UIHint(UIHint.Variable)]
		[ArrayEditor(VariableType.String)]
		public FsmArray userIds;

		[Tooltip("All players' inactivestate")]
		[UIHint(UIHint.Variable)]
		[ArrayEditor(VariableType.Bool)]
		public FsmArray isInactives;

		[Tooltip("All players' isLocals")]
		[UIHint(UIHint.Variable)]
		[ArrayEditor(VariableType.Bool)]
		public FsmArray isLocals;

		[Tooltip("All players' isMasterClient")]
		[UIHint(UIHint.Variable)]
		[ArrayEditor(VariableType.Bool)]
		public FsmArray isMasterClients;


		[Tooltip("Custom Properties you have assigned to players.")]
		[CompoundArray("Players Custom Properties", "property", "value")]
		public FsmString[] customPropertyKeys;
		[UIHint(UIHint.Variable)]
		public FsmArray[] customPropertiesValues;

		private PhotonPlayer[] players;

		public override void Reset()
		{
			otherPLayerOnly = true;

			names = new FsmArray() {UseVariable=true};
			ids = new FsmArray() {UseVariable=true};
			userIds = new FsmArray() {UseVariable=true};
			isInactives = new FsmArray() {UseVariable=true};
			isLocals = new FsmArray() {UseVariable=true};
			isMasterClients = new FsmArray() {UseVariable=true};

			customPropertyKeys = null;
			customPropertiesValues = null;

		}
		
		public override void OnEnter()
		{

			if (otherPLayerOnly.Value){
				players = PhotonNetwork.otherPlayers;
			}else{
				players = PhotonNetwork.playerList;
			}

			if (!names.IsNone) names.Resize(players.Length);
			if (!userIds.IsNone) userIds.Resize(players.Length);
			if (!ids.IsNone) ids.Resize(players.Length);
			if (!isInactives.IsNone) isInactives.Resize(players.Length);
			if (!isLocals.IsNone) isLocals.Resize(players.Length);
			if (!isMasterClients.IsNone) isMasterClients.Resize(players.Length);


			for(int x=0;x<customPropertiesValues.Length;x++)
			{
				if (! customPropertiesValues[x].IsNone) customPropertiesValues[x].Resize(players.Length);
			}


			int i=0;
			
			foreach (PhotonPlayer player in players)
			{

				if (!names.IsNone) names.Set(i,player.name);
				if (!userIds.IsNone) userIds.Set(i,player.userId);
				if (!ids.IsNone) ids.Set(i,player.ID);
				if (!isInactives.IsNone) isInactives.Set(i,player.isInactive);
				if (!isLocals.IsNone) isLocals.Set(i,player.isLocal);
				if (!isMasterClients.IsNone) isMasterClients.Set(i,player.isMasterClient);


				// get the custom properties
				int k = 0;
				foreach(FsmString key in customPropertyKeys)
				{
					if (player.customProperties.ContainsKey(key.Value) && ! customPropertiesValues[k].IsNone)
					{
						customPropertiesValues[k].Set(i,player.customProperties[key.Value]);
					}
					k++;
				}


				i++;
			}

			if (!names.IsNone)  names.SaveChanges();
			if (!userIds.IsNone)  userIds.SaveChanges();
			if (!ids.IsNone)  ids.SaveChanges();
			if (!isInactives.IsNone)  isInactives.SaveChanges();
			if (!isLocals.IsNone)  isLocals.SaveChanges();
			if (!isMasterClients.IsNone)  isMasterClients.SaveChanges();

			for(int x=0;x<customPropertiesValues.Length;x++)
			{
				if (! customPropertiesValues[x].IsNone) customPropertiesValues[x].SaveChanges();
			}

		}
		
	}
}