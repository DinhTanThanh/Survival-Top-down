using UnityEngine;
using UnityEngine.InputSystem;

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
    }
    public virtual void DisableRangeIndicator()
    {
        this.SetIsAttack(false);
        this.rangeIndicator.SetActive(false);
    }
    public virtual void SetIsAttack(bool isAttack)
    {
        this.isAttack = isAttack;
    }
    private void Update()
    {
        if (Keyboard.current != null)
        {
            if (Keyboard.current.spaceKey.isPressed || Keyboard.current.jKey.isPressed)
            {
                this.isAttack = true;
            }
            else if (Keyboard.current.spaceKey.wasReleasedThisFrame || Keyboard.current.jKey.wasReleasedThisFrame)
            {
                this.isAttack = false;
            }
        }
    }
}
