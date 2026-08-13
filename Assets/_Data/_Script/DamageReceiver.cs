using UnityEngine;

public class DamageReceiver : LoadMonoBehaviour
{
    [SerializeField] protected float baseHp;
    [SerializeField] protected float hp;
    [SerializeField] protected int defence;
    protected virtual float CalculateDefence(float damage)
    {
        return Mathf.Max(damage - this.defence,0);
    }
    protected virtual void ReduceHp(float damage)
    {
        float finalHp = Mathf.Max(this.hp - damage,0);
    }
}
