using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseWeapon : BaseObject
{
    [SerializeField] protected Transform gunT;
    [SerializeField] protected ParticleSystem muzzleFlash;
    [SerializeField] protected GameObject hitParticle;
    [SerializeField] protected bool _fire = true;
    [SerializeField] protected Transform TMCam;
    [SerializeField] protected GameObject cross;

    protected override void Awake()
    {
        base.Awake();

        gunT = transform.GetChild(2);
        muzzleFlash = GetComponentInChildren<ParticleSystem>();
        hitParticle = Resources.Load<GameObject>("Prefabs/Flare");
        TMCam = Camera.main.transform;
    }

    public abstract void Fire();
}
