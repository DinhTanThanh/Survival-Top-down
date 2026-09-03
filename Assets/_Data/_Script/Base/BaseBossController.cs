using UnityEngine;

public class BaseBossController : BaseEnemyController
{
    [SerializeField] protected BossMinionSpawner minionSpawner;
    public BossMinionSpawner MinionSpawner => minionSpawner;
    protected override void LoadComponent()
    {
        base.LoadComponent();
        this.LoadMinionSpawner();
    }
    protected virtual void LoadMinionSpawner()
    {
        if (this.minionSpawner != null) return;
        this.minionSpawner = GetComponentInChildren<BossMinionSpawner>();
        Debug.LogWarning(transform.name + " : LoadMinionSpawner");
    }
    public virtual void CommandMinionsAttack() => this.minionSpawner?.OrderAttack(this.Target);
    public virtual void CommandMinionsRecall() => this.minionSpawner?.OrderRecall();
    public virtual void CommandMinionsHeal(float amount) => this.minionSpawner?.HealMinions(amount);
}
