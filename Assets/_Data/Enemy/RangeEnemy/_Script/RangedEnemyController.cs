using UnityEngine;

public class RangedEnemyController : BaseEntityController
{
    [SerializeField] protected Transform pointer;
    [SerializeField] protected RangedEnemyAttack rangedEnemyAttack;
    [SerializeField] protected RangedEnemyMoving rangedEnemyMoving;
    public Transform Pointer => pointer;
    public RangedEnemyAttack RangedEnemyAttack => rangedEnemyAttack;
    public RangedEnemyMoving RangedEnemyMoving => rangedEnemyMoving;
    protected override void LoadComponent()
    {
        base.LoadComponent();
        this.LoadPointer();
        this.LoadRangedEnemyAttack();
        this.LoadRangedEnemyMoving();
    }
    protected virtual void LoadPointer()
    {
        if (this.pointer != null) return;
        this.pointer = GameObject.FindGameObjectsWithTag("Pointer")?[0].transform;
        Debug.LogWarning(transform.name + " : LoadPointer");
    }
    protected virtual void LoadRangedEnemyMoving()
    {
        if (this.rangedEnemyMoving != null) return;
        this.rangedEnemyMoving = GetComponentInChildren<RangedEnemyMoving>();
        Debug.LogWarning(transform.name + " : LoadRangedEnemyMoving");
    }
    protected virtual void LoadRangedEnemyAttack()
    {
        if (this.rangedEnemyAttack != null) return;
        this.rangedEnemyAttack = GetComponentInChildren<RangedEnemyAttack>();
        Debug.LogWarning(transform.name + " : LoadRangedEnemyAttack");
    }
}
