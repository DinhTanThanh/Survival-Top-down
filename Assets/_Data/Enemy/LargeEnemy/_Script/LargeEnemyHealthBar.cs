using UnityEngine;

public class LargeEnemyHealthBar : BaseSliderBar, IHealthObserver
{
    [SerializeField] protected LargeEnemyBarCanvas largeEnemyBarCanvas;
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
        this.LoadLargeEnemyBarCanvas();
        this.LoadDamageReceiver();
    }
    protected virtual void LoadLargeEnemyBarCanvas()
    {
        if (this.largeEnemyBarCanvas != null) return;
        this.largeEnemyBarCanvas = GetComponentInParent<LargeEnemyBarCanvas>();
        Debug.LogWarning(transform.name + " : LoadLargeEnemyBarCanvas");
    }
    protected virtual void LoadDamageReceiver()
    {
        if (this.largeEnemyBarCanvas == null) return;
        if (this.damageReceiver != null) return;
        this.damageReceiver = this.largeEnemyBarCanvas.BaseEntityController?.DamageReceiver;
        Debug.LogWarning(transform.name + " : LoadDamageReceiver");
    }
    public void UpdateHealthHp()
    {
        this.slider.value = this.damageReceiver.GetHp() / this.damageReceiver.GetBaseHp();
    }
}
