// (c) Copyright HutongGames, LLC 2010-2015. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("Photon")]
	[Tooltip("Defines how many times per second OnPhotonSerialize should be called on PhotonViews. Default is 10.\n" +
		"Choose this value in relation to PhotonNetwork.sendRate. OnPhotonSerialize will create updates and messages to be sent.\n" +
		"A lower rate takes up less performance but will cause more lag.")]
	public class PhotonNetworkSetSendRateOnSerialize : FsmStateAction
	{
		[RequiredField]
		[Tooltip("Defines how many times per second OnPhotonSerialize should be called on PhotonViews, default is 10")]
		public FsmInt sendRateOnSerialize;
		
		public override void Reset()
		{
			sendRateOnSerialize = 10;
		}
		
		public override void OnEnter()
		{
			DoSetSendRateOnSerialize();
			
			Finish();
		}
		
		void DoSetSendRateOnSerialize()
		{
			PhotonNetwork.sendRateOnSerialize = sendRateOnSerialize.Value;
		}
	}
}
