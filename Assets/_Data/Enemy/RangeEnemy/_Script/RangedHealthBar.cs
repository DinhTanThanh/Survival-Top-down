using UnityEngine;

public class RangedHealthBar : BaseSliderBar, IHealthObserver
{
    [SerializeField] protected RangedBarCanvas rangedBarCanvas;
    [SerializeField] protected DamageReceiver damageReceiver;
    private void Start()
    {
        if (this.damageReceiver == null) return;
        this.damageReceiver.AddHealthObserver(this);
    }
    protected override void LoadComponent()
    {
        base.LoadComponent();
        this.LoadRangedBarCanvas();
        this.LoadDamageReceiver();
    }
    protected virtual void LoadRangedBarCanvas()
    {
        if (this.rangedBarCanvas != null) return;
        this.rangedBarCanvas = GetComponentInParent<RangedBarCanvas>();
        Debug.LogWarning(transform.name + " : LoadRangedBarCanvas");
    }
    protected virtual void LoadDamageReceiver()
    {
        if (this.rangedBarCanvas == null) return;
        if (this.damageReceiver != null) return;
        this.damageReceiver = this.rangedBarCanvas.RangedEnemyController?.DamageReceiver;
        Debug.LogWarning(transform.name + " : LoadDamageReceiver");
    }
    public void UpdateHealthHp()
    {
        this.slider.value = this.damageReceiver.GetHp() / this.damageReceiver.GetBaseHp();
    }
}
