// (c) Copyright HutongGames, LLC 2010-2016. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("Photon")]
	[Tooltip("Requesting ownership can give you control over a PhotonView, if the ownershipTransfer setting allows that.\n A PhotonView component is required on the gameObject")]
	[HelpUrl("")]
	public class PhotonViewRequestOwnerShip : FsmStateAction
	{
		[RequiredField]
		[CheckForComponent(typeof(PhotonView))]
		[Tooltip("The Game Object with the PhotonView attached.")]
		public FsmOwnerDefault gameObject;

		private PhotonView _networkView;


		public override void Reset()
		{
			gameObject = null;

		}
		
		public override void OnEnter()
		{
			GameObject go = Fsm.GetOwnerDefaultTarget(gameObject);
			if (go == null) 
			{
				return;
			}
			
			_networkView =  go.GetComponent<PhotonView>();

			_networkView.RequestOwnership();

			Finish();
		}
		
	}
}