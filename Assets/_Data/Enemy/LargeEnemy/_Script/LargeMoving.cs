using UnityEngine;

public class LargeMoving : LoadMonoBehaviour
{
    [SerializeField] protected float timer;
    [SerializeField] protected float timeDelay;
    [SerializeField] protected float minximumDistance;
    [SerializeField] protected float maximumDistance;
    [SerializeField] protected float speedMovement;
    [SerializeField] protected float speedRotation;
    [SerializeField] protected float speedStrafe;
    [SerializeField] protected Transform player;
    [SerializeField] protected Rigidbody rb;
    [SerializeField] protected BaseEnemyController controller;
    protected override void LoadComponent()
    {
        base.LoadComponent();
        this.SetTimeDelay(4f);
        this.LoadRigidbody();
        this.LoadEntityController();
        this.LoadPlayer();
        this.SetMinximumDistance(this.controller.EnemySO.minximumDistance);
        this.SetMaximumDistance(this.controller.EnemySO.maximumDistance);
        this.SetSpeedMovement(this.controller.EntitySO.baseSpeed);
        this.SetSpeedRotation(this.controller.EntitySO.baseRotation);
        this.SetSpeedStrafe(this.controller.EnemySO.strafeMovement);
    }
    public virtual void HandleMovement()
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
    protected virtual void Moving(Vector3 directionToPlayer,float speed)
    {
        Vector3 movement = directionToPlayer*speed*Time.fixedDeltaTime;
        this.rb.MovePosition(this.rb.position + movement);
    }
    public virtual bool CheckCanMoving()
    {
        Vector3 directionToPlayer = this.player.position - this.transform.parent.position;
        float distance = directionToPlayer.magnitude;
        if (distance > this.maximumDistance || distance < this.minximumDistance) return true;
        return false;
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
    public virtual void SetMinximumDistance(float minximumDistance)
    {
        this.minximumDistance = minximumDistance;
    }
    public virtual void SetMaximumDistance(float maximumDistance)
    {
        this.maximumDistance = maximumDistance;
    }
    protected virtual void SetSpeedMovement(float speedMovement)
    {
        this.speedMovement = speedMovement;
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
