using UnityEngine;
public class MinionAttack : LoadMonoBehaviour
{
    [SerializeField] protected float timer;
    [SerializeField] protected float timeDelay;
    [SerializeField] protected float attackRange;
    [SerializeField] protected bool isAttack;
    [SerializeField] protected MinionController controller;
    protected Transform targetPosition;
    public Transform TargetPosition => targetPosition;
    protected override void LoadComponent()
    {
        base.LoadComponent();
        this.LoadMinionController();
        this.SetTimeDelay(1.5f);
        if (this.controller != null && this.controller.EntitySO != null)
        {
            this.SetAttackRange(this.controller.EntitySO.attackRange);
        }
    }
    private void Update()
    {
        if (this.isAttack) return;
        this.Timing();
    }
    public virtual bool IsReachedAttackRange()
    {
        if (this.targetPosition == null) return false;
        if (Vector3.Distance(this.targetPosition.position, this.transform.position) > this.attackRange) return false;
        return true;
    }
    public virtual bool IsReachedAngletAttack()
    {
        if (this.targetPosition == null) return false;
        Vector3 directionToPlayer = this.targetPosition.position - this.transform.position;
        float angleAttack = Vector3.Angle(this.transform.forward, directionToPlayer);
        return angleAttack <=45;
    }
    protected virtual void Timing()
    {
        this.timer += Time.deltaTime;
        if (this.timer < this.timeDelay) return;
        this.timer = 0f;
        this.isAttack = true;
    }
    protected virtual void LoadMinionController()
    {
        if (this.controller != null) return;
        this.controller = GetComponent<MinionController>();
        Debug.LogWarning(transform.name + " : LoadMinionController");
    }
    public virtual void SetTarget(Transform target)
    {
        this.targetPosition = target;
    }
    protected virtual void SetAttackRange(float attackRange)
    {
        this.attackRange = attackRange;
    }
    protected virtual void SetTimeDelay(float timeDelay)
    {
        this.timeDelay = timeDelay;
    }
    public virtual void SetCanAttack(bool canAttack)
    {
        if (!canAttack)
        {
            this.isAttack = false;
        }
    }
    public virtual void SetIsAttack(bool isAttack)
    {
        this.isAttack=isAttack;
    }
    public virtual bool GetIsAttack()
    {
        return this.isAttack;
    }
}
