using UnityEngine;

public class MinionController : BaseEntityController
{
    [SerializeField] protected Rigidbody rb;
    [SerializeField] protected StateManager stateManager;
    protected MinionIdleState idleState;
    protected MinionAttackState attackState;
    protected MinionMoveState moveState;
    public Rigidbody Rb => rb;
    public StateManager StateManager=> stateManager;
    public MinionIdleState IdleState=> idleState;
    public MinionAttackState AttackState=> attackState;
    public MinionMoveState MoveState=> moveState;
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
        if (this.stateManager != null)
        {
            this.stateManager.ChangeState(idleState);
        }
    }
    protected override void LoadComponent()
    {
        base.LoadComponent();
        this.LoadRigidbody();
    }
    protected virtual void LoadRigidbody()
    {
        if (this.rb != null) return;
        this.rb = GetComponent<Rigidbody>();
        Debug.LogWarning(transform.name + " : LoadRigidbody");
    }
}
