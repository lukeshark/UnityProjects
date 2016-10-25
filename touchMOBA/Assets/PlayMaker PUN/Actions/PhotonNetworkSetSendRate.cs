// (c) Copyright HutongGames, LLC 2010-2015. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("Photon")]
	[Tooltip("Defines how many times per second PhotonNetwork should send a package. Default is 20.\n" +
		"If you change this, do not forget to also change 'sendRateOnSerialize'.. \n" +
	         "Less packages are less overhead but more delay.Setting the sendRate to 50 will create up to 50 packages per second (which is a lot!).\n" +
	         "Keep your target platform in mind: mobile networks are slower and less reliable.")]
	public class PhotonNetworkSetSendRate : FsmStateAction
	{
		[RequiredField]
		[Tooltip("Defines how many times per second PhotonNetwork should send a package, default is 20")]
		public FsmInt sendRate;

		public override void Reset()
		{
			sendRate = 20;
		}
		
		public override void OnEnter()
		{
			DoSetSendRate();
			
			Finish();
		}
		
		void DoSetSendRate()
		{
			PhotonNetwork.sendRate = sendRate.Value;
		}
	}
}
