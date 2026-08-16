using TMPro;
using UnityEditor.MPE;
using UnityEngine;

public class ButtonBombSkill : BaseButton
{
    [SerializeField] protected float timer;
    [SerializeField] protected float coolDown;
    [SerializeField] protected bool canUseSkill;
    [SerializeField] protected Transform player;
    [SerializeField] protected GameObject bombPrefab;
    [SerializeField] protected GameObject rangeIndicator;
    [SerializeField] protected TextMeshProUGUI textMeshProUGUI;
    [SerializeField] protected SkillData skillData;
    public SkillData SkillData => skillData;
    
    protected override void LoadComponent()
    {
        base.LoadComponent();
        this.SetCanUseSkill(true);
        this.LoadRangIndicator();
        this.LoadSkillData();
        this.LoadPlayer();
        this.LoadTextMeshProUGUI();
        this.SetCoolDown(this.skillData.CoolDown);
        this.SetBombPrefab(this.skillData.prefab);
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
    protected virtual void SetBombPrefab(GameObject prefab)
    {
        if (prefab == null) return;
        this.bombPrefab = prefab;
    }
    protected virtual void LoadRangIndicator()
    {
        if (this.rangeIndicator != null) return;
        this.rangeIndicator = GameObject.Find("RangeIndicator");
        Debug.LogWarning(transform.name + " : LoadRangeIndicator");
    }
    protected virtual void LoadSkillData()
    {
        if (this.skillData != null) return;
        this.skillData = Resources.Load<SkillData>("WeaponData/BombData");
        Debug.LogWarning(transform.name + " : LoadSkillData");
    }
    protected virtual void LoadTextMeshProUGUI()
    {
        if(this.textMeshProUGUI != null) return;
        this.textMeshProUGUI=GetComponentInChildren<TextMeshProUGUI>();
        Debug.LogWarning(transform.name + " : LoadTextMesh");
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
    public virtual void ExecuteBombSkill()
    {
        if (!this.canUseSkill) return;
        Vector3 pos = this.player.position;
        pos.y = 0.7f;
        SpawnBomb.Instance.ExecuteSpawnPooling(this.bombPrefab, pos, Quaternion.identity);
        this.canUseSkill = false;
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
        this.timer = 0f;
        return true;
    }
}
