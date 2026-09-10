using System.Collections;
using UnityEngine;

public class SwingAttackStrategy : BaseBossAttackStrategy
{
    public SwingAttackStrategy(LargeEnemyController controller, LargeAttack attackComponent) : base(controller, attackComponent)
    {
        this.minRange = 0f;
        this.maxRange = 3.2f;
        this.duration = 0.75f;
        this.cooldown = 1.5f;
    }

    protected override IEnumerator AttackRoutine()
    {
        this.RotateTowardsTarget();
        this.PlayAnimation("MT02_SwingAttack", "MT02_Attack01 1", "SwingAttack");

        yield return new WaitForSeconds(this.duration);
    }
}
