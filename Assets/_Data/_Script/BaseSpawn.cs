using UnityEngine;

public class BaseSpawn : LoadMonoBehaviour
{
    protected virtual GameObject Spawn(GameObject prefab)
    {
        return Instantiate(prefab);
    }
}
