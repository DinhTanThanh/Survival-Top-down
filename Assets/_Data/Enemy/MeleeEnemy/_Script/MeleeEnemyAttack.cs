using UnityEngine;

public class MeleeEnemyAttack : LoadMonoBehaviour
{
    [SerializeField] protected float attackRange;
    [SerializeField] protected bool isAttack;
    [SerializeField] protected Transform target;
    [SerializeField] protected Transform enemyRoot;
    [SerializeField] protected MeleeEnemyController meleeEnemycontroller;
    protected override void LoadComponent()
    {
        base.LoadComponent();
        this.LoadMeleeEnemyController();
        this.SetTarget(this.meleeEnemycontroller.Target);
        this.SetEnemyRoot(this.meleeEnemycontroller.transform);
        this.SetAttackRange(this.meleeEnemycontroller.EntitySO.attackRange);
    }
    protected virtual void LoadMeleeEnemyController()
    {
        if (this.meleeEnemycontroller != null) return;
        this.meleeEnemycontroller = GetComponentInParent<MeleeEnemyController>();
        Debug.LogWarning(transform.name + " : LoadMeleeEnemyController");
    }
    public virtual void SetIsAttack(bool IsAttack)
    {
        this.isAttack = IsAttack;
    }
    public virtual bool GetIsAttack()
    {
        return this.isAttack;
    }
    protected virtual void SetAttackRange(float attackRange)
    {
        this.attackRange = attackRange;
    }
    protected virtual void SetTarget(Transform target)
    {
        if (target == null) return;
        this.target = target;
    }
    protected virtual void SetEnemyRoot(Transform enemyRoot)
    {
        if (enemyRoot == null) return;
        this.enemyRoot= enemyRoot;
    }
    public virtual bool IsReachedDistance()
    {
        if (this.target == null || this.enemyRoot == null) return false;
        if (Vector3.Distance(this.target.position, this.enemyRoot.position) > this.attackRange) return false;
        return true;
    }

    public virtual bool IsReachedAngleAttack()
    {
        Vector3 directionToTarget = this.target.position - this.enemyRoot.position;
        directionToTarget.y = 0f;
        directionToTarget.Normalize();

        float angle = Vector3.Angle(this.enemyRoot.forward, directionToTarget);
        if (angle > 45f) return false;
        return true;
    }
}
