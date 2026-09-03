using UnityEngine;

public class MinionDamageSender : DamageSender
{
    [SerializeField] protected MinionController minionController;
    protected override void LoadComponent()
    {
        base.LoadComponent();
        this.LoadMinionController();
        this.SetBaseDamage(this.minionController.EntitySO.baseDamage);
        this.SetDamageMultiplier(this.minionController.EntitySO.damageMultiplier);
    }
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("va cham");
        DamageReceiver dameReceiver = other.transform.parent?.GetComponentInChildren<DamageReceiver>();
        if (dameReceiver == null) return;
        if (dameReceiver is PlayerDamageReceiver)
        {
            float damage = this.CalculateDamage();
            dameReceiver.ReduceHp(damage);
        }
    }
    protected virtual void LoadMinionController()
    {
        if (this.minionController != null) return;
        this.minionController = GetComponent<MinionController>();
        Debug.LogWarning(transform.name + " : LoadMinionController");
    }
}
