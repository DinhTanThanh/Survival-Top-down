using UnityEngine;

public class ProjectileDisableByDistance : BaseDisableByDistance
{
    [SerializeField] protected ProjectileController projectileController;
    protected override void LoadComponent()
    {
        base.LoadComponent();
        this.LoadProjectileController();
        this.SetDistanceLimit(this.projectileController.WeaponData.maxDistance);
    }
    protected virtual void LoadProjectileController()
    {
        if (this.projectileController != null) return;
        this.projectileController = GetComponentInParent<ProjectileController>();
        Debug.LogWarning(transform.name + " : LoadProjectileController");
    }
    private void OnEnable()
    {
        this.posStart = this.transform.parent.position;
    }
    private void Update()
    {
        this.posCurrent = this.transform.parent.position;
        if (!this.IsMaxDistanceReached()) return;
        this.transform.parent.gameObject.SetActive(false);
        SpawnProjectile.Instance.GoBackList(this.transform.parent.gameObject);
    }
}
