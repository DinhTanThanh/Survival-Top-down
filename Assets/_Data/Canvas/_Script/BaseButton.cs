using UnityEngine;
using UnityEngine.UI;

public class BaseButton : LoadMonoBehaviour
{
    [SerializeField] protected Button button;
    protected override void LoadComponent()
    {
        base.LoadComponent();
        this.LoadButton();
    }
    protected virtual void LoadButton()
    {
        if (this.button != null) return;
        this.button = GetComponent<Button>();
        Debug.LogWarning(transform.name + " : LoadButton");
    }
}
