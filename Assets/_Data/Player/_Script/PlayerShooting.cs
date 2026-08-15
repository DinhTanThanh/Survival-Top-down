using JetBrains.Annotations;
using Unity.Mathematics;
using UnityEngine;

public class PlayerShooting : LoadMonoBehaviour
{
    [SerializeField] protected int bulletPerShot;
    [SerializeField] protected int currentCharge;
    [SerializeField] protected int maxCharge;
    [SerializeField] protected float chargeRegenTime;
    [SerializeField] protected float timer;
    [SerializeField] protected float timeDelay;
    [SerializeField] protected float[] bulletAngles;
    [SerializeField] protected bool canFire;
    [SerializeField] protected GameObject bulletPrefab;
    [SerializeField] protected Transform firePoint;
    [SerializeField] protected PlayerController playerController;
    public float ChargeRegenTime => chargeRegenTime;
    public int MaxCharge => maxCharge;
    public int CurrentCharge
    {
        get { return this.currentCharge; }
        set {this.currentCharge = value;}
    }
    public PlayerController PlayerController => playerController;
    protected override void LoadComponent()
    {
        base.LoadComponent();
        this.LoadPlayerController();
        this.GetBulletPrefab();
        this.GetFirePoint();
        this.SetChargeRegenTime(this.playerController.ShotData.chargeRegenTime);
        this.SetMaxCharge(this.playerController.ShotData.maxCharge);
        this.SetCurrentCharge(this.playerController.ShotData.maxCharge);
        this.GetBulletPerShot(this.playerController.ShotData.bulletsPerShot);
        this.SetTimeDelay(this.playerController.ShotData.fireCooldown);
        this.SetArrayDirectionBullet(this.playerController.ShotData.bulletAngles);
        this.SetCanFire(true);
    }
    private void Update()
    {
        if (this.canFire)
        {
            if (this.currentCharge <= 0) return;
            if (!this.playerController.ButtonAttack.IsAttack) return;
            this.Shooting();
            this.currentCharge--;
            this.canFire = false;
        }
        if (!this.Timing()) return;
        this.canFire = true;
    }
    protected virtual void Shooting()
    {
        Quaternion rotCurrent = this.playerController.transform.rotation;
        for(int i = 0; i < this.bulletPerShot; i++)
        {
            Quaternion newRotation = Quaternion.Euler(0f, this.bulletAngles[i],0f);
            Quaternion rot = newRotation * rotCurrent;
            SpawnBullet.Instance.ExecuteSpawnPooling(this.bulletPrefab, this.firePoint.position, rot);
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
    protected virtual void SetChargeRegenTime(float chargeRegenTime)
    {
        this.chargeRegenTime = chargeRegenTime;
    }
    protected virtual void SetMaxCharge(int maxCharge)
    {
        this.maxCharge = maxCharge;
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
        if (arrayDirectionBullet.Length <= 0) return;
        this.bulletAngles = arrayDirectionBullet;
    }
    protected virtual void SetCurrentCharge(int numberCharge)
    {
        this.currentCharge = numberCharge;
    }
    protected virtual bool Timing()
    {
        this.timer += Time.deltaTime;
        if (this.timer < this.timeDelay) return false;
        this.timer = 0f;
        return true;
    }
}
