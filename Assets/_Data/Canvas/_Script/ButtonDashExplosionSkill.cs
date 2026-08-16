using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class ButtonDashExplosionSkill : BaseButton
{
    [SerializeField] protected float timer;
    [SerializeField] protected float elapsedTime;
    [SerializeField] protected float coolDown;
    [SerializeField] protected float explosionRedius;
    [SerializeField] protected float dashDistance;
    [SerializeField] protected float dashDuration;
    [SerializeField] protected bool canUseSkill;
    [SerializeField] protected Transform player;
    [SerializeField] protected GameObject rangeIndicator;
    [SerializeField] protected TextMeshProUGUI textMeshProUGUI;
    [SerializeField] protected DashExplosionData dashExplosionData;
    [SerializeField] protected PlayerController playerController;
    public SkillData SkillData => dashExplosionData;
    protected override void LoadComponent()
    {
        base.LoadComponent();
        this.SetCanUseSkill(true);
        this.LoadRangIndicator();
        this.LoadDashExplosionData();
        this.LoadPlayer();
        this.LoadTextMeshProUGUI();
        this.LoadPlayerController();
        this.SetExplosionRadius(this.dashExplosionData.ExplosionRadius);
        this.SetDashDistance(this.dashExplosionData.DashDistance);
        this.SetDashDuration(this.dashExplosionData.DashDuration);
        this.SetCoolDown(this.dashExplosionData.CoolDown);
    }
    protected virtual void SetExplosionRadius(float explosionRadius)
    {
        this.explosionRedius= explosionRadius;
    }
    protected virtual void SetDashDuration(float dashDuration)
    {
        this.dashDuration = dashDuration;   
    }
    protected virtual void SetDashDistance(float dashDistance)
    {
        this.dashDistance = dashDistance;
    }
    protected virtual void SetCoolDown(float coolDown)
    {
        this.coolDown = coolDown;
    }
    protected virtual void SetCanUseSkill(bool canUseSkill)
    {
        this.canUseSkill = canUseSkill;
    }
    protected virtual void LoadPlayer()
    {
        if (this.player != null) return;
        this.player = GameObject.Find("Player")?.transform;
        Debug.LogWarning(transform.name + " : LoadPlayer");
    }
    protected virtual void LoadRangIndicator()
    {
        if (this.rangeIndicator != null) return;
        this.rangeIndicator = GameObject.Find("RangeIndicator");
        Debug.LogWarning(transform.name + " : LoadRangeIndicator");
    }
    protected virtual void LoadDashExplosionData()
    {
        if (this.dashExplosionData != null) return;
        this.dashExplosionData = Resources.Load<DashExplosionData>("WeaponData/DashExplosionData");
        Debug.LogWarning(transform.name + " : LoadDashExplosionData");
    }
    protected virtual void LoadTextMeshProUGUI()
    {
        if (this.textMeshProUGUI != null) return;
        this.textMeshProUGUI = GetComponentInChildren<TextMeshProUGUI>();
        Debug.LogWarning(transform.name + " : LoadTextMesh");
    }
    protected virtual void LoadPlayerController()
    {
        if (this.playerController != null) return;
        this.playerController = FindFirstObjectByType<PlayerController>();
        Debug.LogWarning(transform.name + " : LoadPlayerController");
    }
    private void Update()
    {
        if (this.canUseSkill) return;
        if (!this.Timing())
        {
            this.textMeshProUGUI.text = (this.coolDown - this.timer).ToString("F1");
            return;
        }
        this.textMeshProUGUI.text = "";
        this.canUseSkill = true;
    }
    public virtual void ExecuteDashExplosionSkill()
    {
        if (!this.canUseSkill) return;
        StartCoroutine(PerformDash());
    }
    IEnumerator PerformDash()
    {
        Vector3 posStart = this.player.position;
        Vector3 posDestination = this.player.position + this.player.forward * this.dashDistance;
        this.playerController.Animator.SetTrigger("IsRunGuard");
        while (this.elapsedTime <= this.dashDuration)
        {
            this.player.position = Vector3.Lerp(posStart, posDestination, this.elapsedTime / this.dashDuration);
            this.elapsedTime += Time.deltaTime;
            yield return null;
        }
        this.player.position = posDestination;
        this.elapsedTime = 0f;
        this.canUseSkill = false;
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(this.player.position, this.explosionRedius);
    }
    public virtual void EnableRangeIndicator()
    {
        this.rangeIndicator.SetActive(true);
    }
    public virtual void DisableRangeIndicator()
    {
        this.rangeIndicator.SetActive(false);
    }
    protected virtual bool Timing()
    {
        this.timer += Time.deltaTime;
        if (this.timer < this.coolDown) return false;
        this.timer = 0;
        return true;
    }
}
