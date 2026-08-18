using UnityEngine;

public class ProjectileDamageSender : DamageSender
{
    [SerializeField] protected ProjectileController projectileController;
    protected override void LoadComponent()
    {
        base.LoadComponent();
        this.LoadProjectileController();
        this.SetBaseDamage(this.projectileController.WeaponData.baseDamage);
        this.SetDamageMultiplier(this.projectileController.WeaponData.damageMultiplier);
    }
    protected virtual void LoadProjectileController()
    {
        if (this.projectileController != null) return;
        this.projectileController = GetComponent<ProjectileController>();
        Debug.LogWarning(transform.name + " : LoadProjectileController");
    }
    private void OnTriggerEnter(Collider other)
    {
        DamageReceiver damageReceiver = other.transform.parent?.GetComponentInChildren<DamageReceiver>();
        if (damageReceiver == null || damageReceiver is RangedEnemyDamageReceiver || damageReceiver is MeleeEnemyDamageReceiver) return;
        float damage = this.CalculateDamage();
        damageReceiver.ReduceHp(damage);
        PlayerPoisonHandler playerPoisonHandler = other.transform.parent?.GetComponentInChildren<PlayerPoisonHandler>();
        if (playerPoisonHandler == null) return;
        playerPoisonHandler.SetBaseDamagePoison(this.projectileController.WeaponData.baseDamage);
        playerPoisonHandler.Refresh();
    }
}