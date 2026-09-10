using System.Collections.Generic;
using UnityEngine;

public class DamageReceiver : LoadMonoBehaviour
{
    [SerializeField] protected int defence;
    [SerializeField] protected float baseHp;
    [SerializeField] protected float hp;
    [SerializeField] protected float damageMultiplier;
    [SerializeField] protected bool isTakeDamage;
    [SerializeField] protected bool isDead;
    [SerializeField] protected float expReward;
    [SerializeField] protected BaseEntityController baseEntityController;
    [SerializeField] protected List<IHealthObserver> listHealthObserver = new List<IHealthObserver>();
    protected virtual void OnEnable()
    {
        this.Reborn();
    }
    protected override void LoadComponent()
    {
        base.LoadComponent();
        this.LoadBaseEntityController();
    }
    protected virtual void LoadBaseEntityController()
    {
        if (this.baseEntityController != null) return;
        this.baseEntityController = GetComponentInParent<BaseEntityController>();
        Debug.LogWarning(transform.name + " : LoadBaseEntityController");
    }
    protected virtual float CalculateDefence(float damage)
    {
        return Mathf.Max(damage - this.defence, 0);
    }
    public virtual void ReduceHp(float damage)
    {
        damage = this.CalculateDefence(damage);
        this.hp = Mathf.Max(this.hp - damage, 0);
        this.OnChangeUI();
        if (this.hp <= 0)
        {
            this.isDead = true;
        }
    }
    public virtual void ExecuteDead()
    {
    }
    public virtual void AddHealthObserver(IHealthObserver healthObserver)
    {
        if (healthObserver == null) return;
        if (!this.listHealthObserver.Contains(healthObserver))
        {
            this.listHealthObserver.Add(healthObserver);
        }
    }
    protected virtual void OnChangeUI()
    {
        for (int i = this.listHealthObserver.Count - 1; i >= 0; i--)
        {
            if (this.listHealthObserver[i] == null)
            {
                this.listHealthObserver.RemoveAt(i);
                continue;
            }
            this.listHealthObserver[i].UpdateHealthHp();
        }
    }
    public virtual bool GetIsDead()
    {
        return this.isDead;
    }
    public virtual float GetBaseHp()
    {
        return this.baseHp;
    }
    public virtual float GetHp()
    {
        return this.hp;
    }
    protected virtual void SetDamageMultiplier(float damageMultiplier)
    {
        this.damageMultiplier = damageMultiplier;
    }
    protected virtual void SetExperienceReward(float expReward)
    {
        this.expReward = expReward;
    }
    public virtual void SetIsDead(bool isDead)
    {
        this.isDead = isDead;
    }
    protected virtual void SetBaseHp(float baseHp)
    {
        this.baseHp = baseHp;
    }
    protected virtual void SetHp(float hp)
    {
        this.hp = hp;
    }
    protected virtual void SetDefence(int defence)
    {
        this.defence = defence;
    }
    public virtual void SetIsTakeDamage(bool isTakeDamage)
    {
        this.isTakeDamage = isTakeDamage;
    }
    public virtual bool GetIsTakeDamage()
    {
        return this.isTakeDamage;
    }

    public virtual void AddBaseHealth(float health)
    {
        if (this.isDead) return;
        this.baseHp += health;
    }
    public virtual void AddHealth(float health)
    {
        if (this.isDead) return;
        float newHP = this.hp+health;
        if (newHP >= this.baseHp)
        {
            this.hp = this.baseHp;
        }
        else
        {
            this.hp += health;
        }
        this.OnChangeUI();
    }
    public virtual void AddDefence(int defence)
    {
        if (this.isDead) return;
        this.defence += defence;
    }
    public virtual void AddDamageMultiplier(float damageMultiplier)
    {
        if (this.isDead) return;
        this.damageMultiplier += damageMultiplier;
    }
    public virtual void Reborn()
    {
        this.isDead = false;
        this.isTakeDamage = false;
        if (this.baseEntityController == null)
        {
            this.LoadBaseEntityController();
        }
        if (this.baseEntityController != null && this.baseEntityController.EntitySO != null)
        {
            this.baseHp = this.baseEntityController.EntitySO.baseHp;
            this.defence = this.baseEntityController.EntitySO.baseDefence;
            this.damageMultiplier = this.baseEntityController.EntitySO.damageMultiplier;
            this.expReward = this.baseEntityController.EntitySO.expReward;
        }
        if (this.baseHp <= 0)
        {
            this.baseHp = 500f;
        }
        this.hp = this.baseHp;
        this.OnChangeUI();
    }
}
