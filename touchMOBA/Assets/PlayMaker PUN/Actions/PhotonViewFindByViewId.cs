// (c) Copyright HutongGames, LLC 2010-2015. All rights reserved.

using UnityEngine;
using ExitGames.Client;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("Photon")]
	[Tooltip("Find the GameObject with a photon NetworkView with a given NetworkView ID.")]
	//[HelpUrl("https://hutonggames.fogbugz.com/default.asp?W917")]
	public class PhotonViewFindByViewID : FsmStateAction
	{

		[Tooltip("The PhotonView ID as int to find")]
		public FsmInt ID;

		[Tooltip("The PhotonView ID as string to find. Leave to false for no effect")]
		public FsmString IDAsString;

		[ActionSection("result")]
		[UIHint(UIHint.Variable)]
		[Tooltip("The Game Object with the PhotonView of the given ID.")]
		public FsmGameObject gameObject;

		[Tooltip("Set to true if a GameObject is found with that photonView ID")]
		[UIHint(UIHint.Variable)]
		public FsmBool GameObjectFound;

		[Tooltip("Event sent if a GameObject is found with that photonView ID")]
		public FsmEvent GameObjectFoundEvent;

		[Tooltip("Event sent if bo GameObject is found with that photonView ID")]
		public FsmEvent GameObjectNotFoundEvent;

		public override void Reset()
		{
			gameObject = null;
			ID = null;
			IDAsString = new FsmString(){UseVariable=true};
			GameObjectFoundEvent = null;
			GameObjectNotFoundEvent = null;
		}
		
		public override void OnEnter()
		{
			int _id = -1;

			bool ok = false;

			if (!IDAsString.IsNone)
			{
				ok = int.TryParse(IDAsString.Value,out _id);
			}

			if (!ok)
			{
				_id = ID.Value;
			}

			PhotonView _pv = PhotonNetwork.networkingPeer.GetPhotonView(_id);

			bool _found = _pv!=null;
			GameObjectFound.Value = _found;

			if (!_found)
			{
				gameObject.Value = null;
				Fsm.Event(GameObjectNotFoundEvent);
			}else{
				gameObject.Value = _pv.gameObject;
				Fsm.Event(GameObjectFoundEvent);
			}

			Finish();
		}
		
	}
}