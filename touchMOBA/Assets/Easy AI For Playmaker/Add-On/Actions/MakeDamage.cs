using UnityEngine;
using System.Collections;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("Agents AI")]
	public class MakeDamage : FsmStateAction {

		public FsmGameObject Target;
		public FsmFloat Damage;


		public override void OnEnter() {
			Target.Value.SendMessage("Damage", Damage.Value, SendMessageOptions.DontRequireReceiver);
			Finish ();
		}

	
}

}
