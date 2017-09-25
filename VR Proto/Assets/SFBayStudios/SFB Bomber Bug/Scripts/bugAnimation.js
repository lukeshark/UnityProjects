#pragma strict

var charAnimator			: Animator;							// Animation Controller
var desiredWeight			: float			= 0.0;				// Desired Weight of Body Layer
var checkTransitionDone		: boolean		= false;			// Use to find the END of a transition

var spitSpawnPos			: Transform;						// Spawn Position for Spit Particle
var spitParticle			: GameObject;						// Particle used for the cast animation
var explodeParticle			: GameObject;						// Particle used for explosion animation
var explodeRotation			: Vector3		= Vector3(-90,0,0);	// Rotation for explosion.

var bodyObj					: GameObject;
var bodyMat1				: Material[];						// Add on mateirals
var bodyMat2				: Material[];						// main body mateirals

function Start () {
	charAnimator		= GetComponent.<Animator>();
}

function Update(){
	if (charAnimator.GetLayerWeight(1) != desiredWeight){
		charAnimator.SetLayerWeight(1, Mathf.MoveTowards(charAnimator.GetLayerWeight(1), desiredWeight, Time.deltaTime * 3));
	}

	if (charAnimator.IsInTransition(1))											// If we are in transition...
		checkTransitionDone	= true;												// Set this to be true, so that...

	if (checkTransitionDone && !charAnimator.IsInTransition(1))					// If we were in transition and not we aren't....
	{
		checkTransitionDone	= false;											// Set checkTransitionDone to false...
		if (charAnimator.GetCurrentAnimatorStateInfo(1).IsName("fly idle"))		// ...and if we're now in "fly idle"...
			desiredWeight	= 0;												// ...set desiredWeight to 0
	}
}

function UpdateBodyMaterial(value : int){
	var materials	: Material[]	= bodyObj.GetComponent.<Renderer>().materials;
	materials[0]	= bodyMat1[value];
	materials[1]	= bodyMat2[value];
	bodyObj.GetComponent.<Renderer>().materials	= materials;
}

function SetLocomotionSpeed(newSpeed : float){
	charAnimator.SetFloat("locomotion", newSpeed);
}

function SetBodyLayerWeight(newWeight : float){
	desiredWeight = 1;
}

function ExplodeBug(){
	var newParticle		= Instantiate(explodeParticle, transform.position, Quaternion.identity);
	newParticle.transform.eulerAngles	= explodeRotation;
	Destroy(newParticle, 20.0);
}

function CastSpell(){
	var newParticle		= Instantiate(spitParticle, spitSpawnPos.position, Quaternion.identity);
	Destroy(newParticle, 20.0);
}