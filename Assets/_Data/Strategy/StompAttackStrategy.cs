using System.Collections;
using UnityEngine;

public class StompAttackStrategy : BaseBossAttackStrategy
{
    public StompAttackStrategy(LargeEnemyController controller, LargeAttack attackComponent) 
        : base(controller, attackComponent)
    {
        this.minRange = 0f;
        this.maxRange = 4.5f;
        this.duration = 2.0f;
        this.cooldown = 5.0f;
    }

    protected override IEnumerator AttackRoutine()
    {
        this.RotateTowardsTarget();
        this.PlayAnimation("MT02_StompAttack", "MT02_Attack10 1", "StompAttack");

        yield return new WaitForSeconds(0.85f);

        if (this.controller != null)
        {
            this.DealDamageInSphere(this.controller.transform.position, 4.2f, 1.35f);
        }

        float remaining = this.duration - 0.85f;
        if (remaining > 0f)
        {
            yield return new WaitForSeconds(remaining);
        }
    }
}
