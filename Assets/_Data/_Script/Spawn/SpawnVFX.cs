using UnityEngine;

public class SpawnVFX : BasePooling
{
    private static SpawnVFX instance;
    public static SpawnVFX Instance => instance;

    protected override void Awake()
    {
        base.Awake();
        if (SpawnVFX.instance != null)
        {
            Debug.LogError("Singleton already exists. Only a singleton is allowed to exist: " + transform.name);
            return;
        }
        SpawnVFX.instance = this;
    }
}
