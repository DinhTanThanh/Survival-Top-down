using System.Data.Common;
using UnityEngine;

public class BaseEnemyController : LoadMonoBehaviour
{
    [SerializeField] protected Transform target;
    [SerializeField] protected EnemySO enemySO;
    [SerializeField] protected Animator animator;
    [SerializeField] protected DamageReceiver damageReceiver;
    public Transform Target => target;
    public EnemySO EnemySO => enemySO;
    public Animator Animator => animator;
    public DamageReceiver DamageReceiver => damageReceiver;
    protected override void LoadComponent()
    {
        base.LoadComponent();
        this.LoadTarget();
        this.LoadEntitySO();
        this.LoadAnimator();
        this.LoadDamageReceiver();
    }
    protected virtual void LoadDamageReceiver()
    {
        if (this.damageReceiver != null) return;
        this.damageReceiver = GetComponentInChildren<DamageReceiver>();
        Debug.LogWarning(transform.name + " : LoaddamageReceiver");
    }
    protected virtual void LoadTarget()
    {
        if (this.target != null) return;
        this.target = GameObject.Find("Player")?.transform;
        Debug.LogWarning(transform.name + " : LoadTarget");
    }
    protected virtual void LoadEntitySO()
    {
        if (this.enemySO != null) return;
        this.enemySO = Resources.Load<EnemySO>("Enemy/" + transform.name + "SO");
        Debug.LogWarning(transform.name + " : LoadEnemySO");
    }
    protected virtual void LoadAnimator()
    {
        if (this.animator != null) return;
        this.animator = GetComponentInChildren<Animator>();
        Debug.LogWarning(transform.name + " : LoadAnimator");
    }
}
