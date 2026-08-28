using UnityEngine;
public class MeleeEnemyMoving : BaseMoving
{
    protected virtual void OnEnable()
    {
        if (this.baseEntityController != null && this.baseEntityController.EntitySO != null)
        {
            this.SetSpeedMovement(this.baseEntityController.EntitySO.baseSpeed);
        }
    }
    public virtual void MeleeMoving()
    {
        if (!this.IsReachedLimit())
        {
            this.Moving();
        }
    }
    public virtual bool IsReachedLimit()
    {
        if (Vector3.Distance(this.target.position, this.enemyRoot.position) > (this.attackRange-0.3f)) return false;
        return true;
    }
}
