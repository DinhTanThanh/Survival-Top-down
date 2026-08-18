using UnityEngine;

public class RangedBarCanvas : LoadMonoBehaviour
{
    [SerializeField] protected RangedEnemyController rangedEnemyController;
    public RangedEnemyController RangedEnemyController => rangedEnemyController;
    protected override void LoadComponent()
    {
        base.LoadComponent();
        this.LoadRangedEnemyController();
    }
    protected virtual void LoadRangedEnemyController()
    {
        if (this.rangedEnemyController != null) return;
        this.rangedEnemyController = GetComponentInParent<RangedEnemyController>();
        Debug.LogWarning(transform.name + " : LoadRangedEnemyController");
    }
}
