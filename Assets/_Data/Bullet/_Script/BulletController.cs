using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.Rendering;

public class BulletController : BaseWeaponController
{
    [SerializeField] protected Transform firePointStart;
    [SerializeField] protected GameObject vfx_SphereVio;
    public Transform FirePointStart => firePointStart;
    public GameObject VFX_SphereVio => vfx_SphereVio;
    protected override void LoadComponent()
    {
        base.LoadComponent();
        this.LoadFirePointStart();
        this.LoadVFX_SphereVio();
    }
    protected virtual void LoadVFX_SphereVio()
    {
        if (this.vfx_SphereVio != null) return;
        this.vfx_SphereVio = GameObject.Find("VFX_SphereVio");
        Debug.LogWarning(transform.name + " : LoadVFX_SphereVio");
    }
    protected virtual void LoadFirePointStart()
    {
        if (this.firePointStart != null) return;
        this.firePointStart = GameObject.Find("Player")?.transform;
        Debug.LogWarning(transform.name + " : LoadFirePointStart");
    }
}
