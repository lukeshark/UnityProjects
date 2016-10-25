// (c) Copyright HutongGames, LLC 2010-2016. All rights reserved.

using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.StateMachine)]
	[Tooltip("Gets info on the last Rpc event. Order of data has to be respected.")]
	public class PhotonViewGetRpcEventData : FsmStateAction
	{

		public FsmVar[] storeData;
		
		public override void Reset()
		{
			storeData = null;
		}
		
		public override void OnEnter()
		{


			int i = 0;
			foreach(FsmVar _var in storeData)
			{
				_var.SetValue(PlayMakerPhotonGameObjectProxy.lastrpc_d_data[i]);

			  i++;
			}


			Finish();
		}
	}
}