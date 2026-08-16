using UnityEngine;

public class MeleeEnemyDamageReceiver : DamageReceiver
{
    [SerializeField] protected MeleeEnemyController meleeEnemyController;
    private void OnEnable()
    {
        this.Reborn();
    }
    protected override void LoadComponent()
    {
        base.LoadComponent();
        this.LoadMeleeEnemyController();
        this.SetBaseHp(this.meleeEnemyController.EntitySO.baseHp);
        this.SetHp(this.meleeEnemyController.EntitySO.baseHp);
        this.SetDefence(this.meleeEnemyController.EntitySO.baseDefence);
    }
    protected virtual void LoadMeleeEnemyController()
    {
        if (this.meleeEnemyController != null) return;
        this.meleeEnemyController = GetComponentInParent<MeleeEnemyController>();
        Debug.LogWarning(transform.name + " : LoadMeleeEnemyController");
    }
}
