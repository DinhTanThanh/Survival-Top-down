using UnityEngine;
public class ProjectileMoving : Movement
{
    [SerializeField] protected ProjectileController projectileController;
    protected override void LoadComponent()
    {
        base.LoadComponent();
        this.LoadProjectileController();
        this.SetSpeedMovement(this.projectileController.WeaponData.moveSpeed);
        this.LoadRigidbody();
    }
    protected virtual void LoadProjectileController()
    {
        if (this.projectileController != null) return;
        this.projectileController = GetComponentInParent<ProjectileController>();
        Debug.LogWarning(transform.name + " : LoadProjectileController");
    }
}
