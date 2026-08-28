using System.Collections.Generic;
using UnityEngine;

public class BasePooling : BaseSpawn
{
    [SerializeField] protected List<GameObject> listObjectPooling = new List<GameObject>();
    protected virtual GameObject SpawnObjectPooling(GameObject prefab)
    {
        GameObject objectPooling;
        for (int i = this.listObjectPooling.Count - 1; i >= 0; i--)
        {
            if (this.listObjectPooling[i].name.CompareTo(prefab.name) == 0)
            {
                objectPooling = this.listObjectPooling[i];
                this.listObjectPooling.Remove(objectPooling);
                return objectPooling;
            }
        }
        return this.Spawn(prefab);
    }
    protected virtual void SetTransformObjectSpawn(GameObject prefab, Vector3 position, Quaternion rotation)
    {
        prefab.transform.position = position;
        prefab.transform.rotation = rotation;
        prefab.name = prefab.name.Replace("(Clone)", "");
        prefab.gameObject.SetActive(true);
    }
    public virtual void ExecuteSpawnPooling(GameObject prefab, Vector3 position, Quaternion rotation)
    {
        GameObject newObject = this.SpawnObjectPooling(prefab);
        this.SetTransformObjectSpawn(newObject, position, rotation);
    }
    public virtual void GoBackList(GameObject gameObejct)
    {
        this.listObjectPooling.Add(gameObejct);
    }
}
