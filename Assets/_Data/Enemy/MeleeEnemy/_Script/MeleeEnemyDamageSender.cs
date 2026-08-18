using UnityEngine;

public class MeleeEnemyDamageSender : DamageSender
{
    [SerializeField] protected MeleeEnemyController meleeEnemyController;
    protected override void LoadComponent()
    {
        base.LoadComponent();
        this.LoadMeleeEnemyController();
        this.SetBaseDamage(this.meleeEnemyController.EntitySO.baseDamage);
        this.SetDamageMultiplier(this.meleeEnemyController.EntitySO.damageMultiplier);
    }
    private void OnTriggerEnter(Collider other)
    {
        DamageReceiver dameReceiver=other.transform.parent?.GetComponentInChildren<DamageReceiver>();
        if (dameReceiver == null || dameReceiver is MeleeEnemyDamageReceiver ||dameReceiver is RangedEnemyDamageReceiver) return;
        float damage = this.CalculateDamage();
        dameReceiver.ReduceHp(damage);
    }
    protected virtual void LoadMeleeEnemyController()
    {
        if (this.meleeEnemyController != null) return;
        this.meleeEnemyController = GetComponentInParent<MeleeEnemyController>();
        Debug.LogWarning(transform.name + " : LoadMeleeEnemyController");
    }
}
