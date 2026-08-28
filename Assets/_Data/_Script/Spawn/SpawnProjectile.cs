using UnityEngine;

public class SpawnProjectile : BasePooling
{
    private static SpawnProjectile instance;
    public static SpawnProjectile Instance => instance;
    protected override void Awake()
    {
        base.Awake();
        if (SpawnProjectile.instance != null)
        {
            Debug.LogError("Singleton already exists. Only a singleton is allowed to exist");
            return;
        }
        SpawnProjectile.instance = this;
    }
}
