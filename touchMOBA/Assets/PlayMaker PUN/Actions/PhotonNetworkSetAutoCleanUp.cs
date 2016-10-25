// (c) Copyright HutongGames, LLC 2010-2015. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("Photon")]
	[Tooltip("This setting defines per room, if network-instantiated GameObjects (with PhotonView) get cleaned up when the creator of it leaves.This setting is done per room. It can't be changed in the room and it will override the settings of individual clients." +
		"If room.AutoCleanUp is enabled in a room, the PUN clients will destroy a player's GameObjects on leave. " +
		"This includes GameObjects manually instantiated (via RPCs, e.g.)." +
		"When enabled, the server will clean RPCs, instantiated GameObjects and PhotonViews of the leaving player, too. and " +
		"Players who join after someone left, won't get the events of that player anymore." +
		" Under the hood, this setting is stored as a Custom Room Property. " +
		"Enabled by default.")]
	//[HelpUrl("https://hutonggames.fogbugz.com/default.asp?W1108")]
	public class PhotonNetworkSetAutoCleanUpPlayerObjects : FsmStateAction
	{
		[Tooltip("This setting defines per room, if network-instantiated GameObjects (with PhotonView) get cleaned up when the creator of it leaves.")]
		public FsmBool autoCleanUpPlayerObjects;
		
		public override void Reset()
		{
			autoCleanUpPlayerObjects  = null;
		}

		public override void OnEnter()
		{
			PhotonNetwork.autoCleanUpPlayerObjects = autoCleanUpPlayerObjects.Value;
			Finish();
		}

	}
}