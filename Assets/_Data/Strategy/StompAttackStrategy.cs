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

        if (this.controller == null) yield return null;
        
            Vector3 localOffset = new Vector3(0.18f, 0f, 1f);
            Vector3 newPos = this.controller.transform.position + this.controller.transform.rotation * localOffset;
            newPos.y = 0.011f;

            float fixedRotX = -90f;
            float fixedRotZ = -25f;
            Quaternion newRot = this.controller.transform.rotation * Quaternion.Euler(fixedRotX, 0f, fixedRotZ);

            this.ShowIndicator("StompIndicator", newPos, newRot);
        

        try
        {
            yield return new WaitForSeconds(0.85f);
            this.ShowIndicator("FX_Stomp", newPos, Quaternion.identity);

            if (this.controller != null)
            {
                this.DealDamageInSphere(this.controller.transform.position, 4.2f, 1.35f);
            }

            float remaining = this.duration - 0.85f;
            if (remaining > 0f)
            {
                yield return new WaitForSeconds(remaining);
                this.HideIndicator();
            }
        }
        finally { }
    }
}
