using UnityEngine;

public class VFX_SphereVioDisableByTime : BaseDisableByTime
{
    [SerializeField] protected VFX_SphereVioController controller;
    private void OnEnable()
    {
        this.SetTimer(0f);
    }
    protected override void LoadComponent()
    {
        base.LoadComponent();
        this.SetDelayTime(1f);
        this.LoadVFX_SphereVioController();
    }
    protected virtual void LoadVFX_SphereVioController()
    {
        if (this.controller != null) return;
        this.controller = GetComponentInParent<VFX_SphereVioController>();
        Debug.LogWarning(transform.name + " : LoadVFX_SphereVioController");
    }
    private void Update()
    {
        if (this.Timing())
        {
            this.controller.PartSystem.Stop();
            this.transform.parent.gameObject.SetActive(false);
            SpawnVFX_SphereVio.Instance.GoBackList(this.transform.parent.gameObject);
        }
    }
}
