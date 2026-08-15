using UnityEngine;

public class PlayerController : BaseEntityController
{
    [SerializeField] protected GameObject bullet;
    [SerializeField] protected Transform firePoint;
    [SerializeField] protected ButtonAttack buttonAttack;
    [SerializeField] protected PlayerPoisonHandler playerPoisonHandler;
    public GameObject Bullet => bullet;
    public Transform FirePoint => firePoint;
    public ButtonAttack ButtonAttack => buttonAttack;
    public PlayerPoisonHandler PlayerPoisonHandler => playerPoisonHandler;
    protected override void LoadComponent()
    {
        base.LoadComponent();
        this.LoadFirePoint();
        this.LoadBullet();
        this.LoadButtonAttack();
        this.LoadPlayerPoisonHandler();
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
        this.firePoint = transform.Find("FirePoint");
        Debug.LogWarning(transform.name + " : LoadFirePoint");
    }
    
}
