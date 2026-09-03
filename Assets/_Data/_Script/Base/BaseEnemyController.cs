using System.Data.Common;
using UnityEngine;

public class BaseEnemyController : BaseEntityController
{
    [SerializeField] protected EnemySO enemySO;
    public EnemySO EnemySO => enemySO;
    protected override void LoadComponent()
    {
        base.LoadComponent();
        this.LoadTarget();
        this.LoadEnemySO();
        this.LoadAnimator();
        this.LoadDamageReceiver();
    }
    protected virtual void LoadEnemySO()
    {
        if (this.enemySO != null) return;
        this.enemySO = Resources.Load<EnemySO>("Enemy/" + transform.name + "SO");
        Debug.LogWarning(transform.name + " : LoadEnemySO");
    }
}
