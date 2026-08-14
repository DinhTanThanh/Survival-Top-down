using UnityEngine;

public class DamageReceiver : LoadMonoBehaviour
{
    [SerializeField] protected float baseHp;
    [SerializeField] protected float hp;
    [SerializeField] protected int defence;
    [SerializeField] protected bool isDead;
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
}
