using TMPro;
using UnityEngine;

public class PlayerLevel : LoadMonoBehaviour
{
    [SerializeField] protected int level;
    [SerializeField] protected int levelCurrent;
    [SerializeField] protected int defenceIncreasePerLevel;
    [SerializeField] protected float expToNextLevel;
    [SerializeField] protected float expCurrent;
    [SerializeField] protected float healthIncreasePerLevel;
    [SerializeField] protected float damageMultiplierIncreasePerLevel;
    [SerializeField] protected bool isLockRotation = true;
    [SerializeField] protected bool faceCamera = false;
    [SerializeField] protected PlayerController playerController;
    [SerializeField] protected TextMeshProUGUI textMeshProUGUI;
    [SerializeField] protected Transform targetTransform;
    [SerializeField] protected Quaternion fixedRotation;
    [SerializeField] protected Vector3 fixedWorldOffset;

    protected override void Awake()
    {
        base.Awake();
        this.InitFixedRotation();
    }

    protected virtual void Start()
    {
        this.InitFixedRotation();
    }

    protected virtual void InitFixedRotation()
    {
        if (this.targetTransform == null)
        {
            Canvas parentCanvas = GetComponentInParent<Canvas>();
            if (parentCanvas != null && parentCanvas.renderMode == RenderMode.WorldSpace)
            {
                this.targetTransform = parentCanvas.transform;
            }
            else if (transform.parent != null)
            {
                this.targetTransform = transform.parent;
            }
            else
            {
                this.targetTransform = transform;
            }
        }

        if (this.fixedRotation.w == 0f && this.fixedRotation.x == 0f && this.fixedRotation.y == 0f && this.fixedRotation.z == 0f)
        {
            this.fixedRotation = this.targetTransform.rotation;
        }

        if (this.playerController != null && this.fixedWorldOffset == Vector3.zero)
        {
            this.fixedWorldOffset = this.targetTransform.position - this.playerController.transform.position;
        }
    }

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
    protected virtual void LateUpdate()
    {
        this.LockRotation();
    }
    protected virtual void LockRotation()
    {
        if (!this.isLockRotation || this.targetTransform == null) return;

        if (this.playerController != null)
        {
            this.targetTransform.position = this.playerController.transform.position + this.fixedWorldOffset;
        }

        if (this.faceCamera && Camera.main != null)
        {
            this.targetTransform.rotation = Camera.main.transform.rotation;
        }
        else
        {
            this.targetTransform.rotation = this.fixedRotation;
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
