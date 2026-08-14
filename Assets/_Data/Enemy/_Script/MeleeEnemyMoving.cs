using Unity.Jobs;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.Rendering;

public class MeleeEnemyMoving : LoadMonoBehaviour
{
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
    private void FixedUpdate()
    {
        this.Moving();
    }
    protected virtual void Moving()
    {
        Vector3 direction = (this.target.position - this.enemyRoot.position).normalized;
        this.enemyRoot.rotation = Quaternion.LookRotation(direction);
        enemyRoot.Translate(Vector3.forward * this.speedMovement * Time.deltaTime);
    }
}
