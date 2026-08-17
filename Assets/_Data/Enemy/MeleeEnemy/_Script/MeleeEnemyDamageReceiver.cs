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
        this.SetExperienceReward(this.meleeEnemyController.EntitySO.expReward);
    }
    protected virtual void LoadMeleeEnemyController()
    {
        if (this.meleeEnemyController != null) return;
        this.meleeEnemyController = GetComponentInParent<MeleeEnemyController>();
        Debug.LogWarning(transform.name + " : LoadMeleeEnemyController");
    }
    protected override void Update()
    {
        if (this.isExecuteDead) return;
        if (!this.isDead) return;
        this.baseEntityController.Animator.SetTrigger("Dead");
        this.meleeEnemyController.PlayerController.PlayerLevel.AddExpReward(this.expReward);
        this.isExecuteDead = true;
        this.meleeEnemyController.WaveSpawnManager.ReduceNumberMeleeEnemyOnScene();
    }
}
