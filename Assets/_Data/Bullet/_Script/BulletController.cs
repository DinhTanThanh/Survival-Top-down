using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.Rendering;

public class BulletController : BaseWeaponController
{
    [SerializeField] protected Transform firePointStart;
    [SerializeField] protected GameObject hitVFX;
    public Transform FirePointStart => firePointStart;
    public GameObject HitVFX => hitVFX;
    public GameObject VFX_SphereVio => hitVFX;

    protected override void LoadComponent()
    {
        base.LoadComponent();
        this.LoadFirePointStart();
        this.LoadHitVFX();
    }
    protected virtual void LoadHitVFX()
    {
        if (this.hitVFX != null) return;
        this.hitVFX = GameObject.Find("VFX_SphereVio");
        Debug.LogWarning(transform.name + " : LoadHitVFX");
    }
    protected virtual void LoadFirePointStart()
    {
        if (this.firePointStart != null) return;
        this.firePointStart = GameObject.Find("Player")?.transform;
        Debug.LogWarning(transform.name + " : LoadFirePointStart");
    }
}
