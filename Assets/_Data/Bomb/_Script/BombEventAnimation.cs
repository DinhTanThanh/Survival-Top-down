using UnityEngine;

public class BombEventAnimation : LoadMonoBehaviour
{
    [SerializeField] protected float baseDamage;
    [SerializeField] protected float explosionRadius;
    [SerializeField] protected BombSkillController bombSkillController;

    protected override void LoadComponent()
    {
        base.LoadComponent();
        this.LoadBombSkillController();
        this.SetBaseDamage(this.bombSkillController.SkillData.BaseDamage);
        this.SetExplosionRadius(this.bombSkillController.SkillData.ExplosionRadius);
    }
    protected virtual void LoadBombSkillController()
    {
        if (this.bombSkillController != null) return;
        this.bombSkillController = GetComponentInParent<BombSkillController>();
        Debug.LogWarning(transform.name + " : LoadBombSkillController");
    }
    protected virtual void SetBaseDamage(float baseDamage)
    {
        this.baseDamage = baseDamage;
    }
    protected virtual void SetExplosionRadius(float explosionRadius)
    {
        this.explosionRadius = explosionRadius;
    }
    protected virtual void ExecuteExplosion()
    {
        Collider[] hitEnemies = Physics.OverlapSphere(this.transform.parent.position, this.explosionRadius);
        foreach (Collider collider in hitEnemies)
        {
            DamageReceiver damageReceiver = collider.transform.parent?.GetComponentInChildren<DamageReceiver>();
            if (damageReceiver == null) continue;
            damageReceiver.ReduceHp(this.baseDamage);
        }
    }
    protected virtual void DisableBomb()
    {
        this.transform.parent.gameObject.SetActive(false);
        SpawnBomb.Instance.GoBackList(this.transform.parent.gameObject);
    }
}
