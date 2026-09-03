using UnityEngine;

public class LargeCoolDown : CoolDown
{
    [SerializeField] protected LargeAttack largeAttack;
    protected override void LoadComponent()
    {
        base.LoadComponent();
        this.SetTimeDelay(1f);
        this.LoadLargeAttack();
    }
    private void Update()
    {
        if (!this.largeAttack.GetIsAttack())
        {
            if (this.Timing())
            {
                this.largeAttack.SetIsAttack(true);
            }
        }
    }
    protected virtual void LoadLargeAttack()
    {
        if (this.largeAttack != null) return;
        this.largeAttack = GetComponent<LargeAttack>();
        Debug.LogWarning(transform.name + " : LoadLargeAttack");
    }
}
