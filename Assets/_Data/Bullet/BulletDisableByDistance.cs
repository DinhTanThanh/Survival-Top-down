using System.Security.Cryptography;
using UnityEngine;

public class BulletDisableByDistance : BaseDisableByDistance
{
    [SerializeField] protected BulletController bulletController;
    protected override void LoadComponent()
    {
        base.LoadComponent();
        this.LoadBulletController();
        this.SetDistanceLimit(this.bulletController.WeaponData.maxDistance);
    }
    protected virtual void LoadBulletController()
    {
        if (this.bulletController != null) return;
        this.bulletController = GetComponentInParent<BulletController>();
        Debug.LogWarning(transform.name + " : LoadBulletController");
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
        SpawnBullet.Instance.GoBackList(this.transform.parent.gameObject);
    }
}
