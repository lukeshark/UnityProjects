// (c) Copyright HutongGames, LLC 2010-2012. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Vector3)]
	[Tooltip("Linearly interpolate between 2 vectors. it also lets you lerp against deltatime")]
	public class Vector3Lerp2 : FsmStateAction
	{
		[RequiredField]
		[Tooltip("First Vector.")]
		public FsmVector3 fromVector;
		
		[RequiredField]
		[Tooltip("Second Vector.")]
		public FsmVector3 toVector;
		
		[RequiredField]
		[Tooltip("Interpolate between From Vector and ToVector by this amount. Value is clamped to 0-1 range. 0 = From Vector; 1 = To Vector; 0.5 = half way between.")]
		public FsmFloat amount;
		
		[Tooltip("amount is multiplied by the deltatime")]
		public bool lerpAgainstDeltaTime;
		
		
		[RequiredField]
		[UIHint(UIHint.Variable)]
		[Tooltip("Store the result in this vector variable.")]
		public FsmVector3 storeResult;

		[Tooltip("Repeat every frame. Useful if any of the values are changing.")]
		public bool everyFrame;

		public override void Reset()
		{
			fromVector = new FsmVector3 { UseVariable = true };
			toVector = new FsmVector3 { UseVariable = true };
			storeResult = null;
			lerpAgainstDeltaTime = false;
			everyFrame = true;
		}

		public override void OnEnter()
		{
			DoVector3Lerp();

			if (!everyFrame)
			{
				Finish();
			}
		}

		public override void OnUpdate()
		{
			DoVector3Lerp();
		}

		void DoVector3Lerp()
		{
			if (lerpAgainstDeltaTime)
			{
				storeResult.Value = Vector3.Lerp(fromVector.Value, toVector.Value, Time.deltaTime*amount.Value);
			}else{
				storeResult.Value = Vector3.Lerp(fromVector.Value, toVector.Value, amount.Value);
			}
		}
	}
}

