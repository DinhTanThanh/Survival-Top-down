using System.Collections.Generic;
using UnityEngine;

public class IndicatorSystem : BasePooling
{
    private static IndicatorSystem instance;
    [SerializeField] protected Dictionary<string,GameObject> dicIndicator=new Dictionary<string,GameObject>();
    public static IndicatorSystem Instance => instance;
    protected override void Awake()
    {
        base.Awake();
        if (IndicatorSystem.instance != null)
        {
            Debug.LogError("Singleton has already exit");
            return;
        }
        IndicatorSystem.instance = this;
    }
    protected override void LoadComponent()
    {
        base.LoadComponent();
        this.LoadDictionaryIndicator();
    }
    protected virtual void LoadDictionaryIndicator()
    {
        if (dicIndicator.Count > 0) return;
        foreach(Transform child in transform)
        {
            if (child == null) continue;
            this.dicIndicator.Add(child.name, child.gameObject);
            child.gameObject.SetActive(false);
        }
    }
    public virtual GameObject SpawnIndicator(string nameIndicator, Vector3 pos, Quaternion rot)
    {
        if (!this.dicIndicator.ContainsKey(nameIndicator))
        {
            Debug.LogWarning($"Indicator with name '{nameIndicator}' not found in IndicatorSystem!");
            return null;
        }
        GameObject objPrefab = this.dicIndicator[nameIndicator];
        return this.ExecuteSpawnPooling(objPrefab, pos, rot);
    }

    public virtual void DespawnIndicator(GameObject indicator)
    {
        if (indicator == null) return;
        indicator.SetActive(false);
        this.GoBackList(indicator);
    }
}
