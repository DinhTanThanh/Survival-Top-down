using UnityEngine;

public class VFX_SphereVioController : LoadMonoBehaviour
{
    [SerializeField] protected ParticleSystem partSystem;
    public ParticleSystem PartSystem => partSystem;
    private void OnEnable()
    {
        this.partSystem.Play();
    }
    protected override void LoadComponent()
    {
        base.LoadComponent();
        this.LoadParticleSystem();
    }
    protected virtual void LoadParticleSystem()
    {
        if (this.partSystem != null) return;
        this.partSystem=GetComponentInChildren<ParticleSystem>();
        Debug.LogWarning(transform.name + " : LoadParticleSystem");
    }
}
