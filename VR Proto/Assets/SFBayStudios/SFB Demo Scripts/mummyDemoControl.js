#pragma strict

var mummy		: GameObject;
var animator	: Animator;

function Start(){
	animator	= mummy.GetComponent.<Animator>();
}

function ResetMummy(){
	mummy.transform.position	= Vector3(0,0,0);
	mummy.transform.eulerAngles	= Vector3(0,0,0);
}

function locomotion(newValue : float){
	animator.SetFloat("locomotion", newValue);
}