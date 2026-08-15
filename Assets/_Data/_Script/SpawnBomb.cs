using UnityEngine;

public class SpawnBomb : BasePooling
{
    private static SpawnBomb instance;
    public static SpawnBomb Instance => instance;
    protected override void Awake()
    {
        base.Awake();
        if (SpawnBomb.instance != null)
        {
            Debug.LogError("Singleton already exists. Only a singleton is allowed to exist");
            return;
        }
        SpawnBomb.instance = this;
    }
}
