using UnityEngine;

public class MinionnMoving : LoadMonoBehaviour
{
    [SerializeField] protected float stoppingDistance = 0.2f;
    [SerializeField] protected float attackRange;
    [SerializeField] protected float speedMovement;
    [SerializeField] protected float speedRotation;
    [SerializeField] protected float separationRadius = 2f;
    [SerializeField] protected float separationWeight = 1.8f;
    [SerializeField] protected Transform target;
    [SerializeField] protected MinionController controller;
    protected Vector3 targetOffset;
    protected Vector3 randomOffset;

    protected override void LoadComponent()
    {
        base.LoadComponent();
        this.LoadMinionController();
        if (this.controller != null && this.controller.EntitySO != null)
        {
            this.SetAttackRange(this.controller.EntitySO.attackRange);
            this.SetSpeedRotation(this.controller.EntitySO.baseRotation);
            this.SetSpeedMovement(this.controller.EntitySO.baseSpeed);
        }
    }
    protected override void Awake()
    {
        this.randomOffset = Random.insideUnitSphere * 2.5f;
        this.randomOffset.y = 0;
    }
    public virtual Vector3 GetDestination()
    {
        Vector3 currentPos = this.controller != null ? this.controller.transform.position : transform.position;
        if (this.target == null) return currentPos;
        return this.target.position + this.targetOffset;
    }

    public virtual bool IsReachedTargetLimit()
    {
        if (this.target == null) return true;
        Vector3 destination = this.GetDestination();
        Vector3 currentPos = this.controller != null ? this.controller.transform.position : transform.position;
        return Vector3.Distance(destination, currentPos) <= this.stoppingDistance;
    }

    public virtual void Moving()
    {
        if (this.target == null || this.controller == null || this.controller.Rb == null) return;
        Vector3 destination = this.GetDestination();
        Vector3 currentPos = this.controller.transform.position;
        Vector3 directionToDestination = destination - currentPos;
        directionToDestination.y = 0f;

        Vector3 separation = this.CalculateSeparation();
        Vector3 desiredDirection = directionToDestination.normalized + separation * this.separationWeight;
        desiredDirection.y = 0f;

        if (desiredDirection.sqrMagnitude > 0.01f)
        {
            desiredDirection.Normalize();
            Quaternion targetRotation = Quaternion.LookRotation(desiredDirection);
            Quaternion newRotation = Quaternion.RotateTowards(this.controller.Rb.rotation, targetRotation, this.speedRotation * Time.fixedDeltaTime);
            this.controller.Rb.MoveRotation(newRotation);

            Vector3 moveDirection = this.controller.Rb.rotation * Vector3.forward;
            Vector3 newDelta = moveDirection * (this.speedMovement * Time.fixedDeltaTime);
            this.controller.Rb.MovePosition(this.controller.Rb.position + newDelta);
        }
        else
        {
            this.FaceTarget();
        }
    }

    protected virtual Vector3 CalculateSeparation()
    {
        Vector3 currentPos = this.controller != null ? this.controller.transform.position : transform.position;
        Vector3 separation = Vector3.zero;
        int neighborCount = 0;

        Collider[] colliders = Physics.OverlapSphere(currentPos, this.separationRadius);
        foreach (Collider col in colliders)
        {
            if (col == null) continue;
            if (this.controller != null && (col.transform == this.controller.transform || col.transform.IsChildOf(this.controller.transform))) continue;

            MinionController otherMinion = col.GetComponentInParent<MinionController>();
            if (otherMinion != null && otherMinion != this.controller)
            {
                Vector3 diff = currentPos - otherMinion.transform.position;
                diff.y = 0f;
                float distance = diff.magnitude;
                if (distance < this.separationRadius)
                {
                    if (distance <= 0.001f)
                    {
                        diff = new Vector3(Random.Range(-1f, 1f), 0f, Random.Range(-1f, 1f)).normalized;
                        distance = 0.001f;
                    }
                    float strength = 1f - (distance / this.separationRadius);
                    separation += diff.normalized * strength;
                    neighborCount++;
                }
            }
        }

        if (neighborCount > 0)
        {
            separation /= neighborCount;
        }

        return separation;
    }

    public virtual void FaceTarget()
    {
        if (this.target == null || this.controller == null || this.controller.Rb == null) return;
        Vector3 currentPos = this.controller.transform.position;
        Vector3 directionToTarget = this.target.position - currentPos;
        directionToTarget.y = 0f;
        if (directionToTarget.sqrMagnitude > 0.001f)
        {
            Quaternion faceTarget = Quaternion.LookRotation(directionToTarget);
            Quaternion newRotation = Quaternion.RotateTowards(this.controller.Rb.rotation, faceTarget, this.speedRotation * Time.fixedDeltaTime);
            this.controller.Rb.MoveRotation(newRotation);
        }
    }

    public virtual void SetTarget(Transform target)
    {
        this.target = target;
    }
    public virtual void SetTargetOffset(Vector3 targetOffset)
    {
        this.targetOffset = targetOffset;
    }
    protected virtual void SetAttackRange(float attackRange)
    {
        this.attackRange = attackRange;
    }

    protected virtual void SetSpeedRotation(float speedRotation)
    {
        this.speedRotation = speedRotation;
    }

    public virtual void SetSpeedMovement(float speedMovement)
    {
        this.speedMovement = speedMovement;
    }
    public virtual Vector3 GetRandomOffset()
    {
        return this.randomOffset;
    }
    protected virtual void LoadMinionController()
    {
        if (this.controller != null) return;
        this.controller = GetComponentInParent<MinionController>();
        Debug.LogWarning(transform.name + " : LoadMinionController");
    }
}
