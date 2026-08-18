using UnityEngine;

public class PlayerChargeBar : BaseSliderBar,IChargeObserver
{
    [SerializeField] protected PlayerBarCanvas playerBarCanvas;
    [SerializeField] protected PlayerShooting playerShooting;
    private void Start()
    {
        if (this.playerShooting == null) return;
        this.playerShooting.AddChargeObserver(this);
        this.playerShooting.BaseChargeSystem.AddChargeObserver(this);
    }
    protected override void LoadComponent()
    {
        base.LoadComponent();
        this.LoadPlayerBarCanvas();
        this.LoadPlayerShooting();
    }
    protected virtual void LoadPlayerBarCanvas()
    {
        if (this.playerBarCanvas != null) return;
        this.playerBarCanvas = GetComponentInParent<PlayerBarCanvas>();
        Debug.LogWarning(transform.name + " : LoadPlayerBarCanvas");
    }
    protected virtual void LoadPlayerShooting()
    {
        if (this.playerBarCanvas == null) return;
        if (this.playerShooting != null) return;
        this.playerShooting = this.playerBarCanvas.PlayerController?.PlayerShooting;
        Debug.LogWarning(transform.name + " : LoadPlayerShooting");
    }
    public void UpdateCharge()
    {
        this.slider.value = (float)this.playerShooting.GetCurrentCharge() / this.playerShooting.GetMaxCharge();
    }
}
