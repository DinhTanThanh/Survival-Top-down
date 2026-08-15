using UnityEngine;
[CreateAssetMenu(fileName ="SkillsDefault",menuName = "ScriptableObject/SkillData")]
public class SkillData : ScriptableObject
{
    public float TimeDelay;
    public int BaseDamage;
    public float ExplosionRadius;
    public float CoolDown;
    public GameObject prefab;
}
