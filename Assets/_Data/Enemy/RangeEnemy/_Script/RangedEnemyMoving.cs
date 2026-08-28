using UnityEngine;

public class RangedEnemyMoving : BaseMoving
{
    protected override void LoadComponent()
    {
        base.LoadComponent();
        this.SetAttackRanged(this.baseEntityController.EntitySO.attackRange);
    }
    public virtual void RangedMoving()
    {
        if (!this.IsReachedLimit())
        {
            this.Moving();
        }
    }
    public virtual bool IsReachedLimit()
    {
        if (Vector3.Distance(this.enemyRoot.position, this.target.position) > (this.attackRange - 0.3f)) return false;
        return true;
    }
}
