using UnityEngine;

public class RangedEnemyAttackCoolDown : CoolDown
{
    [SerializeField] protected RangedEnemyAttack rangedEnemyAttack;
    [SerializeField] protected RangedEnemyController rangedEnemyController;
    protected override void LoadComponent()
    {
        base.LoadComponent();
        this.SetTimeDelay(1f);
        this.LoadRangedEnemyAttack();
        this.LoadRangedEnemyController();
    }
    protected virtual void LoadRangedEnemyController()
    {
        if (this.rangedEnemyController != null) return;
        this.rangedEnemyController=GetComponentInParent<RangedEnemyController>();
        Debug.LogWarning(transform.name + " : LoadRangedEnemyController");
    }
    protected virtual void LoadRangedEnemyAttack()
    {
        if (this.rangedEnemyAttack != null) return;
        this.rangedEnemyAttack = GetComponent<RangedEnemyAttack>();
        Debug.LogWarning(transform.name + " : LoadRangedEnemyAttack");
    }
    private void Update()
    {
        if (!this.rangedEnemyAttack.IsAttack) return;
        if (!this.Timing()) return;
        this.rangedEnemyAttack.SetAttack(false);
        this.rangedEnemyController.RangedEnemyMoving.SetSpeedMovement(this.rangedEnemyController.EntitySO.baseSpeed);
    }
}
