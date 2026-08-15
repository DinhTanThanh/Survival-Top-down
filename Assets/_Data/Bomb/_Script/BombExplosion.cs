using Unity.VisualScripting;
using UnityEngine;

public class BombExplosion : LoadMonoBehaviour
{
    [SerializeField] protected float timer;
    [SerializeField] protected float timeDelay;
    [SerializeField] protected bool isExplosion;
    [SerializeField] protected BombSkillController bombSkillController;
    protected override void LoadComponent()
    {
        base.LoadComponent();
        this.LoadBombSkillController();
        this.SetTimeDelay(this.bombSkillController.SkillData.TimeDelay);
    }
    private void OnEnable()
    {
        this.isExplosion = false;
    }
    private void Update()
    {
        if (this.isExplosion) return;
        if (!this.Timing()) return;
        this.bombSkillController.Animator.SetTrigger("Attack");
        this.isExplosion = true;
    }
    protected virtual void LoadBombSkillController()
    {
        if (this.bombSkillController != null) return;
        this.bombSkillController=GetComponentInParent<BombSkillController>();
        Debug.LogWarning(transform.name + " : LoadBombSkillController");
    }
    protected virtual void SetTimeDelay(float timeDelay)
    {
        this.timeDelay = timeDelay;
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 5);
    }
    protected virtual bool Timing()
    {
        this.timer += Time.deltaTime;
        if (this.timer < this.timeDelay) return false;
        this.timer = 0f;
        return true;
    }
}
