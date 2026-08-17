using UnityEngine;
using UnityEngine.UI;

public class BaseSliderBar : LoadMonoBehaviour
{
    [SerializeField] protected Slider slider;
    protected override void LoadComponent()
    {
        base.LoadComponent();
        this.LoadSlider();
    }
    protected virtual void LoadSlider()
    {
        if (this.slider != null) return;
        this.slider = GetComponent<Slider>();
        Debug.LogWarning(transform.name + " : LoadSlider");
    }
}
