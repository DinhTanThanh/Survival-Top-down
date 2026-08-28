using UnityEngine;
public class MinionAttack : LoadMonoBehaviour
{
    [SerializeField] protected float timer;
    [SerializeField] protected float timeDelay;
    [SerializeField] protected float attackRange;
    [SerializeField] protected bool isAttack;
    [SerializeField] protected Transform targetPosition;
    [SerializeField] protected MinionController controller;
    protected override void LoadComponent()
    {
        base.LoadComponent();
        this.LoadMinionController();
        this.SetTimeDelay(1f);
        this.SetAttackRange(this.controller.EntitySO.attackRange);
    }
    private void Update()
    {
        if (this.isAttack) return;
        this.Timing();
    }
    protected virtual bool IsReachedAttackRange()
    {
        if (this.targetPosition == null) return false;
        if (Vector3.Distance(this.targetPosition.position, this.transform.position) > this.attackRange) return false;
        return true;
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
}
