using UnityEngine;

public class BaseEntityController : LoadMonoBehaviour
{
    [SerializeField] protected Transform target;
    [SerializeField] protected EntitySO entitySO;
    [SerializeField] protected Animator animator;
    public Transform Target => target;

    public EntitySO EntitySO => entitySO;
    public Animator Animator => animator;
    protected override void LoadComponent()
    {
        base.LoadComponent();
        this.LoadTarget();
        this.LoadEntitySO();
        this.LoadAnimator();
    }
    protected virtual void LoadTarget()
    {
        if (this.target != null) return;
        this.target = GameObject.Find("Player")?.transform;
        Debug.LogWarning(transform.name + " : LoadTarget");
    }
    protected virtual void LoadEntitySO()
    {
        if (this.entitySO != null) return;
        this.entitySO = Resources.Load<EntitySO>("Entity/" + transform.name + "SO");
        Debug.LogWarning(transform.name + " : LoadEntitySO");
    }
    protected virtual void LoadAnimator()
    {
        if (this.animator != null) return;
        this.animator = GetComponentInChildren<Animator>();
        Debug.LogWarning(transform.name + " : LoadAnimator");
    }
}
