using UnityEngine;

public class MinionHitBox : LoadMonoBehaviour
{
    [SerializeField] protected MinionHandRight minionHandRight;
    [SerializeField] protected MinionHandLeft minionHandLeft;
    [SerializeField] protected MinionController minionController;
    protected override void LoadComponent()
    {
        base.LoadComponent();
        this.LoadMeleeEnemyController();
        this.LoadMinionHandRight();
        this.LoadMinionHandLeft();
    }
    protected virtual void LoadMeleeEnemyController()
    {
        if (this.minionController != null) return;
        this.minionController = GetComponentInParent<MinionController>();
        Debug.LogWarning(transform.name + " : LoadMinionController");
    }
    protected virtual void LoadMinionHandRight()
    {
        if (this.minionHandRight != null) return;
        this.minionHandRight = GetComponentInChildren<MinionHandRight>();
        Debug.LogWarning(transform.name + " : MinionHandRight");
    }
    protected virtual void LoadMinionHandLeft()
    {
        if (this.minionHandLeft != null) return;
        this.minionHandLeft = GetComponentInChildren<MinionHandLeft>();
        Debug.LogWarning(transform.name + " : MinionHandLeft");
    }
    public virtual void EnableColliderHandRight()
    {
        if (this.minionHandRight.Collider == null) return;
        this.minionHandRight.Collider.enabled = true;
    }
    public virtual void DisableColliderHandRight()
    {
        if (this.minionHandRight.Collider == null) return;
        this.minionHandRight.Collider.enabled = false;
    }
    public virtual void EnableColliderHandLeft()
    {
        if (this.minionHandLeft.Collider == null) return;
        this.minionHandLeft.Collider.enabled = true;
    }
    public virtual void DisableColliderHandLeft()
    {
        if (this.minionHandLeft.Collider == null) return;
        this.minionHandLeft.Collider.enabled = false;
    }
    protected virtual void EnableCollider(Collider colliderr)
    {
        colliderr.enabled = true;
    }
    protected virtual void DisableCollider(Collider colliderr)
    {
        colliderr.enabled = false;
    }
}
