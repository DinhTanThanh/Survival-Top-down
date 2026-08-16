using UnityEngine;

public class MeleeEnemyAttackCoolDown : CoolDown
{
    [SerializeField] protected MeleeEnemyController meleeEnemyController;
    protected override void LoadComponent()
    {
        base.LoadComponent();
        this.SetTimeDelay(1f);
        this.LoadMeleeEnemyController();
    }
    protected virtual void LoadMeleeEnemyController()
    {
        if (this.meleeEnemyController != null) return;
        this.meleeEnemyController = GetComponentInParent<MeleeEnemyController>();
        Debug.LogWarning(transform.name + " : LoadMeleeEnemyController");
    }
    private void Update()
    {
        if (!this.meleeEnemyController.MeleeEnemyAttack.GetIsAttack()) return;
        if (!this.Timing()) return;
        this.meleeEnemyController.MeleeEnemyAttack.SetIsAttack(false);
        this.meleeEnemyController.MeleeEnemyAttack.SetHasTriggeredAttack(false);
        this.meleeEnemyController.MeleeEnemyMoving.SetSpeedMovement(this.meleeEnemyController.EntitySO.baseSpeed);
    }
}
