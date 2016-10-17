using UnityEngine;
using System.Collections;

namespace HutongGames.PlayMaker.Actions
{
	public class TakeFlag : FsmStateAction {

		public FsmGameObject Flag;

		public override void OnEnter ()
		{
			Flag.Value.transform.parent= Owner.transform ;
			Finish ();
		}


}

}