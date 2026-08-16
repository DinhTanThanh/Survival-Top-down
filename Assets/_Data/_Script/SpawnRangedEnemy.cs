using UnityEngine;

public class SpawnRangedEnemy : BasePooling
{
    private static SpawnRangedEnemy instance;
    public static SpawnRangedEnemy Instance => instance;
    protected override void Awake()
    {
        base.Awake();
        if (SpawnRangedEnemy.instance != null)
        {
            Debug.LogError("Singleton already exists. Only a singleton is allowed to exist");
            return;
        }
        SpawnRangedEnemy.instance = this;
    }
}
