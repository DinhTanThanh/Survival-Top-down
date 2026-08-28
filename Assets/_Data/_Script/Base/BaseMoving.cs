using UnityEngine;

public class BaseMoving : LoadMonoBehaviour
{
    [SerializeField] protected float speedMovement;
    [SerializeField] protected float speedRotation;
    [SerializeField] protected float attackRange;
    [SerializeField] protected Transform enemyRoot;
    [SerializeField] protected Transform target;
    [SerializeField] protected Rigidbody rb;
    [SerializeField] protected BaseEntityController baseEntityController;
    public float AttackRange => attackRange;
    protected override void LoadComponent()
    {
        base.LoadComponent();
        this.LoadBaseEntityController();
        this.LoadRigidbody();
        this.SetSpeedMovement(this.baseEntityController.EntitySO.baseSpeed);
        this.SetSpeedRotation(this.baseEntityController.EntitySO.baseRotation);
        this.SetAttackRanged(this.baseEntityController.EntitySO.attackRange);
        this.SetEnemyRoot(this.transform.parent);
        this.SetTarget(this.baseEntityController != null ? this.baseEntityController.Target : null);
    }

    protected virtual void LoadRigidbody()
    {
        if (this.rb != null) return;
        this.rb = GetComponentInParent<Rigidbody>();
        Debug.LogWarning(transform.name + " : LoadRigidbody");
    }
    protected virtual void SetAttackRanged(float attackRanged)
    {
        this.attackRange = attackRanged;
    }
    protected virtual void SetEnemyRoot(Transform enemyRoot)
    {
        this.enemyRoot = enemyRoot;
    }

    protected virtual void SetTarget(Transform target)
    {
        if (target == null) return;
        this.target = target;
    }

    public virtual void SetSpeedMovement(float speedMovement)
    {
        this.speedMovement = speedMovement;
    }

    public virtual void SetSpeedRotation(float speedRotation)
    {
        this.speedRotation = speedRotation;
    }

    protected virtual void LoadBaseEntityController()
    {
        if (this.baseEntityController != null) return;
        this.baseEntityController = GetComponentInParent<BaseEntityController>();
        Debug.LogWarning(transform.name + " : LoadBaseEntityController");
    }

    public virtual void Moving()
    {
        if (this.target == null || this.rb == null) return;

        Vector3 direction = this.target.position - this.rb.position;
        direction.y = 0f;

        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            Quaternion newRotation = Quaternion.RotateTowards(this.rb.rotation, targetRotation, this.speedRotation * Time.fixedDeltaTime);
            this.rb.MoveRotation(newRotation);
        }
        Vector3 moveDirection = this.rb.rotation * Vector3.forward;
        Vector3 moveDelta = moveDirection * (this.speedMovement * Time.fixedDeltaTime);
        this.rb.MovePosition(this.rb.position + moveDelta);
    }
}

