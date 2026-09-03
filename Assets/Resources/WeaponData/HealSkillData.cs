using UnityEngine;

[CreateAssetMenu(fileName = "HealSkillDefault", menuName = "ScriptableObject/HealSkillData")]
public class HealSkillData : SkillData
{
    public float HealDuration = 5f;
    public float HealInterval = 1f;
    public float HealPercentPerSecond = 0.05f;
}
