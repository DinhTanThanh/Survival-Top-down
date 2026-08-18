using System.Collections.Generic;
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
    [SerializeField] protected float shootRange = 10f;
    [SerializeField] protected float aimAngle = 90f;
    [SerializeField] protected bool canFire;
    [SerializeField] protected bool enable360Fallback = true;
    [SerializeField] protected LayerMask enemyLayerMask = ~0;
    [SerializeField] protected GameObject bulletPrefab;
    [SerializeField] protected Transform firePoint;
    [SerializeField] protected BaseChargeSystem baseChargeSystem;
    [SerializeField] protected PlayerController playerController;
    [SerializeField] protected List<IChargeObserver> listChargeObserver=new List<IChargeObserver>();
    public float ChargeRegenTime => chargeRegenTime;
    public int MaxCharge => maxCharge;
    public int CurrentCharge
    {
        get { return this.currentCharge; }
        set {this.currentCharge = value;}
    }
    public BaseChargeSystem BaseChargeSystem => baseChargeSystem;
    public PlayerController PlayerController => playerController;
    protected override void LoadComponent()
    {
        base.LoadComponent();
        this.LoadBaseChargeSystem();
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
    public virtual void AddChargeObserver(IChargeObserver chargeObserver)
    {
        this.listChargeObserver.Add(chargeObserver);
    }
    protected virtual void OnChangeChargeUI()
    {
        foreach(IChargeObserver chargeObserver in this.listChargeObserver)
        {
            chargeObserver.UpdateCharge();
        }
    }
    private void Update()
    {
        if (this.canFire)
        {
            if (this.currentCharge <= 0) return;
            if (!this.playerController.ButtonAttack.IsAttack) return;
            this.AutoAimNearestEnemy();
            this.playerController.Animator.SetTrigger("Shoot");
            this.currentCharge--;
            this.OnChangeChargeUI();
            this.canFire = false;
        }
        if (!this.Timing()) return;
        this.canFire = true;
    }
    protected virtual void Shooting()
    {
        this.AutoAimNearestEnemy();
        Quaternion rotCurrent = this.playerController.transform.rotation;
        for(int i = 0; i < this.bulletPerShot; i++)
        {
            Quaternion newRotation = Quaternion.Euler(0f, this.bulletAngles[i],0f);
            Quaternion rot = newRotation * rotCurrent;
            SpawnBullet.Instance.ExecuteSpawnPooling(this.bulletPrefab, this.firePoint.position, rot);
        }
    }
    protected virtual void AutoAimNearestEnemy()
    {
        if (this.playerController == null) return;
        PlayerMoving playerMoving = this.playerController.GetComponentInChildren<PlayerMoving>();
        if (playerMoving != null && playerMoving.IsDashing) return;

        Vector3 playerPos = this.playerController.transform.position;
        Vector3 playerForward = this.playerController.transform.forward;
        playerForward.y = 0f;
        if (playerForward == Vector3.zero) playerForward = Vector3.forward;
        playerForward.Normalize();
        Collider[] hitColliders = Physics.OverlapSphere(playerPos, this.shootRange, this.enemyLayerMask);
        Transform nearestInCone = null;
        float minDist = float.MaxValue;
        Transform nearestAnywhere = null;
        float minDistAnywhere = float.MaxValue;
        foreach (Collider collider in hitColliders)
        {
            DamageReceiver damageReceiver = collider.transform.parent?.GetComponentInChildren<DamageReceiver>();
            if (damageReceiver == null || damageReceiver is PlayerDamageReceiver) continue;
            Vector3 enemyPos = damageReceiver.transform.position;
            Vector3 dirToEnemy = enemyPos - playerPos;
            dirToEnemy.y = 0f;
            if (dirToEnemy == Vector3.zero) continue;
            float distanceSqr = dirToEnemy.sqrMagnitude;
            if (distanceSqr < minDistAnywhere)
            {
                minDistAnywhere = distanceSqr;
                nearestAnywhere = damageReceiver.transform;
            }
            float angle = Vector3.Angle(playerForward, dirToEnemy.normalized);
            if (angle <= this.aimAngle / 2f)
            {
                if (distanceSqr < minDist)
                {
                    minDist = distanceSqr;
                    nearestInCone = damageReceiver.transform;
                }
            }
        }
        Transform targetEnemy = nearestInCone != null ? nearestInCone : (this.enable360Fallback ? nearestAnywhere : null);
        if (targetEnemy != null)
        {
            Vector3 targetDir = targetEnemy.position - playerPos;
            targetDir.y = 0f;
            if (targetDir != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(targetDir);
                this.playerController.transform.rotation = targetRotation;

                Rigidbody rb = this.playerController.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.rotation = targetRotation;
                }
            }
        }
    }
    protected virtual void LoadBaseChargeSystem()
    {
        if (this.baseChargeSystem != null) return;
        ;this.baseChargeSystem = GetComponentInChildren<BaseChargeSystem>();
        Debug.LogWarning(transform.name + " : LoadBaseChargeSystem");
    }
    protected virtual void LoadPlayerController()
    {
        if (this.playerController != null) return;
        this.playerController = GetComponentInParent<PlayerController>();
        Debug.LogWarning(transform.name + " : LoadPlayerController");
    }
    public virtual int GetCurrentCharge()
    {
        return this.currentCharge;
    }
    public virtual int GetMaxCharge()
    {
        return this.maxCharge;
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
