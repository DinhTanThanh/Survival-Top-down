using UnityEngine;

public class PlayerController : BaseEntityController
{
    [SerializeField] protected GameObject bullet;
    [SerializeField] protected Transform firePoint;
    [SerializeField] protected ShotData shotData;
    [SerializeField] protected LevelGrowthData levelGrowthData;
    [SerializeField] protected ButtonAttack buttonAttack;
    [SerializeField] protected PlayerPoisonHandler playerPoisonHandler;
    [SerializeField] protected PlayerLevel playerLevel;
    [SerializeField] protected PlayerShooting playerShooting;
    public GameObject Bullet => bullet;
    public Transform FirePoint => firePoint;
    public ShotData ShotData => shotData;
    public LevelGrowthData LevelGrowthData => levelGrowthData;
    public ButtonAttack ButtonAttack => buttonAttack;
    public PlayerPoisonHandler PlayerPoisonHandler => playerPoisonHandler;
    public PlayerLevel PlayerLevel=> playerLevel;
    public PlayerShooting PlayerShooting => playerShooting;
    protected override void LoadComponent()
    {
        base.LoadComponent();
        this.LoadShotData();
        this.LoadLevelSO();
        this.LoadFirePoint();
        this.LoadBullet();
        this.LoadButtonAttack();
        this.LoadPlayerPoisonHandler();
        this.LoadPlayerLevel();
        this.LoadPlayerShooting();
    }
    protected virtual void LoadLevelSO()
    {
        if (this.levelGrowthData != null) return;
        this.levelGrowthData = Resources.Load<LevelGrowthData>("Level/" + transform.name + "LevelGrowthData");
        Debug.LogWarning(transform.name + " : LoadLevelSO");
    }
    protected virtual void LoadPlayerLevel()
    {
        if (this.playerLevel != null) return;
        this.playerLevel=GetComponentInChildren<PlayerLevel>();
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
        this.buttonAttack=FindFirstObjectByType<ButtonAttack>();
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
        this.playerShooting=GetComponentInChildren<PlayerShooting>();
        Debug.LogWarning(transform.name + " : LoadPlayerShooting");
    }
}
