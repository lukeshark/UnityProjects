using UnityEngine;
using System.Collections;


namespace HutongGames.PlayMaker.Actions
{
	[Tooltip ("Store the gameobjects in scene with a specific tag in an Array")]
	public class FindGameobjectsByTag : FsmStateAction
	{
		[RequiredField] 
		[UIHint (UIHint.Variable)] 
		[Tooltip ("Store the value in an array.")] 
		public FsmArray StoreValue;
		[RequiredField] 
		[UIHint (UIHint.Tag)]
		public FsmString Tag;

		public override void OnEnter ()
		{

			StoreValue.Values = GameObject.FindGameObjectsWithTag (Tag.Value);
			Finish ();

		}
	}

}