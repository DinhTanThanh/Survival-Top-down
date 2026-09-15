using UnityEngine;

public class VFXDisableByTime : BaseDisableByTime
{
    [SerializeField] protected ParticleSystem partSystem;

    protected virtual void OnEnable()
    {
        this.SetTimer(0f);
        if (this.partSystem != null)
        {
            this.partSystem.Play();
        }
    }

    protected override void LoadComponent()
    {
        base.LoadComponent();
        this.SetDelayTime(1f);
        this.LoadParticleSystem();
    }

    protected virtual void LoadParticleSystem()
    {
        if (this.partSystem != null) return;
        this.partSystem = GetComponentInChildren<ParticleSystem>();
        Debug.LogWarning(transform.name + " : LoadParticleSystem");
    }

    protected virtual void Update()
    {
        if (this.Timing())
        {
            if (this.partSystem != null)
            {
                this.partSystem.Stop();
            }

            GameObject rootObj = this.transform.parent != null ? this.transform.parent.gameObject : this.gameObject;
            rootObj.SetActive(false);

            if (SpawnVFX.Instance != null)
            {
                SpawnVFX.Instance.GoBackList(rootObj);
            }
        }
    }
}
