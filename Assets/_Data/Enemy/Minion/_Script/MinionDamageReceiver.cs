using UnityEngine;

public class MinionDamageReceiver : DamageReceiver
{
    protected override void LoadComponent()
    {
        base.LoadComponent();
        if (this.baseEntityController != null && this.baseEntityController.EntitySO != null)
        {
            this.SetBaseHp(this.baseEntityController.EntitySO.baseHp);
            this.SetHp(this.baseEntityController.EntitySO.baseHp);
            this.SetDefence(this.baseEntityController.EntitySO.baseDefence);
            this.SetDamageMultiplier(this.baseEntityController.EntitySO.damageMultiplier);
            this.SetExperienceReward(this.baseEntityController.EntitySO.expReward);
        }
    }
}
