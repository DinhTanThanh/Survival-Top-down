using UnityEngine;

public class BulletMoving : Movement
{
    [SerializeField] protected BulletController bulletController;
    [SerializeField] protected float initialUpwardSpeed = 2f;
    [SerializeField] protected float gravity = 12f;
    protected Vector3 currentVelocity;
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
    protected virtual void OnEnable()
    {
        this.currentVelocity = (transform.forward * this.speedMovement) + (Vector3.up * this.initialUpwardSpeed);
    }
    protected virtual void FixedUpdate()
    {
        if (this.rb == null) return;
        this.currentVelocity.y -= this.gravity * Time.fixedDeltaTime;
        Vector3 moveDelta = this.currentVelocity * Time.fixedDeltaTime;
        this.rb.MovePosition(this.rb.position + moveDelta);
        if (this.currentVelocity != Vector3.zero)
        {
            this.rb.MoveRotation(Quaternion.LookRotation(this.currentVelocity));
        }
    }
}
