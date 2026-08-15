using UnityEngine;
[CreateAssetMenu(fileName ="WeaponDefault",menuName = "ScriptableObject/WeaponData")]
public class WeaponData : ScriptableObject
{
    public float baseDamage;
    public float damageMultiplier;
    public float moveSpeed;
    public float maxDistance;
}
