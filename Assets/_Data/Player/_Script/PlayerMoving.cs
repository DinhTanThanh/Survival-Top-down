using UnityEngine;

public class PlayerMoving : LoadMonoBehaviour
{
    [SerializeField] protected Rigidbody rb;
    [SerializeField] protected float horizontal;
    [SerializeField] protected float vertical;
    [SerializeField] protected float speedMovement;
    [SerializeField] protected float speedRotation;
    [SerializeField] protected PlayerController playerController;
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
        this.playerController= GetComponentInParent<PlayerController>();
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
    }
    private void FixedUpdate()
    {
        this.Moving();
    }
    protected virtual void Moving()
    {
        Transform camTransform = Camera.main.transform;
        Vector3 camForward = camTransform.forward;
        Vector3 camRight = camTransform.right;  

        camForward.y = 0f;
        camRight.y = 0f;

        Vector3 movementPosition = (camForward * this.vertical) + (camRight * this.horizontal);
        if (movementPosition != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(movementPosition);
            this.transform.parent.rotation = Quaternion.RotateTowards(this.transform.parent.rotation, targetRotation, this.speedRotation * Time.fixedDeltaTime);
        }
        this.transform.parent.Translate(movementPosition * this.speedMovement * Time.fixedDeltaTime, Space.World);
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
