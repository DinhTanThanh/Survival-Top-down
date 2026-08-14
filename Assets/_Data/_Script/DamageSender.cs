using UnityEngine;

public class DamageSender : LoadMonoBehaviour
{
    [SerializeField] protected float baseDamage;
    [SerializeField] protected float damageMultiplier;
    protected virtual void SetBaseDamage(float baseDamage)
    {
        this.baseDamage = baseDamage;
    }
    protected virtual void SetDamageMultiplier(float damageMultiplier)
    {
        this.damageMultiplier = damageMultiplier;
    }
    public virtual float CalculateDamage()
    {
        return this.baseDamage * (1 + this.damageMultiplier);
    }
}
