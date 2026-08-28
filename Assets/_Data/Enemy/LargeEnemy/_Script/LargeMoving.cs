using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LargeMoving : LoadMonoBehaviour
{
    [SerializeField] protected float timer;
    [SerializeField] protected float timeDelay;
    [SerializeField] protected float minximumDistance;
    [SerializeField] protected float maximumDistance;
    [SerializeField] protected float speedMovement;
    [SerializeField] protected float speedRotation;
    [SerializeField] protected float speedStrafe;
    [SerializeField] protected float directionStrafe;
    [SerializeField] protected Transform player;
    [SerializeField] protected Rigidbody rb;
    [SerializeField] protected BaseEnemyController controller;
    protected override void LoadComponent()
    {
        base.LoadComponent();
        this.SetTimeDelay(4f);
        this.SetDirectionStrafe(1f);
        this.LoadRigidbody();
        this.LoadEntityController();
        this.LoadPlayer();
        this.SetMinximumDistance(this.controller.EnemySO.minximumDistance);
        this.SetMaximumDistance(this.controller.EnemySO.maximumDistance);
        this.SetSpeedMovement(this.controller.EnemySO.baseSpeed);
        this.SetSpeedRotation(this.controller.EnemySO.baseRotation);
        this.SetSpeedStrafe(this.controller.EnemySO.strafeMovement);
    }
    private void Update()
    {
        this.Timing();
    }
    private void FixedUpdate()
    {
        if (this.player == null) return;
        this.HandleMovement();
    }
    protected virtual void HandleMovement()
    {
        Vector3 directionToPlayer = this.player.position - this.transform.parent.position;
        float distance = directionToPlayer.magnitude;
        directionToPlayer.y = 0f;
        Quaternion rotation = Quaternion.LookRotation(directionToPlayer);
        Quaternion newRotation= Quaternion.RotateTowards(this.rb.rotation, rotation, this.speedRotation * Time.fixedDeltaTime);
        this.rb.MoveRotation(newRotation);
        if (distance > this.maximumDistance)
        {
            this.ApproadToPlayer(directionToPlayer);
        }
        else if (distance < this.minximumDistance)
        {
            this.RetreatFromPlayer(directionToPlayer);
        }
        else
        {
            this.CircleStrafe(directionToPlayer);
        }
    }
    protected virtual void RetreatFromPlayer(Vector3 directionToPlayer)
    {
        Vector3 direction = -directionToPlayer.normalized;
        this.Moving(direction, this.speedMovement);
    }
    protected virtual void ApproadToPlayer(Vector3 directionToPlayer)
    {
        Vector3 direction = directionToPlayer.normalized;
        this.Moving(direction,this.speedMovement);
    }
    protected virtual void CircleStrafe(Vector3 directionToPlayer)
    {
        Vector3 direction = directionToPlayer.normalized;
        Vector3 directionStrafe = Vector3.Cross(Vector3.up, direction);
        directionStrafe *= this.directionStrafe;
        this.Moving(directionStrafe, this.speedStrafe);
    }
    protected virtual void Moving(Vector3 directionToPlayer,float speed)
    {
        Vector3 movement = directionToPlayer*speed*Time.fixedDeltaTime;
        this.rb.MovePosition(this.rb.position + movement);
    }
    protected virtual void Timing()
    {
        this.timer += Time.deltaTime;
        if (this.timer < this.timeDelay) return;
        this.timer = 0;
        this.directionStrafe *= -1f;
    }
    protected virtual void LoadRigidbody()
    {
        if (this.rb != null) return;
        this.rb = GetComponentInParent<Rigidbody>();
        Debug.LogWarning(transform.name + " : LoadRigidbody");
    }
    protected virtual void LoadEntityController()
    {
        if (this.controller != null) return;
        this.controller = GetComponentInParent<BaseEnemyController>();
        Debug.LogWarning(transform.name + " : LoadEntityController");
    }
    protected virtual void LoadPlayer()
    {
        if (this.player != null) return;
        this.player = GameObject.Find("Player")?.transform;
        Debug.LogWarning(transform.name + " : LoadPlayer");
    }
    protected virtual void SetTimeDelay(float timeDelay)
    {
        this.timeDelay = timeDelay;
    }
    protected virtual void SetMinximumDistance(float minximumDistance)
    {
        this.minximumDistance = minximumDistance;
    }
    protected virtual void SetMaximumDistance(float maximumDistance)
    {
        this.maximumDistance = maximumDistance;
    }
    protected virtual void SetSpeedMovement(float speedMovement)
    {
        this.speedMovement = speedMovement;
    }
    protected virtual void SetDirectionStrafe(float directionStrafe)
    {
        this.directionStrafe = directionStrafe;
    }
    protected virtual void SetSpeedStrafe(float speedStrafe)
    {
        this.speedStrafe = speedStrafe;
    }
    protected virtual void SetSpeedRotation(float speedRotation)
    {
        this.speedRotation = speedRotation;
    }
}
