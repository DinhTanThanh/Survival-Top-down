using System.Security.Cryptography;
using UnityEngine;

public class BulletDisableByDistance : BaseDisableByDistance
{
    protected override void LoadComponent()
    {
        base.LoadComponent();
        this.SetDistanceLimit(5f);
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
