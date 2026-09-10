using UnityEngine;

public class LargeEnemyController : BaseBossController
{
    [SerializeField] protected bool enableAutoActions = true;     
    [SerializeField] protected float attackDuration = 30f;         
    [SerializeField] protected float recallDuration = 12f;       
    [SerializeField] protected float healInterval = 8f;         
    [SerializeField] protected float healAmount = 25f;
    [SerializeField] protected bool isAttacking = true;
    [SerializeField] protected bool isDirectAttack = false;
    [SerializeField] protected float combatTimer = 0f;
    [SerializeField] protected float healTimer = 0f;
    [SerializeField] protected BossMinionSpawner bossMinionSpawner;
    [SerializeField] protected StateManager stateManager;
    [SerializeField] protected LargeMoving largeMoving;
    [SerializeField] protected LargeAttack largeAttack;
    protected LargeEnemyIdleState idleState;
    protected LargeEnemyMovingState moveState;
    protected LargeEnemyAttackState attackState;
    protected LargeEnemyGetHitState gethitState;
    protected LargeEnemyDeadState deadState;
    public StateManager StateManager => stateManager;
    public LargeEnemyIdleState IdleState => idleState;
    public LargeEnemyMovingState MoveState => moveState;
    public LargeEnemyAttackState AttackState => attackState;
    public LargeEnemyGetHitState GetHitState => gethitState;
    public LargeEnemyDeadState DeadState => deadState;
    public LargeMoving LargeMoving=>largeMoving;
    public LargeAttack LargeAttack=> largeAttack;   
    public bool IsAttacking => isAttacking;
    public bool IsDirectAttack => isDirectAttack;

    public virtual bool CanDirectAttack()
    {
        if (this.isDirectAttack) return true;
        if (this.bossMinionSpawner == null) return true;
        return false;
    }
    private void OnEnable()
    {
        this.isDirectAttack = false;
        this.isAttacking = true;
        this.combatTimer = 0f;
        this.healTimer = 0f;
        if (this.damageReceiver == null)
        {
            this.LoadDamageReceiver();
        }
        if (this.damageReceiver != null)
        {
            this.damageReceiver.Reborn();
        }
        if (this.stateManager != null && this.idleState != null)
        {
            this.stateManager.ChangeState(this.idleState);  
        }
    }
    protected virtual void InitState()
    {
        this.idleState = new LargeEnemyIdleState(this);
        this.moveState=new LargeEnemyMovingState(this);
        this.attackState = new LargeEnemyAttackState(this);
        this.gethitState=new LargeEnemyGetHitState(this);
        this.deadState=new LargeEnemyDeadState(this);
        if (this.stateManager != null)
        {
            this.stateManager.ChangeState(this.idleState);
        }
    }
    protected override void LoadComponent()
    {
        base.LoadComponent();
        this.LoadBossMinionSpawner();
        this.LoadStateManager();
        this.LoadLargeMoving();
        this.LoadLargeAttack();
    }
    
    private void Start()
    {
        if (this.enableAutoActions)
        {
            this.StartAttackPhase();
        }
        InitState();
    }
    private void Update()
    {
        this.bossMinionSpawner.CleanDeadMinions();
        this.bossMinionSpawner.HandleAutoRespawn();
        if (this.enableAutoActions)
        {
            this.HandleCombatCycleByTime();
            this.HandleHealByTime();
        }
    }
    public virtual void SwitchToDirectAttack()
    {
        if (this.isDirectAttack) return;
        this.isDirectAttack = true;

        if (this.largeMoving != null)
        {
            this.largeMoving.SetMaximumDistance(1.5f);
            this.largeMoving.SetMinximumDistance(0.5f);
        }
    }
    public virtual void StartAttackPhase()
    {
        this.isAttacking = true;
        this.combatTimer = 0f;
        CommandMinionsAttack();
    }
    public virtual void StartRecallPhase()
    {
        this.isAttacking = false;
        this.combatTimer = 0f;
        this.CommandMinionsRecall();
    }
    protected virtual void HandleCombatCycleByTime()
    {
        if (this.bossMinionSpawner.ActiveMinions.Count == 0) return;
        this.combatTimer += Time.deltaTime;
        if (this.isAttacking)
        {
            if (this.combatTimer >= this.attackDuration)
            {
                this.StartRecallPhase();
            }
        }
        else
        {
            if (this.combatTimer >= this.recallDuration)
            {
                this.StartAttackPhase();
            }
        }
    }
    protected virtual void HandleHealByTime()
    {
        if (this.bossMinionSpawner.ActiveMinions.Count == 0) return;
        this.healTimer += Time.deltaTime;
        if (this.healTimer >= this.healInterval)
        {
            this.healTimer = 0f;
            this.CommandMinionsHeal(this.healAmount);
        }
    }
    protected virtual void LoadBossMinionSpawner()
    {
        if (this.bossMinionSpawner != null) return;
        this.bossMinionSpawner = GetComponentInChildren<BossMinionSpawner>();
        Debug.LogWarning(transform.name + " : LOadBossMinionSpawner");
    }
    protected virtual void LoadStateManager()
    {
        if (this.stateManager != null) return;
        this.stateManager=GetComponent<StateManager>();
        Debug.LogWarning(transform.name + " : LoadStateManager");
    }
    protected virtual void LoadLargeMoving()
    {
        if (this.largeMoving != null) return;
        this.largeMoving = GetComponentInChildren<LargeMoving>();
        Debug.LogWarning(transform.name + " : LoadLargeMoving");
    }
    protected virtual void LoadLargeAttack()
    {
        if (this.largeAttack != null) return;
        this.largeAttack = GetComponentInChildren<LargeAttack>();
        Debug.LogWarning(transform.name + " : LoadLargeAttack");
    }
}
