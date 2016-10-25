// (c) Copyright HutongGames, LLC 2010-2015. All rights reserved.
// http://hutonggames.com/playmakerforum/index.php?topic=11297.msg54177#msg54177

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("Photon")]
	[Tooltip("Receives Network Ping and Stores in a String or an Int")]
	public class PhotonGetPing : FsmStateAction
	{

		[Tooltip("The current roundtrip time to the photon server")]
		[UIHint(UIHint.Variable)]
		public FsmInt ping;

		[Tooltip("The current roundtrip time to the photon server as string")]
		[UIHint(UIHint.Variable)]
		public FsmString pingString;

		[Tooltip("Execute every update")]
		public bool everyFrame;

		public override void Reset()
		{
			ping = null;
			pingString = null;
			everyFrame = false;
		}
		
		public override void OnEnter()
		{
			DoGetPing();

			if (!everyFrame)
			{
				Finish();
			}
		}
		public override void OnUpdate()
		{
			DoGetPing();
		}
		
		void DoGetPing()
		{
			if (!ping.IsNone)	ping.Value = PhotonNetwork.GetPing ();
			if (!pingString.IsNone) pingString.Value = ping.Value.ToString();
		}
		
	}
}