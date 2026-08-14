using UnityEngine;

public class MeleeEnemyAttack : LoadMonoBehaviour
{
    [SerializeField] protected bool isAttack;
    [SerializeField] protected float attackRange;
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
    public virtual void SetIsAttack(bool isAttack)
    {
        this.isAttack = isAttack;
    }
    public virtual bool GetIsAttack()
    {
        return this.isAttack;
    }
    private void Update()
    {
        this.ExecuteMeleeAttack();
    }
    protected virtual bool IsReachedDistance()
    {
        if (Vector3.Distance(this.target.position, this.enemyRoot.position) > this.attackRange) return false;
        return true;
    }
    protected virtual void ExecuteMeleeAttack()
    {
        if (!this.IsReachedDistance()) return;
        Vector3 directionToTarget = this.target.position - this.enemyRoot.position;
        directionToTarget.y = 0f;
        directionToTarget.Normalize();
        float angle = Vector3.Angle(this.enemyRoot.forward, directionToTarget);
        if (angle > 25f) return;
        if (this.isAttack) return;
        this.meleeEnemycontroller.Animator.SetTrigger("Attack");
        this.meleeEnemycontroller.MeleeEnemyMoving.SetSpeedMovement(0f);
        this.SetIsAttack(true);
    }
}
