using UnityEngine;
[CreateAssetMenu(fileName ="ShotDefault",menuName = "ScriptableObject/ShotData")]
public class ShotData : ScriptableObject
{
    public int bulletsPerShot;
    public float[] bulletAngles;
    public float fireCooldown;
    public int maxCharge;
    public float chargeRegenTime;
}
