using UnityEngine;
using System.Collections;

namespace HutongGames.PlayMaker.Actions
{

	[ActionCategory ("Agents AI")]
	[Tooltip ("This action shoot a gun")]
	public class Shoot : FsmStateAction
	{
		[Tooltip ("The time that the agent wait between each shoot")]
		public FsmFloat timeBetweenBullets = 0.15f;
		[Tooltip ("The distance of the bullet")]
		public FsmFloat range = 100f;

		[UIHint (UIHint.Layer)]
		[Tooltip ("The layer of the gameobjects that the agent can shoot")]
		public FsmInt[] ShootableMask;

		[Tooltip ("The gameobject where the bullet come out")]
		public FsmGameObject GunGameobject;
		[Tooltip ("Would you like to play an animation when the agent shoot?")]
		public FsmBool PlayShootAnimation;
		[UIHint (UIHint.AnimatorTrigger)]

		[Tooltip ("The animation that will be played when the agent shoot. If PlayShootAnimation is false you haven't to fill this")]
		public FsmString TriggerShootAnimation;

		[Tooltip ("The event that will be fire when the agent hit the target")]
		public FsmEvent OnPlayerHit;

		[Tooltip ("Do you want to the agent can reload?")]
		public FsmBool AgentCanReload;

		[Tooltip ("A number of bullets that the agent can shoot before of reload. If AgentCanReload is false dont fill this field")]
		public FsmInt AmmunitionForCartridge = 10;

		[UIHint (UIHint.AnimatorTrigger)]
		[Tooltip ("The parameter in the animator.If AgentCanReload is false do not fill this field")]
		public FsmString TriggerAnimationReload;

		private int AmmunitionRemaining;

		[Tooltip ("Do you want to turn on a light when the agent shoot?")]
		public FsmBool GunLight;

		[Tooltip ("Would you like to play an audio when the agent shoot?")]
		public bool PlayAudioOnShoot;

		[Tooltip ("The seconds that the agent will take reloading")]
		public FsmFloat SecondsReloading = 1.5f;
		[Tooltip ("Do you want that the pistol play a muzzle flash when the agent shoot")]
		public bool PlayMuzzleFlash;

		[ObjectType (typeof(AudioClip))]
		[Tooltip ("The audio that will be played when the agent shoot. If PlayAudioOnShoot is false dont fill this field")]

		public FsmObject Audio;

		[UIHintAttribute (UIHint.Variable)]
		[Tooltip ("The hit point of the shoot(Only if hit a ShootableMask)")]
		public FsmVector3 HitPoint;

		private AudioSource _audio;
		private Animator _animator;
		private int _paramTriggerReloadID;
		private int _paramTriggerShootID;
		private float timerReload;
		private float timerEffects;
		bool shoothing;
		float _timer;
		Ray _shootRay;
		RaycastHit shootHit;
		bool error;
		Light _gunLight;
		private ParticleSystem _particles;
		private int cc;

		public override void Awake ()
		{
			_animator = Owner.GetComponent<Animator> ();
			if (_animator == null) {
				Debug.Log ("The agent must have an animator,please add an animator to the agent " + Owner.gameObject);
				error = true;


			}
			if (PlayMuzzleFlash) {
				_particles = GunGameobject.Value.GetComponent<ParticleSystem> ();


			}
			if (PlayShootAnimation.Value) {
				if (TriggerShootAnimation.Value == null) {
					Debug.Log ("The ShootAnimation field is empty please fill this field or change the value PlayShootAnimation to false");
					error = true;
				} 

			}
		

			if (GunGameobject.Value == null) {
				Debug.Log ("GunGameobject is empty, please fill this field");
				error = true;

			} else {
				if (GunLight.Value) {
					_gunLight = GunGameobject.Value.GetComponent<Light> ();
					if (_gunLight == null) {
						Debug.Log ("The GunGameobject haven't a Light Component,please add to GunGameobject a Light Component or change the value GunLight to false");
						error = true;
					}
				}
			}

			if (PlayAudioOnShoot) {
				if (Audio != null) {
					_audio = Owner.GetComponent<AudioSource> ();
					if (_audio != null)
						_audio.clip = Audio.Value as AudioClip;
					else {
						Debug.Log ("There isn't AudioSource attached to " + Owner.gameObject + " Please add to this gameobject an AudioSource component");
						error = true;
					}
				} else { 
					Debug.Log ("Audio is empty.Please fill this field or change to false the variable type bool PlayAudioOnShoot");
					error = true;

				}
			}
			if (OnPlayerHit == null) {
				Debug.Log ("The field OnPlayerHit is empty, please fill this field");
				error = true;
			}
			if (AgentCanReload.Value) {
				
				if (TriggerAnimationReload == null) {
					Debug.Log ("Please fill the field TriggerAnimationReload or change the value AgentCanReload to false");
					error = true;
				}

				if (AmmunitionForCartridge.Value <= 0) {
					Debug.Log ("The field AmmunitionInCartridge must be greather than 0,please increases this value or change the bool AgentCanReload to false");
					error = true;
				}

			}
			if (ShootableMask.Length <= 0) {
				Debug.Log ("You must fill the field ShootableMask");
				error = true;
			}
		

		
			if (TriggerAnimationReload != null)
				_paramTriggerReloadID = Animator.StringToHash (TriggerAnimationReload.Value);
			if (TriggerShootAnimation != null)
				_paramTriggerShootID = Animator.StringToHash (TriggerShootAnimation.Value);
				

			AmmunitionRemaining = 0;
				

		}

	

		public override void OnUpdate ()
		{
			_timer += Time.deltaTime;


			if (_timer >= timeBetweenBullets.Value) {

				if (AgentCanReload.Value) {
					if (AmmunitionRemaining <= AmmunitionForCartridge.Value) {
						

						ShootWp ();
					} else {
						if (cc == 0) {
							_animator.SetTrigger (_paramTriggerReloadID);
							cc = 1;
						}
						timerReload += Time.deltaTime;
						if (timerReload >= SecondsReloading.Value) {
								
							AmmunitionRemaining = 0;
							timerReload = 0;
							cc = 0;
						}
						//Fsm.Event (reload);
						//Finish ();

					}

				} else {
					if (GunLight.Value)
						_gunLight.enabled = true;
					ShootWp ();
				}

			}



			_shootRay.origin = GunGameobject.Value.transform.position;
			_shootRay.direction = GunGameobject.Value.transform.forward;


		}

		void ShootWp ()
		{
			if (AgentCanReload.Value)
				AmmunitionRemaining = AmmunitionRemaining + 1;
			_timer = 0f;
			if (PlayShootAnimation.Value)
				_animator.SetTrigger (_paramTriggerShootID);
			if (PlayMuzzleFlash) {
				_particles.Stop ();
				_particles.Play ();
			}
				
			if (GunLight.Value) {
				_gunLight.enabled = true;
				StartCoroutine (DissableEffects ());
			}

			if (PlayAudioOnShoot && _audio != null)
				_audio.Play ();



			RaycastHit hit;
			if (Physics.Raycast (_shootRay, out hit, range.Value, ActionHelpers.LayerArrayToLayerMask (ShootableMask, false))) {

				if (HitPoint.Value != null)
					HitPoint.Value = hit.point;

				Fsm.Event (OnPlayerHit);
				Finish ();
			}

		}

		IEnumerator DissableEffects ()
		{
			yield return new WaitForSeconds (timeBetweenBullets.Value);
			_gunLight.enabled = false;

		}

		public override void OnDrawActionGizmos ()
		{
			Gizmos.color = Color.red;
			if (GunGameobject != null)
				Gizmos.DrawRay (GunGameobject.Value.transform.position, GunGameobject.Value.transform.forward * range.Value);


		}


	}

}
