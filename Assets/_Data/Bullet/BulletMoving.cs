using UnityEngine;

public class BulletMoving : Movement
{
    [SerializeField] protected BulletController bulletController;
    protected override void LoadComponent()
    {
        base.LoadComponent();
        this.LoadBulletController();
        this.SetSpeedMovement(this.bulletController.WeaponData.moveSpeed);
        this.LoadRigidbody();
    }
    protected virtual void LoadBulletController()
    {
        if (this.bulletController != null) return;
        this.bulletController = GetComponentInParent<BulletController>();
        Debug.LogWarning(transform.name + " : LoadBulletController");
    }
}
