using UnityEngine;
using UnityEngine.UI;

namespace HutongGames.PlayMaker.Actions
{

[ActionCategory("Agents AI")]
public class ShowText : FsmStateAction
{
	public Text _txt;
	
	public FsmGameObject texto;
	// Code that runs on entering the state.
	public override void OnEnter()
	{
		if (texto.Value != null) 
		{
			_txt.text =  texto.Value.name;
		}
		else
		{

				_txt.text =  "Nothing...";
		}
		Finish();
	}


}

}
