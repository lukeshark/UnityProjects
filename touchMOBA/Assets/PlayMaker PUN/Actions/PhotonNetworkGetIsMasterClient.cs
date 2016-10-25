// (c) Copyright HutongGames, LLC 2010-2015. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("Photon")]
	[Tooltip("Test if the Photon network is the masterClient. Note this can change during the session.")]
	//[HelpUrl("https://hutonggames.fogbugz.com/default.asp?W918")]
	public class PhotonViewGetIsMasterClient : FsmStateAction
	{
		
		[UIHint(UIHint.Variable)]
		[Tooltip("True if the Photon network is currently the masterClient.")]
		public FsmBool isMasterClient;
		
		[Tooltip("Send this event if the Photon network is the masterClient.")]
		public FsmEvent isMasterClientEvent;
		
		[Tooltip("Send this event if the Photon network view is NOT the master client.")]
		public FsmEvent isNotMasterClientEvent;
		
		
		public override void Reset()
		{
		
			isMasterClient = null;
			isMasterClientEvent = null;
			isNotMasterClientEvent = null;
		}

		public override void OnEnter()
		{
			
			checkIsMasterClient();
			
			Finish();
		}
		
		void checkIsMasterClient()
		{
			
			bool _isMasterClient = PhotonNetwork.isMasterClient;
			
			isMasterClient.Value = _isMasterClient;
			
			if (_isMasterClient )
			{
				if (isMasterClientEvent!=null)
				{
					Fsm.Event(isMasterClientEvent);
				}
			}
			else if (isNotMasterClientEvent!=null)
			{
				Fsm.Event(isNotMasterClientEvent);
			}
		}

	}
}