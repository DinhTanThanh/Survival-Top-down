using UnityEngine;

public class RangedBarCanvas : LoadMonoBehaviour
{
    [SerializeField] protected RangedEnemyController rangedEnemyController;
    [SerializeField] protected Quaternion fixedRotation;
    [SerializeField] protected Vector3 fixedWorldOffset;
    [SerializeField] protected bool isLockRotation = true;
    [SerializeField] protected bool faceCamera = false;

    public RangedEnemyController RangedEnemyController => rangedEnemyController;

    protected override void Awake()
    {
        base.Awake();
        this.InitFixedRotation();
    }

    protected virtual void Start()
    {
        this.InitFixedRotation();
    }

    protected virtual void InitFixedRotation()
    {
        if (this.fixedRotation.w == 0f && this.fixedRotation.x == 0f && this.fixedRotation.y == 0f && this.fixedRotation.z == 0f)
        {
            this.fixedRotation = transform.rotation;
        }

        if (this.rangedEnemyController != null && this.fixedWorldOffset == Vector3.zero)
        {
            this.fixedWorldOffset = transform.position - this.rangedEnemyController.transform.position;
        }
    }

    protected override void LoadComponent()
    {
        base.LoadComponent();
        this.LoadRangedEnemyController();
    }

    protected virtual void LoadRangedEnemyController()
    {
        if (this.rangedEnemyController != null) return;
        this.rangedEnemyController = GetComponentInParent<RangedEnemyController>();
        Debug.LogWarning(transform.name + " : LoadRangedEnemyController");
    }

    protected virtual void LateUpdate()
    {
        this.LockRotation();
    }

    protected virtual void LockRotation()
    {
        if (!this.isLockRotation) return;

        if (this.rangedEnemyController != null)
        {
            transform.position = this.rangedEnemyController.transform.position + this.fixedWorldOffset;
        }

        if (this.faceCamera && Camera.main != null)
        {
            transform.rotation = Camera.main.transform.rotation;
        }
        else
        {
            transform.rotation = this.fixedRotation;
        }
    }
}
