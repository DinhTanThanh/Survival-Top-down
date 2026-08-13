using Unity.Mathematics;
using UnityEngine;

public class PlayerShooting : LoadMonoBehaviour
{
    [SerializeField] protected int bulletPerShot;
    [SerializeField] protected float timer;
    [SerializeField] protected float timeDelay;
    [SerializeField] protected bool canFire;
    [SerializeField] protected GameObject bulletPrefab;
    [SerializeField] protected Transform firePoint;
    [SerializeField] protected PlayerController playerController;
    [SerializeField] protected int[] arrayDirectionBullet;
    protected override void LoadComponent()
    {
        base.LoadComponent();
        this.LoadPlayerController();
        this.GetBulletPrefab();
        this.GetFirePoint();
        this.GetBulletPerShot(3);
        this.SetTimeDelay(0.5f);
        this.SetArrayDirectionBullet(new int[] { -15, 0, 15 });
        this.SetCanFire(true);
    }
    private void Update()
    {
        if (this.canFire)
        {
            if (!this.playerController.ButtonAttack.IsAttack) return;
            Debug.Log("player: " + this.playerController.ButtonAttack.IsAttack);
            this.Shooting();
            this.canFire = false;
        }
        if (!this.Timing()) return;
        this.canFire = true;
    }
    protected virtual void Shooting()
    {
        for(int i = 0; i < this.bulletPerShot; i++)
        {
            Quaternion newFirePoint = this.firePoint.rotation;
            newFirePoint.y = this.arrayDirectionBullet[i];
            SpawnBullet.Instance.ExecuteSpawnPooling(this.bulletPrefab, this.firePoint.position, newFirePoint);
        }
    }
    protected virtual void LoadPlayerController()
    {
        if (this.playerController != null) return;
        this.playerController = GetComponentInParent<PlayerController>();
        Debug.LogWarning(transform.name + " : LoadPlayerController");
    }
    protected virtual void GetBulletPrefab()
    {
        if (this.playerController == null) return;
        this.bulletPrefab = this.playerController.Bullet;
    }
    protected virtual void GetFirePoint()
    {
        if (this.playerController == null) return;
        this.firePoint = this.playerController.FirePoint;
    }
    protected virtual void GetBulletPerShot(int bulletPerShot)
    {
        this.bulletPerShot = bulletPerShot;
    }
    protected virtual void SetTimeDelay(float timeDelay)
    {
        this.timeDelay= timeDelay;
    }
    protected virtual void SetCanFire(bool canFire)
    {
        this.canFire = canFire;
    }
    protected virtual void SetArrayDirectionBullet(int[] arrayDirectionBullet)
    {
        this.arrayDirectionBullet = arrayDirectionBullet;
        ;
    }
    protected virtual bool Timing()
    {
        this.timer += Time.deltaTime;
        if (this.timer < this.timeDelay) return false;
        this.timer = 0f;
        return true;
    }
}
