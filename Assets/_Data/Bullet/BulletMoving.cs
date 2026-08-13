using UnityEngine;

public class BulletMoving : LoadMonoBehaviour
{
    [SerializeField] protected float speedMovement;
    [SerializeField] protected Rigidbody rb;
    private void OnEnable()
    {
        this.rb.linearVelocity = this.speedMovement * transform.forward;
    }
    protected override void LoadComponent()
    {
        base.LoadComponent();
        this.SetSpeedMovement(2f);
        this.LoadRigidbody();
    }
    protected virtual void LoadRigidbody()
    {
        if (this.rb != null) return;
        this.rb = GetComponentInParent<Rigidbody>();
        Debug.LogWarning(transform.name + " : LoadRigidbody");
    }
    protected virtual void SetSpeedMovement(float speedMovement)
    {
        this.speedMovement = speedMovement;
    }

}
