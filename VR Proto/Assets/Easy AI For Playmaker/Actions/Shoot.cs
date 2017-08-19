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

		

		[Tooltip ("Do you want to turn on a light when the agent shoot?")]
		public FsmBool GunLight;

		[Tooltip ("Would you like to play an audio when the agent shoot?")]
		public bool PlayAudioOnShoot;

		[Tooltip ("The seconds that the agent will take reloading")]
		public FsmFloat SecondsReloading = 1.5f;
		[Tooltip ("Do you want that the pistol play a muzzle flash when the agent shoot")]
		public bool PlayMuzzleFlash;

		[ObjectType (typeof(AudioClip))]
		[Tooltip ("The audio that will be played when the agent shoot. If PlayAudioOnShoot is false dont fill this field. The audio source need to be in the Owner Gameobject")]

		public FsmObject AudioShoot;
		[ObjectType (typeof(AudioClip))]
		[Tooltip ("The audio that will be played when the agent reload. The audio source need to be in the Gun Gameobject")]
		public FsmObject AudioReload;

		[UIHintAttribute (UIHint.Variable)]
		public FsmGameObject StoreTheTarget;

		public FsmBool ShootOnLeftClick;

		[UIHintAttribute (UIHint.Variable)]
		[Tooltip ("The hit point of the shoot(Only if hit a ShootableMask)")]
		public FsmVector3 HitPoint;
		
		
		private int AmmunitionRemaining;
		private AudioSource _audioShoot;
		private AudioSource _audioReload;

		private Animator _animator;
		private int _paramTriggerReloadID;
		private int _paramTriggerShootID;
		private float timerReload;
		private float timerEffects;
		bool shoothing;
		float _timer;
		Ray _shootRay;
		RaycastHit shootHit;
		Light _gunLight;
		private ParticleSystem _particles;
		private int cc;

		private int ccReload;

		public override void OnEnter ()
		{
			_animator = Owner.GetComponent<Animator> ();
			if (_animator == null) {
				Debug.Log ("The agent must have an animator,please add an animator to the agent " + Owner.gameObject);


			}
			if (PlayMuzzleFlash) {
				_particles = GunGameobject.Value.GetComponent<ParticleSystem> ();


			}
			if (PlayShootAnimation.Value) {
				if (TriggerShootAnimation.Value == null) {
					Debug.Log ("The ShootAnimation field is empty please fill this field or change the value PlayShootAnimation to false");
				} 

			}
		

			if (GunGameobject.Value == null) {
				Debug.Log ("GunGameobject is empty, please fill this field");

			} else {
				if (GunLight.Value) {
					_gunLight = GunGameobject.Value.GetComponent<Light> ();
					if (_gunLight == null) {
						Debug.Log ("The GunGameobject haven't a Light Component,please add to GunGameobject a Light Component or change the value GunLight to false");
					}
				}
			}
			if (AudioReload != null) {
				_audioReload = GunGameobject.Value.GetComponent<AudioSource> ();

				if (_audioReload != null)
					_audioReload.clip = AudioReload.Value as AudioClip;
				

			}

			if (PlayAudioOnShoot) {
				if (AudioShoot != null) {
					_audioShoot = Owner.GetComponent<AudioSource> ();
					if (_audioShoot != null)
						_audioShoot.clip = AudioShoot.Value as AudioClip;
					else {
						Debug.Log ("There isn't AudioSource attached to " + Owner.gameObject + " Please add to this gameobject an AudioSource component");
					}
				} else { 
					Debug.Log ("Audio is empty.Please fill this field or change to false the variable type bool PlayAudioOnShoot");

				}
			}
			if (OnPlayerHit == null) {
				Debug.Log ("The field OnPlayerHit is empty, please fill this field");
			}
			if (AgentCanReload.Value) {
				
				if (TriggerAnimationReload == null) {
					Debug.Log ("Please fill the field TriggerAnimationReload or change the value AgentCanReload to false");
				}

				if (AmmunitionForCartridge.Value <= 0) {
					Debug.Log ("The field AmmunitionInCartridge must be greather than 0,please increases this value or change the bool AgentCanReload to false");
				}

			}
			if (ShootableMask.Length <= 0) {
				Debug.Log ("You must fill the field ShootableMask");
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
			if (!ShootOnLeftClick.Value) {



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
							if (AudioReload != null && _audioReload != null && ccReload == 0) {
								_audioReload.Play ();
								ccReload = 1;
							}
							if (timerReload >= SecondsReloading.Value) {
								if (AudioReload != null && _audioReload != null) {
									_audioReload.Stop ();
									ccReload = 0;
								}
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

			} else {
				if (Input.GetMouseButton (0)) {
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
								timerReload += Time.deltaTime;
								if (AudioReload != null && _audioReload != null && ccReload == 0) {
									_audioReload.Play ();
									ccReload = 1;
								}
								if (timerReload >= SecondsReloading.Value) {
									if (AudioReload != null && _audioReload != null) {
										_audioReload.Stop ();
										ccReload = 0;
									}

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
			}


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

			if (PlayAudioOnShoot && _audioShoot != null)
				_audioShoot.Play ();



			RaycastHit hit;
			if (Physics.Raycast (_shootRay, out hit, range.Value, ActionHelpers.LayerArrayToLayerMask (ShootableMask, false))) {

				if (HitPoint != null)
					HitPoint.Value = hit.point;

				StoreTheTarget.Value = hit.transform.gameObject;

				Fsm.Event (OnPlayerHit);
				Finish ();
			}

		}

		IEnumerator DissableEffects ()
		{
			yield return new WaitForSeconds (0.03f);
			_gunLight.enabled = false;

		}

		public override void OnDrawActionGizmos ()
		{
			Gizmos.color = Color.red;
			if (GunGameobject.Value != null)
				Gizmos.DrawRay (GunGameobject.Value.transform.position, GunGameobject.Value.transform.forward * range.Value);


		}


	}

}
