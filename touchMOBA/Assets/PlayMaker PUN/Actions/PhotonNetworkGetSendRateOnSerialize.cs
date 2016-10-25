// (c) Copyright HutongGames, LLC 2010-2015. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("Photon")]
	[Tooltip("Get how many times per second OnPhotonSerialize should be called on PhotonViews")]
	public class PhotonNetworkGetSendRatenSerialize : FsmStateAction
	{

		[Tooltip("how many times per second OnPhotonSerialize should be called on PhotonViews")]
		[UIHint(UIHint.Variable)]
		public FsmInt sendRateOnSerialize;

		[Tooltip("Execute every update")]
		public bool everyFrame;

		public override void Reset()
		{
			sendRateOnSerialize = null;
			everyFrame = false;
		}
		
		public override void OnEnter()
		{
			DoGetSendRateOnSerialize();

			if (!everyFrame)
			{
				Finish();
			}
		}
		public override void OnUpdate()
		{
			DoGetSendRateOnSerialize();
		}
		
		void DoGetSendRateOnSerialize()
		{
			if (!sendRateOnSerialize.IsNone)	sendRateOnSerialize.Value = PhotonNetwork.sendRateOnSerialize;
		}
		
	}
}