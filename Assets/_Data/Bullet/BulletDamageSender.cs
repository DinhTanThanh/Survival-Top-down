using UnityEngine;
using UnityEngine.EventSystems;

public class BulletDamageSender : DamageSender
{
    [SerializeField] protected BulletController bulletController;
    protected override void LoadComponent()
    {
        base.LoadComponent();
        this.LoadBulletController();
        this.SetBaseDamage(this.bulletController.WeaponData.baseDamage);
        this.SetDamageMultiplier(this.bulletController.WeaponData.damageMultiplier);
    }
    protected virtual void LoadBulletController()
    {
        if (this.bulletController != null) return;
        this.bulletController = GetComponentInParent<BulletController>();
        Debug.LogWarning(transform.name + " : LoadBulletController");
    }
    private void OnTriggerEnter(Collider other)
    {
        
    }
}
