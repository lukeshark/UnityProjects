// (c) Copyright HutongGames, LLC 2010-2015. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("Photon")]
	[Tooltip("Get how many times per second PhotonNetwork should send a package")]
	public class PhotonNetworkGetSendRate : FsmStateAction
	{

		[Tooltip("how many times per second PhotonNetwork should send a package")]
		[UIHint(UIHint.Variable)]
		public FsmInt sendRate;

		[Tooltip("Execute every update")]
		public bool everyFrame;

		public override void Reset()
		{
			sendRate = null;
			everyFrame = false;
		}
		
		public override void OnEnter()
		{
			DoGetSendRate();

			if (!everyFrame)
			{
				Finish();
			}
		}
		public override void OnUpdate()
		{
			DoGetSendRate();
		}
		
		void DoGetSendRate()
		{
			if (!sendRate.IsNone)	sendRate.Value = PhotonNetwork.sendRate;
		}
		
	}
}