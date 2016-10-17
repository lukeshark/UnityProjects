using UnityEngine;
using System.Collections;


namespace HutongGames.PlayMaker.Actions
{
public class IHaveTheFlag : FsmStateAction {

		public FsmEvent IDontHave;

		public override void OnUpdate() {

			if (Owner.transform.FindChild ("Flag") == false) {

				Fsm.Event (IDontHave);
				Finish ();
			}

		}
}

}
