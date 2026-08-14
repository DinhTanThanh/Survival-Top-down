using UnityEngine;
using UnityEngine.Rendering;

public class MeleeEnemyController : BaseEntityController
{
    [SerializeField] protected Transform target;
    [SerializeField] protected Animator animator;
    [SerializeField] protected MeleeEnemyAttack meleeEnemyAttack;
    [SerializeField] protected MeleeEnemyMoving meleeEnemyMoving;
    public Transform Target => target;
    public Animator Animator => animator;
    public MeleeEnemyAttack MeleeEnemyAttack => meleeEnemyAttack;
    public MeleeEnemyMoving MeleeEnemyMoving => meleeEnemyMoving;
    protected override void LoadComponent()
    {
        base.LoadComponent();
        this.LoadTarget();
        this.LoadMeleeEnemyAttack();
        this.LoadMeleeEnemyMoving();
        this.LoadAnimator();
    }
    protected virtual void LoadAnimator()
    {
        if (this.animator != null) return;
        this.animator = GetComponentInChildren<Animator>();
        Debug.LogWarning(transform.name + " : LoadAnimator");
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
    protected virtual void LoadTarget()
    {
        if (this.target != null) return;
        this.target = GameObject.Find("Player")?.transform;
        Debug.LogWarning(transform.name + " : LoadTarget");
    }
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("va cham");
    }
}
