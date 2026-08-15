using UnityEngine;

public class TripleShotChargeRecovery : BaseChargeSystem
{
    [SerializeField] protected PlayerShooting playerShooting;

    protected override void LoadComponent()
    {
        base.LoadComponent();
        this.LoadPlayerShooting();
    }
    protected virtual void LoadPlayerShooting()
    {
        if (this.playerShooting != null) return;
        this.playerShooting = GetComponentInParent<PlayerShooting>();
        Debug.LogWarning(transform.name + " : LoadPlayerShooting");
    }
    private void Update()
    {
        this.playerShooting.CurrentCharge=this.ChargeRecovery(this.playerShooting.CurrentCharge,this.playerShooting.MaxCharge,this.playerShooting.ChargeRegenTime);
    }
}
