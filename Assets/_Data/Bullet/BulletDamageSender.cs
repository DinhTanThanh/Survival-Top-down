using UnityEngine;

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
    protected virtual void SetBaseDamage(float baseDamage)
    {
        this.baseDamage = baseDamage;
    }
    protected virtual void SetDamageMultiplier(float damageMultiplier)
    {
        this.damageMultiplier = damageMultiplier;
    }
    private void OnTriggerEnter(Collider other)
    {
        
    }
}
