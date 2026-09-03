using UnityEngine;

public class MinionHandLeft : LoadMonoBehaviour
{
    [SerializeField] protected Collider colliderr;
    public Collider Collider => colliderr;
    protected override void LoadComponent()
    {
        base.LoadComponent();
        this.LoadCollider();
    }
    protected virtual void LoadCollider()
    {
        if (this.colliderr != null) return;
        this.colliderr = GetComponent<Collider>();
        this.colliderr.isTrigger = true;
        Debug.LogWarning(transform.name + " : LoadCollider");
    }
}
