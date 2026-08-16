using UnityEngine;

public class RangedEnemyDamageReceiver : DamageReceiver
{
    [SerializeField] protected RangedEnemyController rangeEnemyController;
    private void OnEnable()
    {
        this.Reborn();
    }
    protected override void LoadComponent()
    {
        base.LoadComponent();
        this.LoadRangeEnemyController();
        this.SetBaseHp(this.rangeEnemyController.EntitySO.baseHp);
        this.SetHp(this.rangeEnemyController.EntitySO.baseHp);
        this.SetDefence(this.rangeEnemyController.EntitySO.baseDefence);
    }
    protected virtual void LoadRangeEnemyController()
    {
        if (this.rangeEnemyController != null) return;
        this.rangeEnemyController = GetComponentInParent<RangedEnemyController>();
        Debug.LogWarning(transform.name + " : LoadRangeEnemyController");
    }
}
