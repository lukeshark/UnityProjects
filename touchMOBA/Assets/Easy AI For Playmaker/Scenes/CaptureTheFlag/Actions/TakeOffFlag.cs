using UnityEngine;
using System.Collections;

namespace HutongGames.PlayMaker.Actions
{
	public class TakeOffFlag : FsmStateAction {

		public FsmGameObject Flag;

		public override void OnEnter ()
		{
			Flag.Value.transform.parent = null ;
			Finish ();
		}


	}

}