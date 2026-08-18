using UnityEngine;
public class MeleeEnemyMoving : BaseMoving
{
    [SerializeField] protected float detectionRange;
    protected override void LoadComponent()
    {
        base.LoadComponent();
        this.SetDetectionRange(10f);
    }
    protected virtual void OnEnable()
    {
        if (this.baseEntityController != null && this.baseEntityController.EntitySO != null)
        {
            this.SetSpeedMovement(this.baseEntityController.EntitySO.baseSpeed);
        }
        this.isForcedRunning = false;
        this.stateRuningCurrent = false;
    }
    protected virtual void SetDetectionRange(float detectionRange)
    {
        this.detectionRange= detectionRange;
    }
    private void Update()
    {
        if (this.isRuning) return;
        if (!this.IsDetectionTarget()) return;
        this.isRuning = true;
    }
    private void FixedUpdate()
    {
        this.Moving();
    }
    protected virtual bool IsDetectionTarget()
    {
        if(Vector3.Distance(this.target.position,this.enemyRoot.position)>this.detectionRange) return false;
        return true;
    }
}
