using UnityEngine;
using UnityEngine.UI;

public class MinionHealthBar : BaseSliderBar, IHealthObserver
{
    [SerializeField] protected MinionBarCanvas minionBarCanvas;
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
        this.LoadMinionBarCanvas();
        this.LoadDamageReceiver();
    }

    protected virtual void LoadMinionBarCanvas()
    {
        if (this.minionBarCanvas != null) return;
        this.minionBarCanvas = GetComponentInParent<MinionBarCanvas>();
        Debug.LogWarning(transform.name + " : LoadMinionBarCanvas");
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
        else if (this.minionBarCanvas != null)
        {
            if (this.minionBarCanvas.MinionController != null)
            {
                this.damageReceiver = this.minionBarCanvas.MinionController.DamageReceiver;
            }
            else if (this.minionBarCanvas.BaseEntityController != null)
            {
                this.damageReceiver = this.minionBarCanvas.BaseEntityController.DamageReceiver;
            }
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
