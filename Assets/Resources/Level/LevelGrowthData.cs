using UnityEngine;
[CreateAssetMenu(fileName ="LevelDefault",menuName = "ScriptableObject/LevelSO")]
public class LevelGrowthData : ScriptableObject
{
    public int BaseLevel;
    public float ExperienceToNextLevel;
    public float HealthIncreasePerLevel;
    public int DefenceIncreasePerLevel;
    public float DamageMultiplierIncreasePerLevel;
}
