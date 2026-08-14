using UnityEngine;

public class RangedEnemyAttack : LoadMonoBehaviour
{
    [SerializeField] protected bool isAttack;
    [SerializeField] protected RangedEnemyController rangedEnemyController;
    public bool IsAttack => isAttack;
    protected override void LoadComponent()
    {
        base.LoadComponent();
        this.LoadRangedEnemyController();
    }
    protected virtual void LoadRangedEnemyController()
    {
        if (this.rangedEnemyController != null) return;
        this.rangedEnemyController = GetComponentInParent<RangedEnemyController>();
        Debug.LogWarning(transform.name + " : LoadRangedEnemyController");
    }
    public virtual void SetAttack(bool isAttack)
    {
        this.isAttack = isAttack;
    }
    private void Update()
    {
        if (Vector3.Distance(this.rangedEnemyController.Target.position, this.transform.parent.position) > 5f) return;
        if (this.isAttack) return;
        this.rangedEnemyController.Animator.SetTrigger("Attack");
        this.rangedEnemyController.RangedEnemyMoving.SetSpeedMovement(0f);
        this.isAttack = true;
    }
}
