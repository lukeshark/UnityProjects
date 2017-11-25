using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageHandler_Monster : vp_DamageHandler {

    public Animator _animator;

    protected override void Awake()
    {
        base.Awake();

        //_animator = GetComponent<Animator>();
    }

    public override void Damage(vp_DamageInfo damageInfo)
    {
        base.Damage(damageInfo);

        _animator.SetTrigger("Hit");
    }


    public override void Die()
    {
        if (!enabled || !vp_Utility.IsActive(gameObject))
            return;

        if (m_Audio != null)
        {
            m_Audio.pitch = Time.timeScale;
            m_Audio.PlayOneShot(DeathSound);
        }

        _animator.SetBool("Die", true);

        foreach (GameObject o in DeathSpawnObjects)
        {
            if (o != null)
            {
                GameObject g = (GameObject)vp_Utility.Instantiate(o, Transform.position, Transform.rotation);
                if ((Source != null) && (g != null))
                    vp_TargetEvent<Transform>.Send(g.transform, "SetSource", OriginalSource);
            }
        }

      



    }

}
