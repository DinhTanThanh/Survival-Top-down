using UnityEngine;

public class MeleeEnemyHealthBar : BaseSliderBar, IHealthObserver
{
    [SerializeField] protected MeleeEnemyBarCanvas meleeEnemyBarCanvas;
    [SerializeField] protected DamageReceiver damageReceiver;
    private void Start()
    {
        if (this.damageReceiver == null) return;
        this.damageReceiver.AddHealthObserver(this);
    }
    protected override void LoadComponent()
    {
        base.LoadComponent();
        this.LoadMeleeEnemyBarCanvas();
        this.LoadDamageReceiver();
    }
    protected virtual void LoadMeleeEnemyBarCanvas()
    {
        if (this.meleeEnemyBarCanvas != null) return;
        this.meleeEnemyBarCanvas = GetComponentInParent<MeleeEnemyBarCanvas>();
        Debug.LogWarning(transform.name + " : LoadMeleeEnemyBarCanvas");
    }
    protected virtual void LoadDamageReceiver()
    {
        if (this.meleeEnemyBarCanvas == null) return;
        if (this.damageReceiver != null) return;
        this.damageReceiver = this.meleeEnemyBarCanvas.MeleeEnemyController?.DamageReceiver;
        Debug.LogWarning(transform.name + " : LoadDamageReceiver");
    }
    public void UpdateHealthHp()
    {
        this.slider.value = this.damageReceiver.GetHp() / this.damageReceiver.GetBaseHp();
        Debug.Log("Update ne");
    }
}
