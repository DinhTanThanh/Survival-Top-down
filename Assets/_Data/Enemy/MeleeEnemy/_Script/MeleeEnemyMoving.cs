using Unity.Jobs;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.Rendering;

public class MeleeEnemyMoving : LoadMonoBehaviour
{
    [SerializeField] protected bool isRuning;
    [SerializeField] protected bool stateRuningCurrent;
    [SerializeField] protected float detectionRange;
    [SerializeField] protected float speedMovement;
    [SerializeField] protected Transform enemyRoot;
    [SerializeField] protected Transform target;
    [SerializeField] protected Rigidbody rb;
    [SerializeField] protected MeleeEnemyController meleeEnemyController;
    protected override void LoadComponent()
    {
        base.LoadComponent();
        this.LoadRigidbody();
        this.LoadEnemyController();
        this.SetSpeedMovement(this.meleeEnemyController.EntitySO.baseSpeed);
        this.SetEnemyRoot(this.transform.parent);
        this.SetTarget(this.meleeEnemyController.Target);
        this.SetDetectionRange(10f);
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
    protected virtual void LoadEnemyController()
    {
        if (this.meleeEnemyController != null) return;
        this.meleeEnemyController = GetComponentInParent<MeleeEnemyController>();
        Debug.LogWarning(transform.name + " : LoadEnemyController");
    }
    public virtual void SetSpeedMovement(float speedMovement)
    {
        this.speedMovement= speedMovement;
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
    protected virtual void Moving()
    {
        if (!this.isRuning) return;
        Vector3 direction = this.target.position - this.enemyRoot.position;
        direction.y = 0f;
        this.enemyRoot.rotation = Quaternion.LookRotation(direction);
        Vector3 newPosition = Vector3.forward * this.speedMovement * Time.deltaTime;
        bool isMoving = newPosition != Vector3.zero;
        if (isMoving != this.stateRuningCurrent)
        {
            this.stateRuningCurrent = isMoving;
            this.meleeEnemyController.Animator.SetBool("IsRunning", isMoving);
        }
        this.enemyRoot.Translate(newPosition);
    }
    protected virtual bool IsDetectionTarget()
    {
        if(Vector3.Distance(this.target.position,this.enemyRoot.position)>this.detectionRange) return false;
        return true;
    }
}
