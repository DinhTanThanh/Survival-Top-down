using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonAttack : BaseButton
{
    [SerializeField] protected bool isAttack;
    [SerializeField] protected GameObject rangeIndicator;
    public bool IsAttack => isAttack;
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
       
        Debug.Log("ExecuteAttack: " + isAttack);
    }
    public virtual void EnableRangeIndicator()
    {
        this.rangeIndicator.SetActive(true);
        this.SetIsAttack(true);
        Debug.Log("EnableRangeIndicator: " + isAttack);
    }
    public virtual void DisableRangeIndicator()
    {
        this.SetIsAttack(false);
        this.rangeIndicator.SetActive(false);
        Debug.Log("thuc hien");
        Debug.Log(isAttack);
    }
    public virtual void SetIsAttack(bool isAttack)
    {
        this.isAttack = isAttack;
    }
}
