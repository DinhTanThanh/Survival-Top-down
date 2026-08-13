using UnityEngine;

public class PlayerController : LoadMonoBehaviour
{
    [SerializeField] protected GameObject bullet;
    [SerializeField] protected Transform firePoint;
    [SerializeField] protected EntitySO entitySO;
    [SerializeField] protected ButtonAttack buttonAttack;
    public GameObject Bullet => bullet;
    public Transform FirePoint => firePoint;
    public EntitySO EntitySO => entitySO;
    public ButtonAttack ButtonAttack => buttonAttack;
    protected override void LoadComponent()
    {
        base.LoadComponent();
        this.LoadEntitySO();
        this.LoadFirePoint();
        this.LoadBullet();
        this.LoadButtonAttack();
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
    protected virtual void LoadEntitySO()
    {
        if (this.entitySO != null) return;
        this.entitySO = Resources.Load<EntitySO>("Entity/" + transform.name + "SO");
        Debug.LogWarning(transform.name + " : LoadEntitySO");
    }
}
