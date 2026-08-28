using UnityEngine;
public class MeleeEnemyController : BaseEntityController
{
    [SerializeField] protected Rigidbody rb;
    [SerializeField] protected MeleeEnemyAttack meleeEnemyAttack;
    [SerializeField] protected MeleeEnemyMoving meleeEnemyMoving;
    [SerializeField] protected PlayerController playercontroller;
    [SerializeField] protected StateManager stateManager;
    protected MeleeEnemyIdleState idleState;
    protected MeleeEnemyMoveState moveState;
    protected MeleeEnemyAttackState attackState;
    protected MeleeEnemyDeadState deadState;
    public Rigidbody Rb => rb;
    public MeleeEnemyAttack MeleeEnemyAttack => meleeEnemyAttack;
    public MeleeEnemyMoving MeleeEnemyMoving => meleeEnemyMoving;
    public PlayerController PlayerController => playercontroller;
    public StateManager StateManager => stateManager;
    public MeleeEnemyIdleState IdleState=> idleState;
    public MeleeEnemyMoveState MoveState => moveState;
    public MeleeEnemyAttackState AttackState => attackState;
    public MeleeEnemyDeadState DeadState => deadState;
    private void OnEnable()
    {
        if (this.stateManager != null && this.idleState != null)
        {
            this.stateManager.ChangeState(this.idleState);
        }
    }
    private void Start()
    {
        this.InitState();
    }
    protected void InitState()
    {
        this.idleState = new MeleeEnemyIdleState(this);
        this.moveState = new MeleeEnemyMoveState(this);
        this.attackState = new MeleeEnemyAttackState(this);
        this.deadState = new MeleeEnemyDeadState(this);
        if (this.stateManager != null)
        {
            this.stateManager.ChangeState(this.idleState);
        }
    }
    protected override void LoadComponent()
    {
        base.LoadComponent();
        this.LoadRigidbody();
        this.LoadMeleeEnemyAttack();
        this.LoadMeleeEnemyMoving();
        this.LoadPlayerController();
        this.LoadStateManager();
    }
    protected virtual void LoadStateManager()
    {
        if (this.stateManager != null) return;
        this.stateManager = GetComponent<StateManager>();
        Debug.LogWarning(transform.name + " : LoadStateManager");
    }
    protected virtual void LoadPlayerController()
    {
        if (this.playercontroller != null) return;
        this.playercontroller = FindFirstObjectByType<PlayerController>();
        Debug.LogWarning(transform.name + " : LoadPlayerController");
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
