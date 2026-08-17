using UnityEngine;
[CreateAssetMenu(fileName ="EntityDefault", menuName = "ScriptableObject/EntitySO")]
public class EntitySO : ScriptableObject
{
    public float baseHp;
    public float baseSpeed;
    public int baseRotation;
    public int baseDefence;
    public int baseDamage;
    public float damageMultiplier;
    public float attackRange;
    public float expReward;
}
