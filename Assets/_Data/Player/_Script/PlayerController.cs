using UnityEngine;

public class PlayerController : LoadMonoBehaviour
{
    [SerializeField] protected EntitySO entitySO;
    public EntitySO EntitySO => entitySO;
    protected override void LoadComponent()
    {
        base.LoadComponent();
        this.LoadEntitySO();
    }
    protected virtual void LoadEntitySO()
    {
        if (this.entitySO != null) return;
        this.entitySO = Resources.Load<EntitySO>("Entity/" + transform.name + "SO");
        Debug.LogWarning(transform.name + " : LoadEntitySO");
    }
}
