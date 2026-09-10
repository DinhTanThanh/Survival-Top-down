using UnityEngine;

public class LargeAttack : StrategyContext
{
    [SerializeField] protected float attackRange;
    [SerializeField] protected bool isAttack;
    [SerializeField] protected Transform target;
    [SerializeField] protected Transform enemyRoot;
    [SerializeField] protected LargeEnemyController largeEnemyController;

    protected SwingAttackStrategy swingStrategy;
    protected JumpSlamAttackStrategy jumpSlamStrategy;
    protected StompAttackStrategy stompStrategy;

    public LargeEnemyController LargeEnemyController => largeEnemyController;
    public Transform Target => target;
    public Transform EnemyRoot => enemyRoot;

    protected override void LoadComponent()
    {
        base.LoadComponent();
        this.LoadLargeEnemyController();
        this.SetTarget(this.largeEnemyController.Target);
        this.SetEnemyRoot(this.largeEnemyController.transform);
        this.SetAttackRange(this.largeEnemyController.EntitySO.attackRange);
        this.InitStrategies();
    }

    protected virtual void Start()
    {
        this.InitStrategies();
    }

    public virtual void InitStrategies()
    {
        if (this.largeEnemyController == null)
        {
            this.LoadLargeEnemyController();
        }
        if (this.largeEnemyController != null)
        {
            if (this.swingStrategy == null)
                this.swingStrategy = new SwingAttackStrategy(this.largeEnemyController, this);
            if (this.jumpSlamStrategy == null)
                this.jumpSlamStrategy = new JumpSlamAttackStrategy(this.largeEnemyController, this);
            if (this.stompStrategy == null)
                this.stompStrategy = new StompAttackStrategy(this.largeEnemyController, this);
        }
    }

    protected virtual void LoadLargeEnemyController()
    {
        if (this.largeEnemyController != null) return;
        this.largeEnemyController = GetComponentInParent<LargeEnemyController>();
        Debug.LogWarning(transform.name + " : LoadLargeEnemyController");
    }

    public virtual void SetIsAttack(bool isAttack)
    {
        this.isAttack = isAttack;
    }
    public virtual bool GetIsAttack()
    {
        return this.isAttack;
    }
    protected virtual void SetAttackRange(float attackRange)
    {
        this.attackRange = attackRange;
    }

    public virtual void SetTarget(Transform target)
    {
        if (target == null) return;
        this.target = target;
    }

    public virtual void SetEnemyRoot(Transform enemyRoot)
    {
        if (enemyRoot == null) return;
        this.enemyRoot = enemyRoot;
    }
    public virtual bool IsReachedDistance()
    {
        if (this.target == null || this.enemyRoot == null) return false;
        float distance = Vector3.Distance(this.target.position, this.enemyRoot.position);
        if (distance <= this.attackRange) return true;
        if (this.jumpSlamStrategy != null && this.jumpSlamStrategy.CanExecute(distance))
        {
            return true;
        }

        return false;
    }

    public virtual BaseBossAttackStrategy SelectBestStrategy()
    {
        if (this.target == null || this.enemyRoot == null) return this.swingStrategy;
        float distance = Vector3.Distance(this.target.position, this.enemyRoot.position);
        if (distance >= 3.2f && this.jumpSlamStrategy != null && this.jumpSlamStrategy.CanExecute(distance))
        {
            return this.jumpSlamStrategy;
        }
        if (this.stompStrategy != null && this.stompStrategy.CanExecute(distance))
        {
            return this.stompStrategy;
        }
        if (this.swingStrategy != null && this.swingStrategy.CanExecute(distance))
        {
            return this.swingStrategy;
        }
        return this.swingStrategy;
    }

    public virtual float ExecuteAttack()
    {
        this.InitStrategies();
        BaseBossAttackStrategy chosenStrategy = this.SelectBestStrategy();
        if (chosenStrategy != null)
        {
            this.SetStrategy(chosenStrategy);
            this.ExecuteStrategy();
            return chosenStrategy.Duration;
        }
        return 1.0f;
    }
}
