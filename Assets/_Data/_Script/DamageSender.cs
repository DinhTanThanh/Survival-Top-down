using UnityEngine;

public class DamageSender : LoadMonoBehaviour
{
    [SerializeField] protected float baseDamage;
    [SerializeField] protected float damageMultiplier;
    protected virtual float CalculateDamage()
    {
        return this.baseDamage * (1 + this.damageMultiplier);
    }
}
