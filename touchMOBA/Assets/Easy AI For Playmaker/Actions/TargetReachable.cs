using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	
	[ActionCategory("Agents AI")]
	public class TargetReachableGua : FsmStateAction
	{
		[Tooltip("GameObject From")]
		public FsmOwnerDefault gameObject;
		[Tooltip("Target Reachable")]
		
		public FsmGameObject target;
		[Tooltip("Event to send if is Reachable.")]
		public FsmEvent isReachableEvent;
		
		[Tooltip("Event to send if is NOT Reachable.")]
		public FsmEvent isNotReachableEvent;
		
		private GameObject go;
		private NavMeshPath _path;
		private float elapsed = 0.0f;
		
		[UIHint(UIHint.Variable)]
		[Tooltip("is it reachable.")]
		public FsmBool isReachable;
		
		
        //bool previousValue;
		
		public override void Reset()
		{
			isReachable = null;
			isReachableEvent = null;
			isNotReachableEvent = null;
			//everyFrame = false;
		}
        // Code that runs on entering the state.
		public override void OnEnter()
		{
			_path = new NavMeshPath();
			elapsed = 0.0f;
			
			if (target != null)
				go = Fsm.GetOwnerDefaultTarget(gameObject);
			
 
		}
		public override void OnUpdate()
		{
			elapsed += Time.deltaTime;
			if (elapsed > 0.5f)
			{
				elapsed = 0.0f;
				isReachable.Value = NavMesh.CalculatePath(go.transform.position, target.Value.transform.position, NavMesh.AllAreas, _path);
				Fsm.Event(isReachable.Value ? isReachableEvent : isNotReachableEvent);
				
			}
			
			for (int i = 0; i < _path.corners.Length - 1; i++)
				Debug.DrawLine(_path.corners[i], _path.corners[i + 1], Color.red);
			

			
		}
	}
	
}