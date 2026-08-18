using UnityEngine;

public class MeleeEnemyAttack : LoadMonoBehaviour
{
    [SerializeField] protected float attackRange;
    [SerializeField] protected bool isAttack;
    [SerializeField] protected bool hasTriggeredAttack = false;
    [SerializeField] protected Transform target;
    [SerializeField] protected Transform enemyRoot;
    [SerializeField] protected MeleeEnemyController meleeEnemycontroller;
    [SerializeField] protected float attackTriggerTimer = 0f;
    protected override void LoadComponent()
    {
        base.LoadComponent();
        this.LoadMeleeEnemyController();
        this.SetTarget(this.meleeEnemycontroller.Target);
        this.SetEnemyRoot(this.meleeEnemycontroller.transform);
        this.SetAttackRange(this.meleeEnemycontroller.EntitySO.attackRange);
    }
    protected virtual void OnEnable()
    {
        this.isAttack = false;
        this.hasTriggeredAttack = false;
        this.attackTriggerTimer = 0f;
    }
    protected virtual void LoadMeleeEnemyController()
    {
        if (this.meleeEnemycontroller != null) return;
        this.meleeEnemycontroller = GetComponentInParent<MeleeEnemyController>();
        Debug.LogWarning(transform.name + " : LoadMeleeEnemyController");
    }
    protected virtual void SetAttackRange(float attackRange)
    {
        this.attackRange = attackRange;
    }
    protected virtual void SetTarget(Transform target)
    {
        if (target == null) return;
        this.target = target;
    }
    protected virtual void SetEnemyRoot(Transform enemyRoot)
    {
        if (enemyRoot == null) return;
        this.enemyRoot= enemyRoot;
    }
    public virtual void SetIsAttack(bool isAttack)
    {
        this.isAttack = isAttack;
    }
    public virtual bool GetIsAttack()
    {
        return this.isAttack;
    }
    private void Update()
    {
        this.CheckSafetyReset();
        this.ExecuteMeleeAttack();
    }
    protected virtual void CheckSafetyReset()
    {
        if (this.hasTriggeredAttack && !this.isAttack)
        {
            this.attackTriggerTimer += Time.deltaTime;
            if (this.attackTriggerTimer > 1.5f)
            {
                this.hasTriggeredAttack = false;
                this.attackTriggerTimer = 0f;
            }
        }
        else
        {
            this.attackTriggerTimer = 0f;
        }
    }
    protected virtual bool IsReachedDistance()
    {
        if (this.target == null || this.enemyRoot == null) return false;
        if (Vector3.Distance(this.target.position, this.enemyRoot.position) > this.attackRange) return false;
        return true;
    }

    public virtual void SetHasTriggeredAttack(bool hasTriggeredAttack)
    {
        this.hasTriggeredAttack = hasTriggeredAttack;
    }

    public virtual bool GetHasTriggeredAttack()
    {
        return this.hasTriggeredAttack;
    }

    protected virtual void ExecuteMeleeAttack()
    {
        if (!this.IsReachedDistance()) return;
        Vector3 directionToTarget = this.target.position - this.enemyRoot.position;
        directionToTarget.y = 0f;
        if (directionToTarget == Vector3.zero) return;
        directionToTarget.Normalize();

        Quaternion targetRot = Quaternion.LookRotation(directionToTarget);
        this.enemyRoot.rotation = Quaternion.Slerp(this.enemyRoot.rotation, targetRot, 15f * Time.deltaTime);

        float angle = Vector3.Angle(this.enemyRoot.forward, directionToTarget);
        if (angle > 45f) return;
        if (this.isAttack) return;
        if (this.hasTriggeredAttack) return;

        this.SetHasTriggeredAttack(true);
        this.meleeEnemycontroller.Animator.SetTrigger("Attack");
        Debug.Log("2");
    }
}
