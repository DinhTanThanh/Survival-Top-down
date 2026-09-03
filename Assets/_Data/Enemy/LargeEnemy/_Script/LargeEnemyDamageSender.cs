using UnityEngine;

public class LargeEnemyDamageSender : DamageSender
{
    [SerializeField] protected LargeEnemyController LargeEnemyController;
    protected override void LoadComponent()
    {
        base.LoadComponent();
        this.LoadLargeEnemyController();
        this.SetBaseDamage(this.LargeEnemyController.EntitySO.baseDamage);
        this.SetDamageMultiplier(this.LargeEnemyController.EntitySO.damageMultiplier);
    }
    private void OnTriggerEnter(Collider other)
    {
        DamageReceiver dameReceiver = other.transform.parent?.GetComponentInChildren<DamageReceiver>();
        if (dameReceiver == null || dameReceiver is MeleeEnemyDamageReceiver || dameReceiver is RangedEnemyDamageReceiver) return;
        float damage = this.CalculateDamage();
        dameReceiver.ReduceHp(damage);
    }
    protected virtual void LoadLargeEnemyController()
    {
        if (this.LargeEnemyController != null) return;
        this.LargeEnemyController = GetComponent<LargeEnemyController>();
        Debug.LogWarning(transform.name + " : LoadLargeEnemyController");
    }
}
