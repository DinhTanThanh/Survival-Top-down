using UnityEngine;

public class MeleeEnemyController : BaseEntityController
{
    [SerializeField] protected Rigidbody rb;
    [SerializeField] protected MeleeEnemyAttack meleeEnemyAttack;
    [SerializeField] protected MeleeEnemyMoving meleeEnemyMoving;
    public Rigidbody Rb => rb;
    public MeleeEnemyAttack MeleeEnemyAttack => meleeEnemyAttack;
    public MeleeEnemyMoving MeleeEnemyMoving => meleeEnemyMoving;
    protected override void LoadComponent()
    {
        base.LoadComponent();
        this.LoadRigidbody();
        this.LoadMeleeEnemyAttack();
        this.LoadMeleeEnemyMoving();
    }
    protected virtual void LoadRigidbody()
    {
        if (this.rb != null) return;
        this.rb = GetComponent<Rigidbody>();
        Debug.LogWarning(transform.name + " : LoadRigidbody");
    }
    protected virtual void LoadMeleeEnemyMoving()
    {
        if (this.meleeEnemyMoving != null) return;
        this.meleeEnemyMoving = GetComponentInChildren<MeleeEnemyMoving>();
        Debug.LogWarning(transform.name + " : LoadMeleeEnemyMoving");
    }
    protected virtual void LoadMeleeEnemyAttack()
    {
        if (this.meleeEnemyAttack != null) return;
        this.meleeEnemyAttack = GetComponentInChildren<MeleeEnemyAttack>();
        Debug.LogWarning(transform.name + " : LoadMeleeEnemyAttack");
    }
    
}
