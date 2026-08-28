using UnityEngine;
public class BulletDamageSender : DamageSender
{
    [SerializeField] protected BulletController bulletController;
    protected override void LoadComponent()
    {
        base.LoadComponent();
        this.LoadBulletController();
        this.SetBaseDamage(this.bulletController.WeaponData.baseDamage);
        this.SetDamageMultiplier(this.bulletController.WeaponData.damageMultiplier);
    }
    protected virtual void LoadBulletController()
    {
        if (this.bulletController != null) return;
        this.bulletController = GetComponent<BulletController>();
        Debug.LogWarning(transform.name + " : LoadBulletController");
    }
    private void OnTriggerEnter(Collider other)
    {
        DamageReceiver damageReceiver=other.transform.parent?.GetComponentInChildren<DamageReceiver>();
        if (damageReceiver == null || damageReceiver is PlayerDamageReceiver) return;
        Vector3 posSpawn = this.transform.position;
        posSpawn.y = 0f;
        SpawnVFX_SphereVio.Instance.ExecuteSpawnPooling(this.bulletController.VFX_SphereVio,posSpawn,Quaternion.identity);
        float damage = this.CalculateDamage();
        damageReceiver.ReduceHp(damage);
        this.transform.gameObject.SetActive(false);
        SpawnBullet.Instance.GoBackList(transform.gameObject);
    }
}
