using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemyPoolingManager : BasePooling
{
    private static SpawnEnemyPoolingManager instance;
    [SerializeField] protected Dictionary<int, MinionController> cachedMinionControllers = new Dictionary<int, MinionController>();
    public static SpawnEnemyPoolingManager Instance => instance;
    protected override void Awake()
    {
        base.Awake();
        if (SpawnEnemyPoolingManager.instance != null)
        {
            Debug.LogError("Singleton already exists. Only a singleton is allowed to exist");
            return;
        }
        SpawnEnemyPoolingManager.instance = this;
    }
    public virtual MinionController SpawnEnemy(GameObject prefab, Vector3 position, Quaternion rotation)
    {
        if (prefab == null) return null;
        GameObject obj = this.SpawnObjectPooling(prefab);
        this.SetTransformObjectSpawn(obj, position, rotation);
        int instanceId = obj.GetInstanceID();
        if(!this.cachedMinionControllers.TryGetValue(instanceId, out MinionController controller))
        {
            controller=obj.GetComponent<MinionController>();
            this.cachedMinionControllers.Add(instanceId, controller);
        }
        return controller;
    }
}
