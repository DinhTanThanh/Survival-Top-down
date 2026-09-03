using UnityEngine;

public class MinionBarCanvas : LoadMonoBehaviour
{
    [SerializeField] protected MinionController minionController;
    [SerializeField] protected Quaternion fixedRotation;
    [SerializeField] protected Vector3 fixedWorldOffset;
    [SerializeField] protected bool isLockRotation = true;
    [SerializeField] protected bool faceCamera = false;

    public MinionController MinionController => minionController;

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

        if (this.minionController != null && this.fixedWorldOffset == Vector3.zero)
        {
            this.fixedWorldOffset = transform.position - this.minionController.transform.position;
        }
    }

    protected override void LoadComponent()
    {
        base.LoadComponent();
        this.LoadMinionController();
    }

    protected virtual void LoadMinionController()
    {
        if (this.minionController != null) return;
        this.minionController = GetComponentInParent<MinionController>();
        Debug.LogWarning(transform.name + " : LoadMinionController");
    }

    protected virtual void LateUpdate()
    {
        this.LockRotation();
    }

    protected virtual void LockRotation()
    {
        if (!this.isLockRotation) return;

        if (this.minionController != null)
        {
            transform.position = this.minionController.transform.position + this.fixedWorldOffset;
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
