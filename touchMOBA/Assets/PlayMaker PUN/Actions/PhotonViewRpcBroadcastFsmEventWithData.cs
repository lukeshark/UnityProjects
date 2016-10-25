// (c) Copyright HutongGames, LLC 2010-2015. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("Photon")]
	[Tooltip("Remote Event Calls (using Photon RPC under the hood) let you broadcast a Fsm Event to photon targets ( all players, other players, master).also let you send one or more Fsm Variables")]
	[HelpUrl("")]
	public class PhotonViewRpcBroadcastFsmEventWithData : FsmStateAction
	{
		public PhotonTargets photonTargets;
		
		[Tooltip("Override photonTargets above if set. Available enums: All,AllBuffered,MasterClient,Others,OthersBuffered,AllViaServer,AllviaServerBuffered")]
		[UIHint(UIHint.Variable)]
		public FsmString photonTargetsFromString;
		
		// ONLY ACCEPTS BROADCAST OR SELF
		public FsmEventTarget eventTarget;
		
		[RequiredField]
		[Tooltip("The event you want to send.")]
		[UIHint(UIHint.FsmEvent)]
		public FsmEvent remoteEvent;
		
		[Tooltip("Optionnal array of variable data ( will be injected in the Event data. Use 'Get Event data' action to retrieve it)")]
		public FsmVar[] data;
		
	
		public override void Reset()
		{
			// JFF: how can I set this silently without a public variable? if I set it to private, it doesn't work anymore. maybe I forgot a setting?
			eventTarget = new FsmEventTarget();
			eventTarget.target = FsmEventTarget.EventTarget.BroadcastAll;
			
			remoteEvent = null;
			photonTargets = PhotonTargets.All;

			photonTargetsFromString = new FsmString() {UseVariable=true};
			data = null;
		}

		public override void OnEnter()
		{
			DoRPC();
			
			Finish();
		}

		void DoRPC()
		{
			
			// get the photon proxy for Photon RPC access
			GameObject go = GameObject.Find("PlayMaker Photon Proxy");
			
			if (go == null )
			{
				return;
			}
			
			
			if (remoteEvent != null && remoteEvent.IsGlobal == false)
			{ 
				return;
			}
			
			PhotonTargets _photonTargets = getPhotonTargets();
		
			// get the proxy component
			PlayMakerPhotonProxy _proxy = go.GetComponent<PlayMakerPhotonProxy>();
			if (_proxy==null)
			{
				Debug.LogWarning("PlayMakerPhotonProxy is missing");
				return;
			}
			
			if (eventTarget.target == FsmEventTarget.EventTarget.BroadcastAll)
			{
				
				if (data.Length>0 ){
					//_proxy.PhotonRpcFsmBroadcastEventWithData(_photonTargets,remoteEvent.Name,data);
				}else{
					_proxy.PhotonRpcBroacastFsmEvent(photonTargets,remoteEvent.Name);
				}
			}else{
				
				PlayMakerPhotonGameObjectProxy _goProxy = Owner.GetComponent<PlayMakerPhotonGameObjectProxy>();
				if (_proxy==null)
				{
						Debug.LogWarning("PlayMakerPhotonProxy is missing");
					return;
				}
				
				if (data.Length>0 ){
					_goProxy.PhotonRpcSendFsmEventWithData(_photonTargets,remoteEvent.Name,data);
				}else{
					_goProxy.PhotonRpcSendFsmEvent(photonTargets,remoteEvent.Name);
				}
			}
			
		}
		
		PhotonTargets getPhotonTargets()
		{
			if ( photonTargetsFromString.IsNone || photonTargetsFromString.Value == "")
			{
				return photonTargets;
			} 
			string _target = photonTargetsFromString.Value.ToLower();
			
			switch (_target)
			{
				case "all":
					return PhotonTargets.All;
				
				
				case "allbuffered":
					return PhotonTargets.AllBuffered;
			
				
				case "masterclient":
					return PhotonTargets.MasterClient;
				
				
				case "others":
					return PhotonTargets.Others;
				
				
				case "othersbuffered":
					return PhotonTargets.OthersBuffered;
				
				
			}
			
			return photonTargets;
		}
		
		public override string ErrorCheck()
		{
			
			if (remoteEvent == null)
			{
				return "Remote Event not set";
			}
			
			if (remoteEvent != null && !remoteEvent.IsGlobal)
			{
				return "Remote Event must be a global event";
			}
			
			if ( photonTargetsFromString.Value == "")
			{
				return "";
				//return "Photon target string must be set if selected.\n Available enums: All,AllBuffered,MasterClient,Others,OthersBuffered";
			} 
			
			string _target = photonTargetsFromString.Value.ToLower();
			
			switch (_target)
			{
				case "all":
					return "";
				
				
				case "allbuffered":
					return "";
				
				
				case "masterclient":
					return "";
				
				
				case "others":
					return "";
			
				
				case "othersbuffered":
					return "";
				
				
			}
			return "Photon target string must be set if selected.\n Available enums: All,AllBuffered,MasterClient,Others,OthersBuffered";
		}
		
		
		
	}
}