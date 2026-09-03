using UnityEngine;

public class MinionHealthBar : BaseSliderBar, IHealthObserver
{
    [SerializeField] protected MinionBarCanvas minionBarCanvas;
    [SerializeField] protected DamageReceiver damageReceiver;
    private void OnEnable()
    {
        this.damageReceiver.Reborn();
        this.UpdateHealthHp();
    }
    private void Start()
    {
        if (this.damageReceiver == null) return;
        this.damageReceiver.AddHealthObserver(this);
    }
    protected override void LoadComponent()
    {
        base.LoadComponent();
        this.LoadMinionBarCanvas();
        this.LoadDamageReceiver();
    }
    protected virtual void LoadMinionBarCanvas()
    {
        if (this.minionBarCanvas != null) return;
        this.minionBarCanvas = GetComponentInParent<MinionBarCanvas>();
        Debug.LogWarning(transform.name + " : LoadMinionBarCanvas");
    }
    protected virtual void LoadDamageReceiver()
    {
        if (this.minionBarCanvas == null) return;
        if (this.damageReceiver != null) return;
        this.damageReceiver = this.minionBarCanvas.MinionController?.DamageReceiver;
        Debug.LogWarning(transform.name + " : LoadMinionController");
    }
    public void UpdateHealthHp()
    {
        this.slider.value = this.damageReceiver.GetHp() / this.damageReceiver.GetBaseHp();
    }
}
