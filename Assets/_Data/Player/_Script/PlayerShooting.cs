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
    public bool CanFire => canFire && currentCharge > 0;
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
        if (!this.canFire)
        {
            if (this.Timing())
            {
                this.canFire = true;
            }
        }
    }
    public virtual void Shooting()
    {
        if (!this.canFire || this.currentCharge <= 0) return;
        Transform targetEnemy = this.GetTargetEnemy();
        Quaternion shootRotation = this.playerController.transform.rotation;

        if (targetEnemy != null)
        {
            Vector3 targetDir = targetEnemy.position - this.playerController.transform.position;
            targetDir.y = 0f;
            if (targetDir.sqrMagnitude > 0.001f)
            {
                shootRotation = Quaternion.LookRotation(targetDir);
                this.playerController.transform.rotation = shootRotation;
                Rigidbody rb = this.playerController.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.rotation = shootRotation;
                }
            }
        }

        for (int i = 0; i < this.bulletPerShot; i++)
        {
            Quaternion newRotation = Quaternion.Euler(0f, this.bulletAngles[i], 0f);
            Quaternion rot = newRotation * shootRotation;
            SpawnBullet.Instance.ExecuteSpawnPooling(this.bulletPrefab, this.firePoint.position, rot);
        }
        this.currentCharge--;
        this.OnChangeChargeUI();
        this.canFire = false;
        this.timer = 0f;
    }

    protected virtual Transform GetTargetEnemy()
    {
        if (this.playerController == null) return null;

        Vector3 playerPos = this.playerController.transform.position;
        Vector3 playerForward = this.playerController.transform.forward;
        playerForward.y = 0f;
        if (playerForward == Vector3.zero) playerForward = Vector3.forward;
        playerForward.Normalize();

        Collider[] hitColliders = Physics.OverlapSphere(playerPos, this.shootRange, this.enemyLayerMask);
        Transform bestTarget = null;
        float bestScore = float.MaxValue;

        foreach (Collider collider in hitColliders)
        {
            if (collider == null) continue;
            DamageReceiver damageReceiver = collider.transform.parent?.GetComponentInChildren<DamageReceiver>();
            if (damageReceiver == null)
            {
                damageReceiver = collider.GetComponentInParent<DamageReceiver>();
            }
            if (damageReceiver == null || damageReceiver is PlayerDamageReceiver || damageReceiver.GetIsDead()) continue;

            Vector3 enemyPos = damageReceiver.transform.position;
            Vector3 dirToEnemy = enemyPos - playerPos;
            dirToEnemy.y = 0f;
            float distance = dirToEnemy.magnitude;
            if (distance <= 0.001f || distance > this.shootRange) continue;

            float angle = Vector3.Angle(playerForward, dirToEnemy.normalized);
            float score = distance + (angle / 180f) * 2f;
            if (score < bestScore)
            {
                bestScore = score;
                bestTarget = damageReceiver.transform;
            }
        }

        return bestTarget;
    }
    protected virtual void LoadBaseChargeSystem()
    {
        if (this.baseChargeSystem != null) return;
        ;this.baseChargeSystem = FindFirstObjectByType<BaseChargeSystem>();
        Debug.LogWarning(transform.name + " : LoadBaseChargeSystem");
    }
    protected virtual void LoadPlayerController()
    {
        if (this.playerController != null) return;
        this.playerController = GetComponentInParent<PlayerController>();
        Debug.LogWarning(transform.name + " : LoadPlayerController");
    }
    public virtual bool GetCanFire()
    {
        return this.canFire;
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
