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
    [SerializeField] protected BulletController bulletController;
    [SerializeField] protected float[] bulletAngles;
    protected override void LoadComponent()
    {
        base.LoadComponent();
        this.LoadPlayerController();
        this.GetBulletPrefab();
        this.LoadBulletController();
        this.GetFirePoint();
        this.GetBulletPerShot(this.bulletController.WeaponData.bulletsPerShot);
        this.SetTimeDelay(this.bulletController.WeaponData.fireCooldown);
        this.SetArrayDirectionBullet(this.bulletController.WeaponData.bulletAngles);
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
            Quaternion newRotation = Quaternion.Euler(0f, this.bulletAngles[i],0f);
            SpawnBullet.Instance.ExecuteSpawnPooling(this.bulletPrefab, this.firePoint.position, newRotation);
        }
    }
    protected virtual void LoadPlayerController()
    {
        if (this.playerController != null) return;
        this.playerController = GetComponentInParent<PlayerController>();
        Debug.LogWarning(transform.name + " : LoadPlayerController");
    }
    protected virtual void LoadBulletController()
    {
        if (this.bulletPrefab == null) return;
        if (this.bulletController != null) return;
        this.bulletController=this.bulletPrefab.GetComponent<BulletController>();
        Debug.LogWarning(transform.name + " : LoadBulletController");
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
    protected virtual void SetArrayDirectionBullet(float[] arrayDirectionBullet)
    {
        this.bulletAngles = arrayDirectionBullet;
    }
    protected virtual bool Timing()
    {
        this.timer += Time.deltaTime;
        if (this.timer < this.timeDelay) return false;
        this.timer = 0f;
        return true;
    }
}
