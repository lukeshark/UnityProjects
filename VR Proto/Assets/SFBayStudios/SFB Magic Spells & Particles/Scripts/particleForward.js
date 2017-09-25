
var speed			: float			= 10.0;

function Update(){
	transform.Translate(transform.forward * Time.deltaTime * speed);
}