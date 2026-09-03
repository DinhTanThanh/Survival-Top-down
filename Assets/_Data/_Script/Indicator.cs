using System.Collections.Generic;
using UnityEngine;
public class Indicator : LoadMonoBehaviour
{
    [SerializeField] protected List<ParticleSystem> particles = new List<ParticleSystem>();
    public List<ParticleSystem> Particles => particles;
    protected override void LoadComponent()
    {
        base.LoadComponent();
        this.LoadParticles();
    }
    protected virtual void LoadParticles()
    {
        if (this.particles.Count > 0) return;
        foreach(Transform child in transform)
        {
            ParticleSystem particle = child.GetComponent<ParticleSystem>();
            if (particle == null) continue;
            this.particles.Add(particle);
            Debug.Log(particle.name);
        }
    }
    public virtual ParticleSystem GetParticleByName(string nameParticle)
    {
        foreach(ParticleSystem particle in this.particles)
        {
            if (particle.name == nameParticle)
            {
                return particle;
            }
        }
        return null;
    }
}
