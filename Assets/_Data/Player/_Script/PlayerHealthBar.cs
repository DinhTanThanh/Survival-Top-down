using UnityEngine;

public class PlayerHealthBar : BaseSliderBar,IHealthObserver
{
    [SerializeField] protected PlayerBarCanvas playerBarCanvas;
    [SerializeField] protected DamageReceiver playerDamageReceiver;
    private void Start()
    {
        if (this.playerDamageReceiver == null) return;
        this.playerDamageReceiver.AddHealthObserver(this);
    }
    protected override void LoadComponent()
    {
        base.LoadComponent();
        this.LoadPlayerBarCanvas();
        this.LoadPlayerDamageReceiver();
    }
    protected virtual void LoadPlayerBarCanvas()
    {
        if (this.playerBarCanvas != null) return;
        this.playerBarCanvas=GetComponentInParent<PlayerBarCanvas>();
        Debug.LogWarning(transform.name + " : LoadPlayerBarCanvas");
    }
    protected virtual void LoadPlayerDamageReceiver()
    {
        if (this.playerBarCanvas == null) return;
        if (this.playerDamageReceiver != null) return;
        this.playerDamageReceiver = this.playerBarCanvas.PlayerController?.DamageReceiver;
        Debug.LogWarning(transform.name + " : LoadPlayerDamageReceiver");
    }
    public void UpdateHealthHp()
    {
        this.slider.value = this.playerDamageReceiver.GetHp() / this.playerDamageReceiver.GetBaseHp();
        Debug.Log("Update ne");
    }
}
