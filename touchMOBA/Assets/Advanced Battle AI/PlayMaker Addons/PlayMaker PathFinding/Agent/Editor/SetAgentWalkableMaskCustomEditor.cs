// (c) Copyright HutongGames, LLC 2010-2014. All rights reserved.

using HutongGames.PlayMaker.Actions;
using HutongGames.PlayMakerEditor;

using HutongGames.PlayMaker;
using UnityEditor;
using UnityEngine;
using System;
using System.Collections.Generic;

namespace HutongGames.PlayMakerEditor
{

	[CustomActionEditor(typeof(SetAgentWalkableMask))]
	public class SetAgentWalkableMaskCustomEditor : CustomActionEditor
	{

		private PlayMakerNavMeshMaskField _maskField;

	    public override bool OnGUI()
	    {
		
			SetAgentWalkableMask _target = (SetAgentWalkableMask)target;
			
			bool edited = false;
			
			
			
			EditField("gameObject");

			if (_target.NavMeshlayerMask ==null)
			{
				_target.NavMeshlayerMask =  new FsmInt();
				_target.NavMeshlayerMask.Value = -1;
			}
			
			LayerMask _mask = _target.NavMeshlayerMask.Value;

			if (_maskField==null)
			{
				_maskField = new PlayMakerNavMeshMaskField();
			}
			LayerMask _newMask = _maskField.LayerMaskField("NavMesh layerMask",_mask,true);

			if (_newMask!=_mask)
			{
				edited = true;
				_target.NavMeshlayerMask.Value = _newMask.value;
			}

			return GUI.changed || edited;
	    }

	}
}
