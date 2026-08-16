using UnityEngine;

public class SpawnMeleeEnemy : BasePooling
{
    private static SpawnMeleeEnemy instance;
    public static SpawnMeleeEnemy Instance => instance;
    protected override void Awake()
    {
        base.Awake();
        if (SpawnMeleeEnemy.instance != null)
        {
            Debug.LogError("Singleton already exists. Only a singleton is allowed to exist");
            return;
        }
        SpawnMeleeEnemy.instance = this;
    }
}
