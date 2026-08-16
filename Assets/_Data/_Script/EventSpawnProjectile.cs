using UnityEngine;

public class EventSpawnProjectile : LoadMonoBehaviour
{
    [SerializeField] protected Transform pointer;
    [SerializeField] protected GameObject projectilePrefab;

    [SerializeField] protected RangedEnemyController rangedEnemyController;
    protected override void LoadComponent()
    {
        base.LoadComponent();
        this.LoadRangedEnemyController();
        this.SetPointer(this.rangedEnemyController.Pointer);
        this.LoadProjectilePrefab();
    }
    protected virtual void LoadProjectilePrefab()
    {
        if (this.projectilePrefab != null) return;
        this.projectilePrefab = GameObject.Find("Projectile");
        Debug.LogWarning(transform.name + " : LoadProjectilePrefab");
    }
    protected virtual void LoadRangedEnemyController()
    {
        if (this.rangedEnemyController != null) return;
        this.rangedEnemyController = GetComponentInParent<RangedEnemyController>();
        Debug.LogWarning(transform.name + " : LoadRangedEnemyController");
    }
    protected virtual void SetPointer(Transform pointer)
    {
        if (pointer == null) return;
        this.pointer = pointer;
    }
    public virtual void Spawn()
    {
        Quaternion rot = Quaternion.Euler(0, -8f, 0);
        Quaternion rotCurrent = this.transform.rotation;
        Quaternion newRot = rot * rotCurrent;
        SpawnProjectile.Instance.ExecuteSpawnPooling(this.projectilePrefab, this.pointer.position, newRot);
    }
}
