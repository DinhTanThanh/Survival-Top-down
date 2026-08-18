using UnityEngine;

public class MeleeEnemyBarCanvas : LoadMonoBehaviour
{
    [SerializeField] protected MeleeEnemyController meleeEnemyController;
    public MeleeEnemyController MeleeEnemyController => meleeEnemyController;
    protected override void LoadComponent()
    {
        base.LoadComponent();
        this.LoadMeleeEnemyController();
    }
    protected virtual void LoadMeleeEnemyController()
    {
        if (this.meleeEnemyController != null) return;
        this.meleeEnemyController = GetComponentInParent<MeleeEnemyController>();
        Debug.LogWarning(transform.name + " : LoadMeleeEnemyController");
    }
}
