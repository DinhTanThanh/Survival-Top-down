using UnityEngine;
using UnityEngine.UI;

public class LargeEnemyHealthBar : BaseSliderBar, IHealthObserver
{
    [SerializeField] protected LargeEnemyBarCanvas largeEnemyBarCanvas;
    [SerializeField] protected DamageReceiver damageReceiver;

    private void OnEnable()
    {
        this.RebindDamageReceiver();
        if (this.damageReceiver != null)
        {
            this.damageReceiver.AddHealthObserver(this);
            this.UpdateHealthHp();
        }
    }

    private void Start()
    {
        this.RebindDamageReceiver();
        if (this.damageReceiver != null)
        {
            this.damageReceiver.AddHealthObserver(this);
            this.UpdateHealthHp();
        }
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

    public virtual void RebindDamageReceiver()
    {
        DamageReceiver localDR = GetComponentInParent<BaseEntityController>()?.DamageReceiver;
        if (localDR == null && transform.root != null)
        {
            localDR = transform.root.GetComponentInChildren<DamageReceiver>();
        }
        if (localDR == null)
        {
            localDR = GetComponentInParent<DamageReceiver>();
        }

        if (localDR != null)
        {
            this.damageReceiver = localDR;
        }
        else if (this.largeEnemyBarCanvas != null && this.largeEnemyBarCanvas.BaseEntityController != null)
        {
            this.damageReceiver = this.largeEnemyBarCanvas.BaseEntityController.DamageReceiver;
        }
    }

    protected virtual void LoadDamageReceiver()
    {
        this.RebindDamageReceiver();
    }

    public void UpdateHealthHp()
    {
        if (this.slider == null)
        {
            this.slider = GetComponent<Slider>();
            if (this.slider == null) this.slider = GetComponentInChildren<Slider>();
        }

        if (this.damageReceiver == null)
        {
            this.RebindDamageReceiver();
        }

        if (this.damageReceiver == null || this.slider == null) return;

        float baseHp = this.damageReceiver.GetBaseHp();
        if (baseHp <= 0) return;

        float hp = this.damageReceiver.GetHp();
        this.slider.value = hp / baseHp;
    }
}
