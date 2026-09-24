using System.Collections;
using UnityEngine;

public interface IStrategy
{
    public void Execute();
}

public abstract class BaseBossAttackStrategy : IStrategy
{
    protected LargeEnemyController controller;
    protected LargeAttack attackComponent;
    protected float duration;
    protected float cooldown;
    protected float lastExecuteTime = -999f;
    protected float minRange;
    protected float maxRange;
    protected GameObject activeIndicator;

    public float Duration => duration;
    public float Cooldown => cooldown;
    public float MinRange => minRange;
    public float MaxRange => maxRange;
    public bool IsOnCooldown => Time.time < lastExecuteTime + cooldown;
    public GameObject ActiveIndicator => activeIndicator;

    public BaseBossAttackStrategy(LargeEnemyController controller, LargeAttack attackComponent)
    {
        this.controller = controller;
        this.attackComponent = attackComponent;
    }

    public virtual bool CanExecute(float distanceToTarget)
    {
        if (IsOnCooldown) return false;
        if (distanceToTarget < minRange || distanceToTarget > maxRange) return false;
        return true;
    }

    public virtual void Execute()
    {
        this.lastExecuteTime = Time.time;
        if (this.attackComponent != null)
        {
            this.attackComponent.StartCoroutine(this.AttackRoutine());
        }
    }

    protected abstract IEnumerator AttackRoutine();

    public virtual GameObject ShowIndicator(string indicatorName, Vector3 position, Quaternion rotation)
    {
        this.HideIndicator();
        if (IndicatorSystem.Instance != null)
        {
            this.activeIndicator = IndicatorSystem.Instance.SpawnIndicator(indicatorName, position, rotation);
        }
        return this.activeIndicator;
    }

    public virtual void HideIndicator()
    {
        if (this.activeIndicator != null)
        {
            if (IndicatorSystem.Instance != null)
            {
                IndicatorSystem.Instance.DespawnIndicator(this.activeIndicator);
            }
            this.activeIndicator = null;
        }
    }

    public virtual void Cancel()
    {
        this.HideIndicator();
    }

    protected virtual void RotateTowardsTarget()
    {
        if (this.controller == null || this.controller.Target == null) return;
        Vector3 direction = this.controller.Target.position - this.controller.transform.position;
        direction.y = 0f;
        if (direction.sqrMagnitude > 0.001f)
        {
            this.controller.transform.rotation = Quaternion.LookRotation(direction);
        }
    }

    protected virtual void PlayAnimation(params string[] stateNames)
    {
        if (this.controller == null || this.controller.Animator == null) return;
        Animator animator = this.controller.Animator;
        foreach (string stateName in stateNames)
        {
            int hash = Animator.StringToHash(stateName);
            if (animator.HasState(0, hash))
            {
                animator.CrossFadeInFixedTime(stateName, 0.1f);
                return;
            }
        }
    }

    protected virtual void DealDamageInSphere(Vector3 center, float radius, float damageMultiplier)
    {
        if (this.controller == null) return;
        float baseDamage = this.controller.EntitySO.baseDamage;
        float multiplier = this.controller.EntitySO.damageMultiplier;
        float totalDamage = baseDamage * (1f + multiplier) * damageMultiplier;

        Collider[] hitColliders = Physics.OverlapSphere(center, radius);
        foreach (Collider col in hitColliders)
        {
            if (col == null) continue;
            DamageReceiver receiver = col.transform.parent?.GetComponentInChildren<DamageReceiver>();
            if (receiver == null || receiver is not PlayerDamageReceiver) continue;
            if (receiver != this.controller.DamageReceiver)
            {
                receiver.SetIsTakeDamage(true);
                receiver.ReduceHp(totalDamage);
            }
        }
    }
}
