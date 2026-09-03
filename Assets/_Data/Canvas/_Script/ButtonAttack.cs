using UnityEngine;
using UnityEngine.InputSystem;

public class ButtonAttack : BaseButton
{
    [SerializeField] protected GameObject indicator;
    [SerializeField] protected GameObject rangeIndicator;
    protected override void LoadComponent()
    {
        base.LoadComponent();
        this.LoadIndicator();
        this.LoadRangIndicator();
    }
    protected virtual void LoadIndicator()
    {
        if (this.indicator != null) return;
        this.indicator = GameObject.Find("Indicator");
        Debug.LogWarning(transform.name + " : LoadIndicator");
    }
    protected virtual void LoadRangIndicator()
    {
        if (this.indicator == null) return;
        if (this.rangeIndicator != null) return;
        this.rangeIndicator = this.indicator.transform.Find("RangeIndicator")?.gameObject;
        Debug.LogWarning(transform.name + " : LoadRangeIndicator");
    }
    public virtual void ExecuteAttack()
    {
    }
    public virtual void EnableRangeIndicator()
    {
        this.rangeIndicator.SetActive(true);
        InputSystem.Instance.SetIsAttack(true);
    }
    public virtual void DisableRangeIndicator()
    {
        this.rangeIndicator.SetActive(false);
    }
}
