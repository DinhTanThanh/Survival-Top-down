using UnityEngine;

public class MeleeEnemyBarCanvas : LoadMonoBehaviour
{
    [SerializeField] protected MeleeEnemyController meleeEnemyController;
    [SerializeField] protected Quaternion fixedRotation;
    [SerializeField] protected Vector3 fixedWorldOffset;
    [SerializeField] protected bool isLockRotation = true;
    [SerializeField] protected bool faceCamera = false;

    public MeleeEnemyController MeleeEnemyController => meleeEnemyController;

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

        if (this.meleeEnemyController != null && this.fixedWorldOffset == Vector3.zero)
        {
            this.fixedWorldOffset = transform.position - this.meleeEnemyController.transform.position;
        }
    }

    protected override void LoadComponent()
    {
        base.LoadComponent();
        this.LoadMeleeEnemyController();
    }

    protected virtual void LoadMeleeEnemyController()
    {
        if (this.meleeEnemyController != null) return;
        this.meleeEnemyController = GetComponentInParent<MeleeEnemyController>();
        Debug.LogWarning(transform.name + " : LoadMeleeEnemyController");
    }

    protected virtual void LateUpdate()
    {
        this.LockRotation();
    }

    protected virtual void LockRotation()
    {
        if (!this.isLockRotation) return;

        if (this.meleeEnemyController != null)
        {
            transform.position = this.meleeEnemyController.transform.position + this.fixedWorldOffset;
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
