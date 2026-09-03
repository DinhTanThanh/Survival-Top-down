using UnityEngine;

[RequireComponent(typeof(StateManager))]
public class PlayerController : BaseEntityController
{
    [SerializeField] protected Transform firePoint;
    [SerializeField] protected GameObject bullet;
    [SerializeField] protected Indicator indicator;
    [SerializeField] protected StateManager playerStateManager;
    [SerializeField] protected ShotData shotData;
    [SerializeField] protected LevelGrowthData levelGrowthData;
    [SerializeField] protected ButtonAttack buttonAttack;
    [SerializeField] protected PlayerPoisonHandler playerPoisonHandler;
    [SerializeField] protected PlayerLevel playerLevel;
    [SerializeField] protected PlayerShooting playerShooting;
    [SerializeField] protected PlayerMoving playerMoving;
    [SerializeField] protected ButtonDashExplosionSkill dashExplosionSkill;
    protected PlayerIdleState idleState;
    protected PlayerMoveState moveState;
    protected PlayerShotState shotState;

    public Transform FirePoint => firePoint;
    public GameObject Bullet => bullet;
    public Indicator Indicator => indicator;
    public StateManager PlayerStateManager => playerStateManager;
    public ShotData ShotData => shotData;
    public LevelGrowthData LevelGrowthData => levelGrowthData;
    public ButtonAttack ButtonAttack => buttonAttack;
    public PlayerPoisonHandler PlayerPoisonHandler => playerPoisonHandler;
    public PlayerLevel PlayerLevel => playerLevel;
    public PlayerShooting PlayerShooting => playerShooting;
    public PlayerMoving PlayerMoving => playerMoving;
    public PlayerIdleState IdleState => idleState;
    public PlayerMoveState MoveState => moveState;
    public PlayerShotState ShotState => shotState;
    public ButtonDashExplosionSkill DashExplosionSkill => dashExplosionSkill;
    protected virtual void Start()
    {
        this.InitStates();
    }

    protected virtual void InitStates()
    {
        this.idleState = new PlayerIdleState(this);
        this.moveState = new PlayerMoveState(this);
        this.shotState = new PlayerShotState(this);

        if (this.playerStateManager != null)
        {
            this.playerStateManager.ChangeState(this.idleState);
        }
    }

    protected override void LoadComponent()
    {
        base.LoadComponent();
        this.LoadPlayerStateManager();
        this.LoadShotData();
        this.LoadLevelSO();
        this.LoadFirePoint();
        this.LoadBullet();
        this.LoadButtonAttack();
        this.LoadPlayerPoisonHandler();
        this.LoadPlayerLevel();
        this.LoadPlayerShooting();
        this.LoadPlayerMoving();
        this.LoadDashExplosionSkill();
        this.LoadIndicator();
    }
    protected virtual void LoadLevelSO()
    {
        if (this.levelGrowthData != null) return;
        this.levelGrowthData = Resources.Load<LevelGrowthData>("Level/" + transform.name + "LevelGrowthData");
        Debug.LogWarning(transform.name + " : LoadLevelSO");
    }
    protected virtual void LoadPlayerMoving()
    {
        if (this.playerMoving != null) return;
        this.playerMoving=GetComponentInChildren<PlayerMoving>();
        Debug.LogWarning(transform.name + " : LoadPlayerMoving");
    }
    protected virtual void LoadPlayerStateManager()
    {
        if (this.playerStateManager != null) return;
        this.playerStateManager = GetComponent<StateManager>();
        Debug.LogWarning(transform.name + "LoadPlayerStateManager");
    }
    protected virtual void LoadPlayerLevel()
    {
        if (this.playerLevel != null) return;
        this.playerLevel = GetComponentInChildren<PlayerLevel>();
        Debug.LogWarning(transform.name + " : LoadPlayerLevel");
    }
    protected virtual void LoadShotData()
    {
        if (this.shotData != null) return;
        this.shotData = Resources.Load<ShotData>("WeaponData/TripleShot");
        Debug.LogWarning(transform.name + " : LoadShotData");
    }
    protected virtual void LoadPlayerPoisonHandler()
    {
        if (this.playerPoisonHandler != null) return;
        this.playerPoisonHandler = GetComponentInChildren<PlayerPoisonHandler>();
        Debug.LogWarning(transform.name + " : LoadPlayerPoisonHandler");
    }
    protected virtual void LoadButtonAttack()
    {
        if (this.buttonAttack != null) return;
        this.buttonAttack = FindFirstObjectByType<ButtonAttack>();
        Debug.LogWarning(transform.name + " : LoadButtonAttack");
    }
    protected virtual void LoadBullet()
    {
        if (this.bullet != null) return;
        this.bullet = GameObject.Find("Bullet");
        Debug.LogWarning(transform.name + " : LoadBullet");
    }
    protected virtual void LoadFirePoint()
    {
        if (this.firePoint != null) return;
        this.firePoint = GameObject.Find("FirePointShoot").transform;
        Debug.LogWarning(transform.name + " : LoadFirePoint");
    }
    protected virtual void LoadPlayerShooting()
    {
        if (this.playerShooting != null) return;
        this.playerShooting = GetComponentInChildren<PlayerShooting>();
        Debug.LogWarning(transform.name + " : LoadPlayerShooting");
    }
    protected virtual void LoadDashExplosionSkill()
    {
        if (this.dashExplosionSkill != null) return;
        this.dashExplosionSkill=FindFirstObjectByType<ButtonDashExplosionSkill>();
        Debug.LogWarning(transform.name + " : LoadDashExplosionSkill");
    }
    protected virtual void LoadIndicator()
    {
        if(this.indicator != null) return;
        this.indicator = GetComponentInChildren<Indicator>();
        Debug.LogWarning(transform.name + " : LoadIndicator");
    }
}


