using UnityEngine;

public class BulletController : BaseWeaponController
{
    [SerializeField] protected Transform firePointStart;
    public Transform FirePointStart => firePointStart;
    protected override void LoadComponent()
    {
        base.LoadComponent();
        this.LoadFirePointStart();
    }
    protected virtual void LoadFirePointStart()
    {
        if (this.firePointStart != null) return;
        this.firePointStart = GameObject.Find("Player")?.transform;
        Debug.LogWarning(transform.name + " : LoadFirePointStart");
    }
}
