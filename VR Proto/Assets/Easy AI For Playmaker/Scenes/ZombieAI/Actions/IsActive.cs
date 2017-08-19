using UnityEngine;
using System.Collections;

namespace HutongGames.PlayMaker.Actions
{
	public class IsActive : FsmStateAction
	{
		public FsmGameObject Gameobject;

		public FsmEvent True;
		public FsmEvent False;

		public override void OnEnter ()
		{

			if (Gameobject.Value.activeInHierarchy)
				Fsm.Event (True);
			else
				Fsm.Event (False);


		}

	}

}