using System.Collections.Generic;
using UnityEngine;

public class ManagerSpawnPoint : LoadMonoBehaviour
{
    [SerializeField] protected List<Transform> listPointer = new List<Transform>();
    public List<Transform> ListPointer => listPointer;
    protected override void LoadComponent()
    {
        base.LoadComponent();
        this.LoadListPointer();
    }
    protected virtual void LoadListPointer()
    {
        if (this.listPointer.Count > 0) return;
        foreach(Transform childPointer in transform)
        {
            this.listPointer.Add(childPointer);
        }
    }
}
