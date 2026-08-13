using UnityEngine;

public class DamageReceiver : LoadMonoBehaviour
{
    [SerializeField] protected float baseHp;
    [SerializeField] protected float hp;
    [SerializeField] protected int defence;
    protected virtual float CalculateDefence(float damage)
    {
        return damage - this.defence;
    }
}
