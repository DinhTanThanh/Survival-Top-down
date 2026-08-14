using UnityEngine;

public class Movement : LoadMonoBehaviour
{
    [SerializeField] protected float speedMovement;
    [SerializeField] protected Rigidbody rb;
    private void OnEnable()
    {
        this.rb.linearVelocity = this.speedMovement * transform.forward;
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
