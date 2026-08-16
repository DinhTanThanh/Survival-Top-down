using UnityEngine;

public class DamageReceiver : LoadMonoBehaviour
{
    [SerializeField] protected float baseHp;
    [SerializeField] protected float hp;
    [SerializeField] protected int defence;
    [SerializeField] protected bool isDead;
    [SerializeField] protected bool isExecuteDead;
    [SerializeField] protected BaseEntityController baseEntityController;
    protected override void LoadComponent()
    {
        base.LoadComponent();
        this.LoadBaseEntityController();
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
    private void Update()
    {
        if (this.isExecuteDead) return;
        if (!this.isDead) return;
        this.baseEntityController.Animator.SetTrigger("Dead");
        this.isExecuteDead = true;
    }
}
