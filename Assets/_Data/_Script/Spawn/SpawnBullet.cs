using UnityEngine;

public class SpawnBullet : BasePooling
{
    private static SpawnBullet instance;
    public static SpawnBullet Instance => instance;
    protected override void Awake()
    {
        base.Awake();
        if (SpawnBullet.instance != null)
        {
            Debug.LogError("Singleton already exists. Only a singleton is allowed to exist");
            return;
        }
        SpawnBullet.instance = this;
    }
}
