using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class ButtonHealSkill : BaseButton
{
    [SerializeField] protected float timer;
    [SerializeField] protected float coolDown = 20f;
    [SerializeField] protected float healDuration = 5f;
    [SerializeField] protected float healInterval = 1f;
    [SerializeField] protected float healPercentPerSecond = 0.05f;
    [SerializeField] protected bool canUseSkill = true;
    [SerializeField] protected DamageReceiver playerDamageReceiver;
    [SerializeField] protected TextMeshProUGUI textMeshProUGUI;
    [SerializeField] protected HealSkillData healSkillData;
    [SerializeField] protected Coroutine healCoroutine;
    [SerializeField] protected Indicator indicator;
    [SerializeField] protected ParticleSystem fx_Healing;

    public SkillData SkillData => healSkillData;
    protected override void LoadComponent()
    {
        base.LoadComponent();
        this.SetCanUseSkill(true);
        this.LoadHealSkillData();
        this.LoadPlayerDamageReceiver();
        this.LoadTextMeshProUGUI();
        if (this.healSkillData != null)
        {
            this.SetCoolDown(this.healSkillData.CoolDown);
            this.SetHealDuration(this.healSkillData.HealDuration);
            this.SetHealInterval(this.healSkillData.HealInterval);
            this.SetHealPercentPerSecond(this.healSkillData.HealPercentPerSecond);
        }
        this.LoadIndicator();
        this.LoadFxHealing();   
    }
    protected virtual void Start()
    {
        if (this.button != null)
        {
            this.button.onClick.AddListener(this.ExecuteHealSkill);
        }
    }
    protected virtual void LoadFxHealing()
    {
        if (this.indicator == null) return;
        if (this.fx_Healing != null) return;
        this.fx_Healing = this.indicator.GetParticleByName("FX_Healing");
        Debug.LogWarning(transform.name + " : LoadFXHealing");
    }
    protected virtual void LoadIndicator()
    {
        if (this.indicator != null) return;
        this.indicator = FindFirstObjectByType<Indicator>();
        Debug.LogWarning(transform.name + " : LoadIndicator");
    }
    protected virtual void LoadPlayerDamageReceiver()
    {
        if (this.playerDamageReceiver != null) return;
        this.playerDamageReceiver = GameObject.Find("Player")?.GetComponentInChildren<DamageReceiver>();
        Debug.LogWarning(transform.name + " : LoadPlayerDamageReceiver");
    }

    protected virtual void LoadHealSkillData()
    {
        if (this.healSkillData != null) return;
        this.healSkillData = Resources.Load<HealSkillData>("WeaponData/HealSkillData");
        Debug.LogWarning(transform.name + " : LoadHealSkillData");
    }
    protected virtual void LoadTextMeshProUGUI()
    {
        if (this.textMeshProUGUI != null) return;
        this.textMeshProUGUI = GetComponentInChildren<TextMeshProUGUI>();
        Debug.LogWarning(transform.name + " : LoadTextMesh");
    }

    protected virtual void SetCoolDown(float coolDown)
    {
        this.coolDown = coolDown;
    }

    protected virtual void SetHealDuration(float healDuration)
    {
        this.healDuration = healDuration;
    }

    protected virtual void SetHealInterval(float healInterval)
    {
        this.healInterval = healInterval;
    }

    protected virtual void SetHealPercentPerSecond(float healPercentPerSecond)
    {
        this.healPercentPerSecond = healPercentPerSecond;
    }

    protected virtual void SetCanUseSkill(bool canUseSkill)
    {
        this.canUseSkill = canUseSkill;
    }

    protected virtual void Update()
    {
        if (Keyboard.current != null && Keyboard.current.hKey.wasPressedThisFrame)
        {
            this.ExecuteHealSkill();
        }

        if (this.canUseSkill) return;

        if (!this.Timing())
        {
            if (this.textMeshProUGUI != null)
            {
                this.textMeshProUGUI.text = (this.coolDown - this.timer).ToString("F1");
            }
            return;
        }

        if (this.textMeshProUGUI != null)
        {
            this.textMeshProUGUI.text = "";
        }
        this.canUseSkill = true;
    }

    public virtual void ExecuteHealSkill()
    {
        if (!this.canUseSkill) return;
        if (this.playerDamageReceiver == null || this.playerDamageReceiver.GetIsDead()) return;

        if (this.healCoroutine != null)
        {
            StopCoroutine(this.healCoroutine);
        }
        this.healCoroutine = StartCoroutine(this.HealOverTime());
        this.fx_Healing.Play();
        this.canUseSkill = false;
        this.timer = 0f;
    }

    protected virtual IEnumerator HealOverTime()
    {
        float elapsed = 0f;
        while (elapsed < this.healDuration)
        {
            yield return new WaitForSeconds(this.healInterval);
            elapsed += this.healInterval;

            if (this.playerDamageReceiver == null || this.playerDamageReceiver.GetIsDead())
            {
                break;
            }

            float healAmount = this.playerDamageReceiver.GetBaseHp() * this.healPercentPerSecond;
            this.playerDamageReceiver.AddHealth(healAmount);
        }
        this.fx_Healing.Stop();
        this.healCoroutine = null;
    }

    protected virtual bool Timing()
    {
        this.timer += Time.deltaTime;
        if (this.timer < this.coolDown) return false;
        this.timer = 0f;
        return true;
    }
}
