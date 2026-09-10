using System.Collections;
using UnityEngine;

public class BossAoEExplosionStrategy : BaseBossAttackStrategy
{
    public BossAoEExplosionStrategy(LargeEnemyController controller, LargeAttack attackComponent) 
        : base(controller, attackComponent)
    {
        this.minRange = 0f;
        this.maxRange = 6.0f;
        this.duration = 2.0f;
        this.cooldown = 8.0f;
    }

    protected override IEnumerator AttackRoutine()
    {
        this.RotateTowardsTarget();
        this.PlayAnimation("MT02_StompAttack", "MT02_Attack10 1");

        yield return new WaitForSeconds(0.85f);

        if (this.controller != null)
        {
            this.DealDamageInSphere(this.controller.transform.position, 5.0f, 2.0f);
        }

        float remaining = this.duration - 0.85f;
        if (remaining > 0f)
        {
            yield return new WaitForSeconds(remaining);
        }
    }
}
