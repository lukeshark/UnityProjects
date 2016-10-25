// (c) Copyright HutongGames, LLC 2010-2016. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("Photon")]
	[Tooltip("Remove all buffered RPCs from server that were sent via targetPhotonView. The Master Client and the owner of the targetPhotonView may call this.")]
	//[HelpUrl("https://hutonggames.fogbugz.com/default.asp?W918")]
	public class PhotonViewRemoveRpc : FsmStateAction
	{
		[RequiredField]
		[CheckForComponent(typeof(PhotonView))]
		[Tooltip("The Game Object with the PhotonView attached.")]
		public FsmOwnerDefault gameObject;
		
		[Tooltip("Send this event if the action was executed")]
		public FsmEvent successEvent;
		
		[Tooltip("Send this event if the action was not executed, likely because we are not on the master and we do not own this photonView")]
		public FsmEvent failureEvent;


		private PhotonView _networkView;
		
		private void _getNetworkView()
		{
			GameObject go = Fsm.GetOwnerDefaultTarget(gameObject);
			if (go == null) 
			{
				return;
			}
			
			_networkView =  go.GetComponent<PhotonView>();
		}
		
		public override void Reset()
		{
			gameObject = null;
			failureEvent = null;
			successEvent = null;
		}

		public override void OnEnter()
		{
			_getNetworkView();
			
			doRemoveRpc();

			Fsm.Event(successEvent);

			Finish();
		}
		
		void doRemoveRpc()
		{
			if (_networkView ==null)
			{
				Fsm.Event(failureEvent);
				return;	
			}

		
			if ( _networkView.isMine)
			{
				PhotonNetwork.RemoveRPCs(_networkView);
			}else{
				if (!PhotonNetwork.isMasterClient)
				{
					Fsm.Event(failureEvent);
					return;
				}
				PhotonNetwork.RemoveRPCs(_networkView);
			}
		}
	}
}