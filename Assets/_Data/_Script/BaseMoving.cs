using UnityEngine;

public class BaseMoving : LoadMonoBehaviour
{
    [SerializeField] protected bool isRuning;
    [SerializeField] protected bool stateRuningCurrent;
    [SerializeField] protected float speedMovement;
    [SerializeField] protected bool isForcedRunning;
    [SerializeField] protected Transform enemyRoot;
    [SerializeField] protected Transform target;
    [SerializeField] protected Rigidbody rb;
    [SerializeField] protected BaseEntityController baseEntityController;
    protected override void LoadComponent()
    {
        base.LoadComponent();
        this.LoadBaseEntityController();
        this.LoadRigidbody();
        this.SetSpeedMovement(this.baseEntityController.EntitySO.baseSpeed);
        this.SetEnemyRoot(this.transform.parent);
        this.SetTarget(this.baseEntityController.Target);
    }
    protected virtual void LoadRigidbody()
    {
        if (this.rb != null) return;
        this.rb = GetComponentInParent<Rigidbody>();
        Debug.LogWarning(transform.name + " : LoadRigidbody");
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
    protected virtual void LoadBaseEntityController()
    {
        if (this.baseEntityController != null) return;
        this.baseEntityController = GetComponentInParent<BaseEntityController>();
        Debug.LogWarning(transform.name + " : LoadBaseEntityController");
    }
    public virtual void SetForcedRunning(bool isForced)
    {
        this.isForcedRunning = isForced;
    }

    protected virtual void Moving()
    {
        //if (!this.isRuning) return;
        Vector3 direction = this.target.position - this.enemyRoot.position;
        direction.y = 0f;
        if (direction != Vector3.zero) 
        {
            this.enemyRoot.rotation = Quaternion.LookRotation(direction);
        }
        Vector3 newPosition = Vector3.forward * this.speedMovement * Time.deltaTime;
        bool isMoving = newPosition != Vector3.zero || this.isForcedRunning;
        if (this.baseEntityController != null && this.baseEntityController.Animator != null)
        {
            this.baseEntityController.Animator.SetBool("IsRunning", isMoving);
        }
        this.stateRuningCurrent = isMoving;
        this.enemyRoot.Translate(newPosition);
    }
}
