using UnityEngine;
public class MinionController : BaseEntityController
{
    [SerializeField] protected Transform owner;
    [SerializeField] protected Rigidbody rb;
    [SerializeField] protected StateManager stateManager;
    [SerializeField] protected MinionnMoving minionMoving;
    [SerializeField] protected MinionAttack minionAttack;
    [SerializeField] protected MinionCommand currentCommand;
    protected MinionIdleState idleState;
    protected MinionAttackState attackState;
    protected MinionMoveState moveState;
    protected MinionGetHitState gethitState;
    protected MinionDeadState deadState;
    public Rigidbody Rb => rb;
    public StateManager StateManager=> stateManager;
    public MinionnMoving MinionnMoving => minionMoving;
    public MinionAttack MinionAttack => minionAttack;
    public MinionIdleState IdleState=> idleState;
    public MinionAttackState AttackState=> attackState;
    public MinionMoveState MoveState=> moveState;
    public MinionGetHitState GetHitState => gethitState;
    public MinionDeadState DeadState => deadState;
    private void Start()
    {
        this.InitState();
    }
    private void OnEnable()
    {
        if(this.stateManager!=null && this.idleState != null)
        {
            this.stateManager.ChangeState(this.idleState);
        }
    }
    protected virtual void InitState()
    {
        this.idleState = new MinionIdleState(this);
        this.attackState=new MinionAttackState(this);
        this.moveState = new MinionMoveState(this);
        this.deadState = new MinionDeadState(this);
        this.gethitState=new MinionGetHitState(this);
        if (this.stateManager != null)
        {
            this.stateManager.ChangeState(idleState);
        }
    }
    protected override void LoadComponent()
    {
        base.LoadComponent();
        this.LoadRigidbody();
        this.LoadStateManager();
        this.LoadMinionMoving();
        this.LoadMinionAttack();
    }
    public virtual void ReceiveCommand(MinionCommand command)
    {
        this.currentCommand = command;
        if (command == null) return;
        if (command.commandType == MinionCommandType.AttackTarget)
        {
            if (this.minionMoving != null)
            {
                this.minionMoving.SetTarget(command.target);
                this.minionMoving.SetTargetOffset(command.formationOffset);
            }
            if (this.minionAttack != null) this.minionAttack.SetTarget(command.target);
        }
        else if (command.commandType == MinionCommandType.GuardBoss)
        {
            if (this.minionMoving != null)
            {
                this.minionMoving.SetTarget(this.owner);
                this.minionMoving.SetTargetOffset(command.formationOffset);
            }
            if (this.minionAttack != null) this.minionAttack.SetTarget(null);
        }
    }

    protected virtual void LoadRigidbody()
    {
        if (this.rb != null) return;
        this.rb = GetComponent<Rigidbody>();
        Debug.LogWarning(transform.name + " : LoadRigidbody");
    }
    protected virtual void LoadStateManager()
    {
        if (this.stateManager != null) return;
        this.stateManager=GetComponent<StateManager>();
        Debug.LogWarning(transform.name + " : LoadStateManager");
    }
    protected virtual void LoadMinionMoving()
    {
        if (this.minionMoving != null) return;
        this.minionMoving=GetComponentInChildren<MinionnMoving>();
        Debug.LogWarning(transform.name + " : LoadMinionMoving");
    }
    protected virtual void LoadMinionAttack()
    {
        if (this.MinionAttack != null) return;
        this.minionAttack = GetComponent<MinionAttack>();
        Debug.LogWarning(transform.name + " : LoadMinionAttack");
    }
    public virtual void SetBossOwner(Transform owner)
    {
        this.owner= owner;
    }
    public virtual void GoBackListMinionDead()
    {
        this.gameObject.SetActive(false);
        if (SpawnEnemyPoolingManager.Instance != null)
        {
            SpawnEnemyPoolingManager.Instance.GoBackList(this.gameObject);
        }
    }
}
