using UnityEngine;

public class LargeEnemyBarCanvas : LoadMonoBehaviour
{
    [SerializeField] protected BaseEntityController baseEntityController;
    [SerializeField] protected Quaternion fixedRotation;
    [SerializeField] protected Vector3 fixedWorldOffset;
    [SerializeField] protected bool isLockRotation = true;
    [SerializeField] protected bool faceCamera = false;

    public BaseEntityController BaseEntityController => baseEntityController;

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

        if (this.baseEntityController != null && this.fixedWorldOffset == Vector3.zero)
        {
            this.fixedWorldOffset = transform.position - this.baseEntityController.transform.position;
        }
    }

    protected override void LoadComponent()
    {
        base.LoadComponent();
        this.LoadBaseEntityController();
    }

    protected virtual void LoadBaseEntityController()
    {
        if (this.baseEntityController != null) return;
        this.baseEntityController = GetComponentInParent<BaseEntityController>();
        Debug.LogWarning(transform.name + " : LoadBaseEntityController");
    }

    protected virtual void LateUpdate()
    {
        this.LockRotation();
    }

    protected virtual void LockRotation()
    {
        if (!this.isLockRotation) return;

        if (this.baseEntityController != null)
        {
            transform.position = this.baseEntityController.transform.position + this.fixedWorldOffset;
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
