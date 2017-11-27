using UnityEngine;
using System.Collections;
using HutongGames.PlayMaker;

public class PlayMakerUltimateFPSCameraActivityWatcher : MonoBehaviour {

	public vp_FPPlayerEventHandler m_Player;

	/// <summary>
	/// registers this component with the event handler (if any)
	/// </summary>
	protected virtual void OnEnable()
	{

		if (m_Player != null)
			m_Player.Register(this);

	}


	/// <summary>
	/// unregisters this component from the event handler (if any)
	/// </summary>
	protected virtual void OnDisable()
	{
		if (m_Player != null)
			m_Player.Unregister(this);
	}
	
	/*
	#region Activity attempts
	
	private string getActivityFromEventName(string eventName)
	{
		string[] splits = eventName.Split("/");
		
		if (splits.Length!=4)
		{
			return "";
		}
		
		return splits[2].Trim();
	}
	
	public void ProcessActivityEvent(string eventName)
	{
		
		// typicaly signature: UFPSC / ACTIVITY / RELOAD / TRY START
		if ( eventName.EndsWith("TRY START") )
		{
			// get the activity
			switch ( getActivityFromEventName(eventName) )
			{
				case "RELOAD":
				m_Player.Reload.TryStart();
				break;
			}
			
			return;
		}
		
		
	}
	
	#endregion
	*/
	
	#region Activity callbacks
	
	/// <summary>
	/// this callback is triggered right after the activity in
	/// question has been approved for activation. 
	///  It Broadcasts a global event related to that activity
	/// </summary>
	protected virtual void OnStart_Dead()
	{
		PlayMakerFSM.BroadcastEvent(GetActivityStartEvent("dead"));
	}
	
	/// <summary>
	/// this callback is triggered when the activity in question
	/// deactivates
	/// </summary>
	protected virtual void OnStop_Dead()
	{
		PlayMakerFSM.BroadcastEvent(GetActivityStopEvent("dead"));
	}
	
	/// <summary>
	/// this callback is triggered right after the activity in
	/// question has been approved for activation. 
	/// It Broadcasts a global event related to that activity
	/// </summary>
	protected virtual void OnStart_Run()
	{
		PlayMakerFSM.BroadcastEvent(GetActivityStartEvent("Run"));
	}
	
	/// <summary>
	/// this callback is triggered when the activity in question
	/// deactivates
	/// </summary>
	protected virtual void OnStop_Run()
	{
		PlayMakerFSM.BroadcastEvent(GetActivityStopEvent("Run"));
	}

	/// <summary>
	/// this callback is triggered right after the activity in
	/// question has been approved for activation. 
	/// It Broadcasts a global event related to that activity
	/// </summary>
	protected virtual void OnStart_Jump()
	{
		PlayMakerFSM.BroadcastEvent(GetActivityStartEvent("Jump"));
	}
	
	/// <summary>
	/// this callback is triggered when the activity in question
	/// deactivates
	/// </summary>
	protected virtual void OnStop_Jump()
	{
		PlayMakerFSM.BroadcastEvent(GetActivityStopEvent("Jump"));
	}

	/// <summary>
	/// this callback is triggered right after the activity in
	/// question has been approved for activation. 
	/// It Broadcasts a global event related to that activity
	/// </summary>
	protected virtual void OnStart_Crouch()
	{
		PlayMakerFSM.BroadcastEvent(GetActivityStartEvent("Crouch"));
	}
	
	/// <summary>
	/// this callback is triggered when the activity in question
	/// deactivates
	/// </summary>
	protected virtual void OnStop_Crouch()
	{
		PlayMakerFSM.BroadcastEvent(GetActivityStopEvent("Jump"));
	}	


	/// <summary>
	/// this callback is triggered right after the activity in
	/// question has been approved for activation. 
	/// It Broadcasts a global event related to that activity
	/// </summary>
	protected virtual void OnStart_Zoom()
	{
		PlayMakerFSM.BroadcastEvent(GetActivityStartEvent("Zoom"));
	}
	
	/// <summary>
	/// this callback is triggered when the activity in question
	/// deactivates
	/// </summary>
	protected virtual void OnStop_Zoom()
	{
		PlayMakerFSM.BroadcastEvent(GetActivityStopEvent("Zoom"));
	}	

	/// <summary>
	/// this callback is triggered right after the activity in
	/// question has been approved for activation. 
	/// It Broadcasts a global event related to that activity
	/// </summary>
	protected virtual void OnStart_Attack()
	{
		PlayMakerFSM.BroadcastEvent(GetActivityStartEvent("Attack"));
	}
	
	/// <summary>
	/// this callback is triggered when the activity in question
	/// deactivates
	/// </summary>
	protected virtual void OnStop_Attack()
	{
		PlayMakerFSM.BroadcastEvent(GetActivityStopEvent("Attack"));
	}	

	/// <summary>
	/// this callback is triggered right after the activity in
	/// question has been approved for activation. 
	/// It Broadcasts a global event related to that activity
	/// </summary>
	protected virtual void OnStart_Reload()
	{
		PlayMakerFSM.BroadcastEvent(GetActivityStartEvent("Reload"));
	}
	
	/// <summary>
	/// this callback is triggered when the activity in question
	/// deactivates
	/// </summary>
	protected virtual void OnStop_Reload()
	{
		PlayMakerFSM.BroadcastEvent(GetActivityStopEvent("Reload"));
	}	

	/// <summary>
	/// this callback is triggered right after the activity in
	/// question has been approved for activation. 
	/// It Broadcasts a global event related to that activity
	/// </summary>
	protected virtual void OnStart_SetWeapon()
	{

		FsmEventData eventData = new FsmEventData();
		eventData.IntData = (int)m_Player.SetWeapon.Argument;
		HutongGames.PlayMaker.Fsm.EventData = eventData;
		
		PlayMakerFSM.BroadcastEvent(GetActivityStartEvent("Set Weapon"));
	}
	
	/// <summary>
	/// this callback is triggered when the activity in question
	/// deactivates
	/// </summary>
	protected virtual void OnStop_SetWeapon()
	{
		PlayMakerFSM.BroadcastEvent(GetActivityStopEvent("Set Weapon"));
	}	

	/// <summary>
	/// this callback is triggered right after the activity in
	/// question has been approved for activation. 
	/// It Broadcasts a global event related to that activity
	/// </summary>
	protected virtual void OnStart_Earthquake()
	{
		FsmEventData eventData = new FsmEventData();
		eventData.Vector3Data = (Vector3)m_Player.SetWeapon.Argument;
		HutongGames.PlayMaker.Fsm.EventData = eventData;
		PlayMakerFSM.BroadcastEvent(GetActivityStartEvent("Earthquake"));
	}
	
	/// <summary>
	/// this callback is triggered when the activity in question
	/// deactivates
	/// </summary>
	protected virtual void OnStop_Earthquake()
	{
	 	PlayMakerFSM.BroadcastEvent(GetActivityStopEvent("Earthquake"));
	}	

	#endregion
	
	
	
	#region Private tools
	private string GetActivityStartEvent(string activity)
	{
		return "UFPSC / ACTIVITY / "+activity.ToUpper()+" / ON START";
	}
	
	private string GetActivityStopEvent(string activity)
	{
		return "UFPSC / ACTIVITY / "+activity.ToUpper()+" / ON STOP";
	}
	
	#endregion
	
	
}
