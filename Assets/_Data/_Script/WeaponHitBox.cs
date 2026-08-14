using UnityEngine;

public class WeaponHitBox : LoadMonoBehaviour
{
    [SerializeField] protected Collider colliderr;
    [SerializeField] protected MeleeEnemyController meleeEnemyController;
    protected override void LoadComponent()
    {
        base.LoadComponent();
        this.LoadCollider();
        this.LoadMeleeEnemyController();
    }
    protected virtual void LoadMeleeEnemyController()
    {
        if (this.meleeEnemyController != null) return;
        this.meleeEnemyController=GetComponentInParent<MeleeEnemyController>();
        Debug.LogWarning(transform.name + " : LoadMeleeEnemyController");
    }
    protected virtual void LoadCollider()
    {
        if (this.colliderr != null) return;
        this.colliderr = GetComponentInChildren<BoxCollider>();
        Debug.LogWarning(transform.name + " : LoadCollider");
    }
    public virtual void EnableCollider()
    {
        this.colliderr.enabled = true;
    }
    public virtual void DisableCollider()
    {
        this.colliderr.enabled = false;
        this.meleeEnemyController.Animator.SetBool("IsAttack", false);
        Debug.Log("thuc hien");
    }
}
