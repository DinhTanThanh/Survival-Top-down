using UnityEngine;

public class PlayerMoving : LoadMonoBehaviour
{
    [SerializeField] protected float horizontal;
    [SerializeField] protected float vertical;
    [SerializeField] protected float speedMovement;
    [SerializeField] protected float speedRotation;
    [SerializeField] protected Rigidbody rb;
    [SerializeField] protected PlayerController playerController;
    private Vector3 camForward;
    private Vector3 camRight;
    protected override void LoadComponent()
    {
        base.LoadComponent();
        this.LoadRigidbody();
        this.LoadPlayerController();
        this.SetSpeedMovement(this.playerController.EntitySO.baseSpeed);
        this.SetSpeedRotation(this.playerController.EntitySO.baseRotation);
    }
    protected virtual void LoadPlayerController()
    {
        if (this.playerController != null) return;
        this.playerController = GetComponentInParent<PlayerController>();
        Debug.LogWarning(transform.name + " : LoadPlayerController");
    }
    protected virtual void LoadRigidbody()
    {
        if (this.rb != null) return;
        this.rb = GetComponentInParent<Rigidbody>();
        Debug.LogWarning(transform.name + " : LoadRigidbody");
    }
    private void Update()
    {
        this.horizontal = InputSystem.Instance.GetHorizontal();
        this.vertical = InputSystem.Instance.GetVertical();

        Camera mainCam = Camera.main;
        if (mainCam != null)
        {
            Transform camTransform = mainCam.transform;
            this.camForward = camTransform.forward;
            this.camRight = camTransform.right;
            camForward.y = 0f;
            camRight.y = 0f;
            camForward.Normalize();
            camRight.Normalize();
        }
        else
        {
            this.camForward = Vector3.forward;
            this.camRight = Vector3.right;
        }
    }
    public virtual void Moving()
    {
        Vector3 movementPosition = (camForward * this.vertical) + (camRight * this.horizontal);
        if (movementPosition.magnitude > 1f)
        {
            movementPosition.Normalize();
        }

        if (movementPosition.sqrMagnitude > 0.001f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(movementPosition);
            Quaternion newRotation = Quaternion.RotateTowards(this.rb.rotation, targetRotation, this.speedRotation * Time.fixedDeltaTime);
            this.rb.MoveRotation(newRotation);
        }

        Vector3 moveDelta = movementPosition * (this.speedMovement * Time.fixedDeltaTime);
        this.rb.MovePosition(this.rb.position + moveDelta);
    }
    protected virtual void SetSpeedMovement(float speedMovement)
    {
        this.speedMovement = speedMovement;
    }
    protected virtual void SetSpeedRotation(float speedRotation)
    {
        this.speedRotation = speedRotation;
    }
}
