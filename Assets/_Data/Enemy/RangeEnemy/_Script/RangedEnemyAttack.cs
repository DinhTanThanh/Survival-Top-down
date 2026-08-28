using UnityEngine;

public class RangedEnemyAttack : LoadMonoBehaviour
{
    [SerializeField] protected bool isAttack;
    [SerializeField] protected RangedEnemyController rangedEnemyController;
    public bool IsAttack => isAttack;
    protected override void LoadComponent()
    {
        base.LoadComponent();
        this.LoadRangedEnemyController();
    }
    protected virtual void LoadRangedEnemyController()
    {
        if (this.rangedEnemyController != null) return;
        this.rangedEnemyController = GetComponentInParent<RangedEnemyController>();
        Debug.LogWarning(transform.name + " : LoadRangedEnemyController");
    }
    public virtual void SetAttack(bool isAttack)
    {
        this.isAttack = isAttack;
    }
    public virtual bool GetAttack()
    {
        return this.isAttack;
    }
    public virtual bool isReachedAttackRange()
    {
        if (Vector3.Distance(this.transform.parent.position, this.rangedEnemyController.Target.position) > this.rangedEnemyController.RangedEnemyMoving.AttackRange) return false;
        return true;
    }
}
