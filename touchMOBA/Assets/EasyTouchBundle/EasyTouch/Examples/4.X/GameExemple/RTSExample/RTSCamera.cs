using System;
//using DG.Tweening;
//using Prime31;
using UnityEngine;
using System.Collections;
using HedgehogTeam.EasyTouch;

public class RTSCamera : MonoBehaviour 
{
	private Vector3 _curPosition;
	private Vector3 _velocity;
	private bool _underInertia;
	private float _time = 0.0f;
	public float SmoothTime;
	
	public float PinchSpeedY = 1f;
	public float PinchSpeedZ = 1f;
	public float DragSpeed = 90f;
	public float ScrollWheelSpeed = 1000f;
	public Vector3 MaxPosition = new Vector3(90, 10, 110);
	public Vector3 MinPosition = new Vector3(-80, -20, 0);
	
	private CameraState _cameraState;
	
	private bool _disabled;
	
	private Vector3 delta;
	
	
	private Vector3 _returnPosition;
	private Quaternion _returnRotation;
	
	void OnEnable(){
		EasyTouch.On_Swipe += On_Swipe;
		EasyTouch.On_Drag += On_Drag;
		EasyTouch.On_Twist += On_Twist;
		EasyTouch.On_Pinch += On_Pinch;
		EasyTouch.On_DragStart += On_DragStart;
		EasyTouch.On_DragEnd += On_DragEnd;
		EasyTouch.On_SwipeStart += On_SwipeStart;
		EasyTouch.On_SwipeEnd += On_SwipeEnd;
		
		
		transform.localPosition = new Vector3(-7.580322f, 0f, 52.01447f);
		transform.localEulerAngles = new Vector3(0f, 0f, 0f);
	}
	
	void OnDestroy(){
		EasyTouch.On_Swipe -= On_Swipe;
		EasyTouch.On_Drag -= On_Drag;
		EasyTouch.On_Twist -= On_Twist;
		EasyTouch.On_Pinch -= On_Pinch;
		EasyTouch.On_DragStart -= On_DragStart;
		EasyTouch.On_DragEnd -= On_DragEnd;
		EasyTouch.On_SwipeStart -= On_SwipeStart;
		EasyTouch.On_SwipeEnd -= On_SwipeEnd;
		
		
	}
	
	private void On_SwipeEnd(Gesture gesture)
	{
		On_DragEnd(gesture);
	}
	
	private void On_SwipeStart(Gesture gesture)
	{
		On_DragStart(gesture);
	}
	
	void On_Twist(Gesture gesture)
	{
        //transform.Rotate( Vector3.up * gesture.twistAngle);
	}
	
	private void On_DragStart(Gesture gesture)
	{
		switch (_cameraState)
		{
		case CameraState.Moveable:
			_underInertia = false;
			break;
		case CameraState.Locked:
			_underInertia = false;
			break;
		case CameraState.Animating:
			_underInertia = false;
			break;
		case CameraState.FinshedAnimating:
			_underInertia = false;
			break;
		}
	}
	
	private void On_DragEnd(Gesture gesture)
	{
		switch (_cameraState)
		{
		case CameraState.Moveable:
			_underInertia = true;
			break;
		case CameraState.Locked:
			_underInertia = true;
			break;
		case CameraState.Animating:
			_underInertia = true;
			break;
		case CameraState.FinshedAnimating:
			_underInertia = true;
			break;
		}
	}
	
	
	void On_Drag (Gesture gesture){
		On_Swipe( gesture);
	}
	
	void On_Swipe (Gesture gesture)
	{
		switch (_cameraState)
		{
		case CameraState.Moveable:
			MoveTouch(gesture);
			break;
		case CameraState.Locked:
			break;
		case CameraState.Animating:
			break;
		case CameraState.FinshedAnimating:
			break;
		}
	}
	
	
	
	void On_Pinch (Gesture gesture)
	{
		switch (_cameraState)
		{
		case CameraState.Moveable:
			ZoomInTouch(gesture);
			break;
		case CameraState.Locked:
			break;
		case CameraState.Animating:
			break;
		case CameraState.FinshedAnimating:
			break;
		}
	}
	
	void Update()
	{
		var d = Input.GetAxis("Mouse ScrollWheel");
		if (d > 0f || d < 0f)
		{
			switch (_cameraState)
			{
			case CameraState.Moveable:
				ZoomMouseWheel(d);
				break;
			case CameraState.Locked:
				break;
			case CameraState.Animating:
				break;
			case CameraState.FinshedAnimating:
				break;
			}
		}
		
		switch (_cameraState)
		{
		case CameraState.Moveable:
			SmoothMotion();
			break;
		case CameraState.Locked:
			break;
		case CameraState.Animating:
			break;
		case CameraState.FinshedAnimating:
			break;
		}
		
		
	}
	
	private void MoveTouch(Gesture gesture)
	{
		var prevPosition = _curPosition;
		transform.Translate(Vector3.left * gesture.deltaPosition.x / Screen.width * DragSpeed);
		transform.Translate(Vector3.back * gesture.deltaPosition.y / Screen.height * DragSpeed);
		CheckBounds();
		_curPosition = transform.position;
		_velocity = _curPosition - prevPosition;
	}
	
	private void SmoothMotion()
	{
		if (_underInertia && _time <= SmoothTime)
		{
			transform.position += _velocity;
			_velocity = Vector3.Lerp(_velocity, Vector3.zero, _time);
			_time += Time.smoothDeltaTime;
			
			CheckBounds();
		}
		else
		{
			_underInertia = false;
			_time = 0.0f;
		}
	}
	
	private void ZoomInTouch(Gesture gesture)
	{
		var differenceY = gesture.deltaPinch * Time.fixedDeltaTime * PinchSpeedY;
		var differenceZ = gesture.deltaPinch * Time.fixedDeltaTime * PinchSpeedZ;
		if ((MaxPosition.y <= transform.localPosition.y && differenceY < 0f) || (MinPosition.y >= transform.localPosition.y && differenceY > 0f))
			return;
		
		transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y - differenceY, transform.localPosition.z + differenceZ);
		CheckBounds();
	}
	
	private void ZoomMouseWheel(float d)
	{
		var difference = d*Time.fixedDeltaTime*ScrollWheelSpeed;
		
		if ((MaxPosition.y <= transform.localPosition.y && difference < 0f) ||
		(MinPosition.y >= transform.localPosition.y && difference > 0f))
			return;
		
		transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y - difference, transform.localPosition.z + difference);
		CheckBounds();
	}
	
	private void CheckBounds()
	{
		if (MaxPosition == Vector3.zero && MinPosition == Vector3.zero)
			return;
		
		var tempX = transform.localPosition.x;
		var tempY = transform.localPosition.y;
		var tempZ = transform.localPosition.z;
		
		if (MaxPosition.x < transform.localPosition.x)
			tempX = MaxPosition.x;
		if (MaxPosition.y < transform.localPosition.y)
			tempY = MaxPosition.y;
		if (MaxPosition.z < transform.localPosition.z)
			tempZ = MaxPosition.z;
		
		if (MinPosition.x > transform.localPosition.x)
			tempX = MinPosition.x;
		if (MinPosition.y > transform.localPosition.y)
			tempY = MinPosition.y;
		if (MinPosition.z > transform.localPosition.z)
			tempZ = MinPosition.z;
		
		transform.localPosition = new Vector3(tempX, tempY, tempZ);
	}
	
	
}

public enum CameraState
{
	Moveable,
	Locked,
	Animating,
	FinshedAnimating,
	WaitingToReturn,
	Returning
}
