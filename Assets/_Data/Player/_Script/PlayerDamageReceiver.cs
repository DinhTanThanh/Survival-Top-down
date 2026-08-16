using UnityEngine;

public class PlayerDamageReceiver : DamageReceiver
{
    [SerializeField] protected bool isExecuteDead;
    [SerializeField] protected PlayerController playerController;
    protected override void LoadComponent()
    {
        base.LoadComponent();
        this.LoadPlayerController();
        this.SetBaseHp(this.playerController.EntitySO.baseHp);
        this.SetHp(this.playerController.EntitySO.baseHp);
        this.SetDefence(this.playerController.EntitySO.baseDefence);
    }
    protected virtual void LoadPlayerController()
    {
        if (this.playerController != null) return;
        this.playerController = GetComponentInParent<PlayerController>();
        Debug.LogWarning(transform.name + " : LoadPlayerController");
    }
    private void Update()
    {
        if (this.isDead) return;
        if (!this.isDead) return;
        this.playerController.Animator.SetBool("IsDead", true);
        this.isExecuteDead = true;
    }
}
