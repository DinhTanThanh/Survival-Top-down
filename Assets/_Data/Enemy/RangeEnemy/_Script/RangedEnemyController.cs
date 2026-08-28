using UnityEngine;
using UnityEngine.EventSystems;

public class RangedEnemyController : BaseEntityController
{
    [SerializeField] protected Transform pointer;
    [SerializeField] protected RangedEnemyAttack rangedEnemyAttack;
    [SerializeField] protected RangedEnemyMoving rangedEnemyMoving;
    [SerializeField] protected PlayerController playercontroller;
    [SerializeField] protected StateManager stateManager;
    protected RangedEnemyIdleState idleState;
    protected RangedEnemyMoveState moveState;
    protected RangedEnemyAttackState attackState;
    protected RangedEnemyDeadState deadState;
    public Transform Pointer => pointer;
    public RangedEnemyAttack RangedEnemyAttack => rangedEnemyAttack;
    public RangedEnemyMoving RangedEnemyMoving => rangedEnemyMoving;
    public PlayerController PlayerController => playercontroller;
    public StateManager StateManager => stateManager;
    public RangedEnemyIdleState IdleState => idleState;
    public RangedEnemyMoveState MoveState => moveState;
    public RangedEnemyAttackState AttackState=> attackState;
    public RangedEnemyDeadState DeadState => deadState;
    private void OnEnable()
    {
        if (this.stateManager != null &&this.idleState!=null)
        {
            this.stateManager.ChangeState(this.idleState);
        }
    }
    private void Start()
    {
        this.InitStateManager();
    }
    protected void InitStateManager()
    {
        this.idleState = new RangedEnemyIdleState(this);
        this.moveState=new RangedEnemyMoveState(this);
        this.attackState=new RangedEnemyAttackState(this);
        this.deadState=new RangedEnemyDeadState(this);
        if (this.stateManager != null)
        {
            this.stateManager.ChangeState(this.idleState);
        }
    }
    protected override void LoadComponent()
    {
        base.LoadComponent();
        this.LoadPointer();
        this.LoadRangedEnemyAttack();
        this.LoadRangedEnemyMoving();
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
