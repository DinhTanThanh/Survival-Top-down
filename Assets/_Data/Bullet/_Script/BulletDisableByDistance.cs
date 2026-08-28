using UnityEngine;

public class BulletDisableByDistance : BaseDisableByDistance
{
    [SerializeField] protected BulletController bulletController;
    private void OnEnable()
    {
        this.SetFirePointStart(this.bulletController.FirePointStart.position);
    }
    protected override void LoadComponent()
    {
        base.LoadComponent();
        this.LoadBulletController();
        this.SetDistanceLimit(this.bulletController.WeaponData.maxDistance);
    }
    protected virtual void SetFirePointStart(Vector3 posStart)
    {
        this.posStart = posStart;
    }
    protected virtual void LoadBulletController()
    {
        if (this.bulletController != null) return;
        this.bulletController = GetComponentInParent<BulletController>();
        Debug.LogWarning(transform.name + " : LoadBulletController");
    }
    private void Update()
    {
        this.posCurrent = this.transform.parent.position;
        if (!this.IsMaxDistanceReached()) return;
        this.transform.parent.gameObject.SetActive(false);
        SpawnBullet.Instance.GoBackList(this.transform.parent.gameObject);
    }
}
