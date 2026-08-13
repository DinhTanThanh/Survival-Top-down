using UnityEngine;
[CreateAssetMenu(fileName ="WeaponDefault",menuName = "ScriptableObject/WeaponData")]
public class WeaponData : ScriptableObject
{
    public float baseDamage;
    public float damageMultiplier;
    public int bulletsPerShot;
    public float[] bulletAngles;
    public float fireCooldown;
    public int maxCharge;
    public float chargeRegenTime;
}
