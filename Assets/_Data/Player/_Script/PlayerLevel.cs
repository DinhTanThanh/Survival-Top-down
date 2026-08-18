using TMPro;
using UnityEngine;

public class PlayerLevel : LoadMonoBehaviour
{
    [SerializeField] protected int level;
    [SerializeField] protected int levelCurrent;
    [SerializeField] protected float expToNextLevel;
    [SerializeField] protected float expCurrent;
    [SerializeField] protected float healthIncreasePerLevel;
    [SerializeField] protected int defenceIncreasePerLevel;
    [SerializeField] protected float damageMultiplierIncreasePerLevel;
    [SerializeField] protected PlayerController playerController;
    [SerializeField] protected TextMeshProUGUI textMeshProUGUI;
    protected override void LoadComponent()
    {
        base.LoadComponent();
        this.LoadPlayerController();
        this.LoadTextMeshProUGUI();
        this.SetbaseLevel(this.playerController.LevelGrowthData.BaseLevel);
        this.SetExperienceToNextLevel(this.playerController.LevelGrowthData.ExperienceToNextLevel);
        this.SetHealthIncreasePerLevel(this.playerController.LevelGrowthData.HealthIncreasePerLevel);
        this.SetDefenceIncreasePerLevel(this.playerController.LevelGrowthData.DefenceIncreasePerLevel);
        this.SetDamageMultiplierIncreasePerLevel(this.playerController.LevelGrowthData.DamageMultiplierIncreasePerLevel);
    }
    protected virtual void LoadTextMeshProUGUI()
    {
        if (this.textMeshProUGUI != null) return;
        this.textMeshProUGUI=GetComponent<TextMeshProUGUI>();
        Debug.LogWarning(transform.name + " : LoadTextMeshProUGUI");
    }
    protected virtual void LoadPlayerController()
    {
        if (this.playerController != null) return;
        this.playerController = GetComponentInParent<PlayerController>();
        Debug.LogWarning(transform.name + " : LoadPlayerController");
    }
    protected virtual void SetbaseLevel(int baseLevel)
    {
        this.level= baseLevel;
    }
    protected virtual void SetExperienceToNextLevel(float experienceToNextLevel)
    {
        this.expToNextLevel = experienceToNextLevel;
    }
    protected virtual void SetHealthIncreasePerLevel(float healthIncreasePerLevel)
    {
        this.healthIncreasePerLevel = healthIncreasePerLevel;
    }
    protected virtual void SetDefenceIncreasePerLevel(int defenceIncreasePerLevel)
    {
        this.defenceIncreasePerLevel = defenceIncreasePerLevel;
    }
    protected virtual void SetDamageMultiplierIncreasePerLevel(float damageMultiplierIncreasePerLevel)
    {
        this.damageMultiplierIncreasePerLevel = damageMultiplierIncreasePerLevel;
    }
    private void Update()
    {
        this.CalculateLevel();
        if (this.level != this.levelCurrent)
        {
            this.levelCurrent = this.level;
            this.textMeshProUGUI.text = this.levelCurrent + "";
        }
    }
    protected virtual void CalculateLevel()
    {
        if (this.expCurrent < this.expToNextLevel) return;
        this.expCurrent -= this.expToNextLevel;
        this.level++;
        this.LeveUp();
    }
    protected virtual void LeveUp()
    {
        this.playerController.DamageReceiver.AddBaseHealth(this.healthIncreasePerLevel);
        this.playerController.DamageReceiver.AddHealth(this.healthIncreasePerLevel);
        this.playerController.DamageReceiver.AddDefence(this.defenceIncreasePerLevel);
        this.playerController.DamageReceiver.AddDamageMultiplier(this.damageMultiplierIncreasePerLevel);
    }
    public virtual void AddExpReward(float expReward)
    {
        this.expCurrent += expReward;
    }
}
