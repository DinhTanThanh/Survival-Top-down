using UnityEngine;

public class BaseSkillController : LoadMonoBehaviour
{
    [SerializeField] protected Animator animator;
    [SerializeField] protected SkillData skillData;
    public Animator Animator => animator;
    public SkillData SkillData => skillData;
    protected override void LoadComponent()
    {
        base.LoadComponent();
        this.LoadSkillData();
        this.LoadAnimator();
    }
    protected virtual void LoadSkillData()
    {
        if (this.skillData != null) return;
        this.skillData = Resources.Load<SkillData>("WeaponData/" + transform.name + "Data");
        Debug.LogWarning(transform.name + " : LoadSkillData");
    }
    protected virtual void LoadAnimator()
    {
        if(this.animator != null) return;
        this.animator=GetComponentInChildren<Animator>();
        Debug.LogWarning(transform.name + " : LoadAnimator");
    }
}
