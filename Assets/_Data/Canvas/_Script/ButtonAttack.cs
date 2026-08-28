using UnityEngine;
using UnityEngine.InputSystem;

public class ButtonAttack : BaseButton
{
    [SerializeField] protected GameObject rangeIndicator;
    protected override void LoadComponent()
    {
        base.LoadComponent();
        this.LoadRangIndicator();
    }
    protected virtual void LoadRangIndicator()
    {
        if (this.rangeIndicator != null) return;
        this.rangeIndicator = GameObject.Find("RangeIndicator");
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
