using UnityEngine;

public class SpawnVFX_SphereVio : BasePooling
{
    private static SpawnVFX_SphereVio instance;
    public static SpawnVFX_SphereVio Instance => instance;
    protected override void Awake()
    {
        base.Awake();
        if (SpawnVFX_SphereVio.instance != null)
        {
            Debug.LogError("Singleton already exists. Only a singleton is allowed to exist");
            return;
        }
        SpawnVFX_SphereVio.instance = this;
    }
}
