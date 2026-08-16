using UnityEngine;

public class RangedEnemyMoving : BaseMoving
{
    [SerializeField] protected float attackRange;
    protected override void LoadComponent()
    {
        base.LoadComponent();
        this.SetAttackRange(this.baseEntityController.EntitySO.attackRange);
    }
    protected virtual void SetAttackRange(float attackRange)
    {
        this.attackRange = attackRange;
    }
    private void FixedUpdate()
    {
        if (Vector3.Distance(this.target.position, this.enemyRoot.position) > this.attackRange)
        {
            this.Moving();
        }
    }
}
