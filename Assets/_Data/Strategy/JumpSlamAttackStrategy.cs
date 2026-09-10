using System.Collections;
using UnityEngine;

public class JumpSlamAttackStrategy : BaseBossAttackStrategy
{
    public JumpSlamAttackStrategy(LargeEnemyController controller, LargeAttack attackComponent) : base(controller, attackComponent)
    {
        this.minRange = 3.0f;
        this.maxRange = 9.0f;
        this.duration = 1.35f;
        this.cooldown = 6.0f;
    }

    protected override IEnumerator AttackRoutine()
    {
        this.RotateTowardsTarget();
        this.PlayAnimation("MT02_JumpSlamAttack", "MT02_Attack13 1", "JumpSlamAttack");

        yield return new WaitForSeconds(0.2f);

        if (this.controller != null && this.controller.Target != null)
        {
            Vector3 startPos = this.controller.transform.position;
            Vector3 targetPos = this.controller.Target.position;
            targetPos.y = startPos.y;

            Vector3 direction = (targetPos - startPos).normalized;
            float targetDistance = Mathf.Max(0f, Vector3.Distance(startPos, targetPos) - 1.2f);
            Vector3 destination = startPos + direction * targetDistance;

            float leapDuration = 0.45f;
            float elapsed = 0f;

            Rigidbody rb = this.controller.GetComponent<Rigidbody>();

            while (elapsed < leapDuration)
            {
                elapsed += Time.deltaTime;
                float t = Mathf.Clamp01(elapsed / leapDuration);
                float smoothT = Mathf.Sin(t * Mathf.PI * 0.5f);
                Vector3 newPos = Vector3.Lerp(startPos, destination, smoothT);

                if (rb != null)
                {
                    rb.MovePosition(newPos);
                }
                else if (this.controller != null)
                {
                    this.controller.transform.position = newPos;
                }

                yield return null;
            }
        }

        yield return new WaitForSeconds(0.05f);

        if (this.controller != null)
        {
            this.DealDamageInSphere(this.controller.transform.position, 3.2f, 1.75f);
        }

        float remaining = this.duration - 0.7f;
        if (remaining > 0f)
        {
            yield return new WaitForSeconds(remaining);
        }
    }
}
