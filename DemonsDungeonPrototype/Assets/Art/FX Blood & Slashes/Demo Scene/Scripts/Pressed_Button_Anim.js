#pragma strict
var ENABLED : boolean = true;
var initial_size_x : float;
var initial_size_y : float;
var factor : float = 0.75;
var speed : float = 1.5;
var GO : boolean = false;
var mySR : SpriteRenderer;


function Start () {
	initial_size_x = this.transform.localScale.x;
	initial_size_y = this.transform.localScale.y;
}

function FixedUpdate () {
if(GO){

	if(this.transform.localScale.y < initial_size_y){
		this.transform.localScale.x *= speed;
		this.transform.localScale.y *= speed;
	}else{
		this.transform.localScale.x = initial_size_x;
		this.transform.localScale.y = initial_size_y;
		GO = false;
	}

}
}


function Enable(spr : Sprite, state : boolean){

	ENABLED = state;
	mySR.sprite = spr;

}


function Go(){
if(ENABLED){

	GO = true;
	this.transform.localScale.x = initial_size_x*factor;
	this.transform.localScale.y = initial_size_y*factor;

}
}