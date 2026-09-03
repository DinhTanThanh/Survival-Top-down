using UnityEngine;

public class PlayerEventAnimation : LoadMonoBehaviour
{
    [SerializeField] protected float lastExplosionTime = -1f;
    [SerializeField] protected float baseDamage;
    [SerializeField] protected float explosionRedius;
    [SerializeField] protected Transform player;
    [SerializeField] protected GameObject explosion_light;
    [SerializeField] protected ParticleSystem particlesystem;
    [SerializeField] protected DashExplosionData dashExplosionData;
    protected override void LoadComponent()
    {
        base.LoadComponent();
        this.LoadPlayer();
        this.LoadDashExplosionData();
        this.LoadExplosion_light();
        this.LoadParticleSystem();
        this.SetExplosionRadius(this.dashExplosionData.ExplosionRadius);
        this.SetBaseDamage(this.dashExplosionData.BaseDamage);
    }
    protected virtual void SetBaseDamage(float baseDamage)
    {
        this.baseDamage = baseDamage;
    }
    protected virtual void SetExplosionRadius(float explosionRadius)
    {
        this.explosionRedius = explosionRadius;
    }
    protected virtual void LoadParticleSystem()
    {
        if (this.explosion_light == null) return;
        if (this.particlesystem != null) return;
        this.particlesystem=this.explosion_light.GetComponent<ParticleSystem>();
        Debug.LogWarning(transform.name + " : LoadParticleSystem");
    }
    protected virtual void LoadExplosion_light()
    {
        if (this.explosion_light != null) return;
        this.explosion_light = GameObject.Find("Explosion_light");
        Debug.LogWarning(transform.name + " : LoadExplosion_light");
    }
    protected virtual void LoadPlayer()
    {
        if (this.player != null) return;
        this.player = GameObject.Find("Player")?.transform;
        Debug.LogWarning(transform.name + " : LoadPlayer");
    }
    protected virtual void LoadDashExplosionData()
    {
        if (this.dashExplosionData != null) return;
        this.dashExplosionData = Resources.Load<DashExplosionData>("WeaponData/DashExplosionData");
        Debug.LogWarning(transform.name + " : LoadDashExplosionData");
    }

    public virtual void Explosion()
    {
        if (Time.time - this.lastExplosionTime < 0.2f) return;
        this.lastExplosionTime = Time.time;

        if (this.explosion_light != null)
        {
            this.explosion_light.transform.position = this.player.position;
            if (this.particlesystem != null)
            {
                this.particlesystem.Play();
            }
        }
        Vector3 center = this.player != null ? this.player.position : transform.position;
        Collider[] hitEnemies = Physics.OverlapSphere(center, this.explosionRedius);
        System.Collections.Generic.HashSet<DamageReceiver> damagedReceivers = new System.Collections.Generic.HashSet<DamageReceiver>();
        foreach (Collider collider in hitEnemies)
        {
            if (collider == null) continue;
            DamageReceiver dameReceiver = collider.transform.parent?.GetComponentInChildren<DamageReceiver>();
            if (dameReceiver == null)
            {
                dameReceiver = collider.GetComponentInParent<DamageReceiver>();
            }
            if (dameReceiver == null || dameReceiver is PlayerDamageReceiver) continue;
            if (damagedReceivers.Add(dameReceiver))
            {
                dameReceiver.SetIsTakeDamage(true);
                dameReceiver.ReduceHp(this.baseDamage);
            }
        }
    }
}
