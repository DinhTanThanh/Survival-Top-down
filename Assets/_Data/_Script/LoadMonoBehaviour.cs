using UnityEngine;

public class LoadMonoBehaviour : MonoBehaviour
{
    protected virtual void Awake()
    {
        this.LoadComponent();
    }
    protected virtual void Reset()
    {
        this.LoadComponent();
    }
    protected virtual void LoadComponent() { }
}
