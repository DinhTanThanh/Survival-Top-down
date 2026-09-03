using System.Collections.Generic;
using UnityEngine;

public class VFX_VioExplosion : DamageSender
{
    [SerializeField] protected float explosionRadius = 1.2f;
    [SerializeField] protected BulletController bulletController;
    protected override void LoadComponent()
    {
        base.LoadComponent();
        this.LoadBulletController();
        if (this.bulletController != null && this.bulletController.WeaponData != null)
        {
            this.SetBaseDamage(this.bulletController.WeaponData.baseDamage);
            this.SetDamageMultiplier(this.bulletController.WeaponData.damageMultiplier);
        }
    }
    protected virtual void LoadBulletController()
    {
        if (this.bulletController != null) return;
        this.bulletController = FindFirstObjectByType<BulletController>();
        Debug.LogWarning(transform.name + " : LoadBulletController");
    }

    private void OnEnable()
    {
        this.ExecuteExplosion();
    }
    public virtual void ExecuteExplosion()
    {
        if (this.bulletController == null)
        {
            this.LoadBulletController();
        }
        if (this.bulletController != null && this.bulletController.WeaponData != null)
        {
            this.SetBaseDamage(this.bulletController.WeaponData.baseDamage);
            this.SetDamageMultiplier(this.bulletController.WeaponData.damageMultiplier);
        }
        Collider[] hitColliders = Physics.OverlapSphere(this.transform.position, this.explosionRadius);
        HashSet<DamageReceiver> damagedReceivers = new HashSet<DamageReceiver>();
        foreach (Collider hitCollider in hitColliders)
        {
            if (hitCollider == null) continue;
            DamageReceiver damageReceiver = hitCollider.transform.parent?.GetComponentInChildren<DamageReceiver>();
            if (damageReceiver == null)
            {
                damageReceiver = hitCollider.GetComponentInParent<DamageReceiver>();
            }

            if (damageReceiver == null || damageReceiver is PlayerDamageReceiver) continue;

            if (damagedReceivers.Add(damageReceiver))
            {
                damageReceiver.SetIsTakeDamage(true);
                float damage = this.CalculateDamage();
                damageReceiver.ReduceHp(damage);
            }
        }
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, this.explosionRadius);
    }
}
