using UnityEngine;

public class LargeAttack : LoadMonoBehaviour
{
    [SerializeField] protected float attackRange;
    [SerializeField] protected bool isAttack;
    [SerializeField] protected Transform target;
    [SerializeField] protected Transform enemyRoot;
    [SerializeField] protected LargeEnemyController largeEnemyController;
    protected override void LoadComponent()
    {
        base.LoadComponent();
        this.LoadLargeEnemyController();
        this.SetTarget(this.largeEnemyController.Target);
        this.SetEnemyRoot(this.largeEnemyController.transform);
        this.SetAttackRange(this.largeEnemyController.EntitySO.attackRange);
    }
    protected virtual void LoadLargeEnemyController()
    {
        if (this.largeEnemyController != null) return;
        this.largeEnemyController = GetComponentInParent<LargeEnemyController>();
        Debug.LogWarning(transform.name + " : LoadLargeEnemyController");
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
        this.enemyRoot = enemyRoot;
    }
    public virtual bool IsReachedDistance()
    {
        if (this.target == null || this.enemyRoot == null) return false;
        if (Vector3.Distance(this.target.position, this.enemyRoot.position) > this.attackRange) return false;
        return true;
    }
}
