using UnityEngine;

public class DamageReceiver : LoadMonoBehaviour
{
    [SerializeField] protected int defence;
    [SerializeField] protected float baseHp;
    [SerializeField] protected float hp;
    [SerializeField] protected float damageMultiplier;
    [SerializeField] protected bool isDead;
    [SerializeField] protected bool isExecuteDead;
    [SerializeField] protected float expReward;
    [SerializeField] protected BaseEntityController baseEntityController;
    protected override void LoadComponent()
    {
        base.LoadComponent();
        this.LoadBaseEntityController();
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
    protected virtual void LoadBaseEntityController()
    {
        if (this.baseEntityController != null) return;
        this.baseEntityController=GetComponentInParent<BaseEntityController>();
        Debug.LogWarning(transform.name + " : LoadBaseEntityController");
    }
    protected virtual float CalculateDefence(float damage)
    {
        return Mathf.Max(damage - this.defence,0);
    }
    public virtual void ReduceHp(float damage)
    {
        damage=this.CalculateDefence(damage);
        this.hp = Mathf.Max(this.hp - damage,0);
        if (this.hp <= 0)
        {
            this.isDead = true;
        }
    }
    public virtual void AddBaseHealth(float health)
    {
        this.baseHp+= health;
    }
    public virtual void AddHealth(float health)
    {
        this.hp += health;
    }
    public virtual void AddDefence(int defence)
    {
        this.defence+= defence;
    }
    public virtual void AddDamageMultiplier(float damageMultiplier)
    {
        this.damageMultiplier += damageMultiplier;
    }
    protected virtual void Update()
    {
        if (this.isExecuteDead) return;
        if (!this.isDead) return;
        this.baseEntityController.Animator.SetTrigger("Dead");
        this.isExecuteDead = true;
    }
    protected virtual void Reborn()
    {
        this.isDead = false;
        this.hp = this.baseHp;
        this.isExecuteDead= false;
    }
}
