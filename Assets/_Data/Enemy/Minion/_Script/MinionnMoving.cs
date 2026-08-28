using UnityEngine;

public class MinionnMoving : LoadMonoBehaviour
{
    [SerializeField] protected float attackRange;
    [SerializeField] protected float speedMovement;
    [SerializeField] protected float speedRotation;
    [SerializeField] protected Vector3 destinationPosition;
    [SerializeField] protected MinionController controller;
    private void FixedUpdate()
    {
        if (this.IsReachedTargetLimit()) return;
        this.Moving();
    }
    protected override void LoadComponent()
    {
        base.LoadComponent();
        this.LoadMinionController();
        this.SetAttackRange(this.controller.EntitySO.attackRange);
        this.SetSpeedRotation(this.controller.EntitySO.baseRotation);
        this.SetSpeedMovement(this.controller.EntitySO.baseSpeed);
    }
    protected virtual bool IsReachedTargetLimit()
    {
        if (Vector3.Distance(this.destinationPosition, this.transform.parent.position) > this.attackRange - 0.3f) return false;
        return true;
    }
    protected virtual void Moving()
    {
        Vector3 directionToTarget = this.destinationPosition - this.transform.parent.position;
        directionToTarget.y = 0f;
        if (directionToTarget.sqrMagnitude > 0.05f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);
            Quaternion newRotation = Quaternion.RotateTowards(this.controller.Rb.rotation, targetRotation, this.speedRotation * Time.fixedDeltaTime);
            this.controller.Rb.MoveRotation(newRotation);

            Vector3 moveDirection = this.controller.Rb.rotation * Vector3.forward;
            Vector3 newDelta = moveDirection * (this.speedMovement*Time.fixedDeltaTime);
            this.controller.Rb.MovePosition(this.controller.Rb.position + newDelta);
        }
    }
    protected virtual void LoadMinionController()
    {
        if (this.controller != null) return;
        this.controller = GetComponentInParent<MinionController>();
        Debug.LogWarning(transform.name + " : LoadMinionController");
    }
    protected virtual void SetAttackRange(float attackRange)
    {
        this.attackRange = attackRange;
    }
    protected virtual void SetSpeedRotation(float speedRotation)
    {
        this.speedRotation = speedRotation;
    }
    protected virtual void SetSpeedMovement(float speedMovement)
    {
        this.speedMovement = speedMovement;
    }
}
