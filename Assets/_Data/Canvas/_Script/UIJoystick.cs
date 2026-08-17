using UnityEngine;
using UnityEngine.EventSystems;

public class UIJoystick : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    [SerializeField] protected RectTransform joystickBackground;
    [SerializeField] protected RectTransform joystickHandle;
    [SerializeField] protected float handleLimit = 75f;

    private Vector2 inputVector = Vector2.zero;
    public Vector2 InputVector => inputVector;

    protected virtual void Start()
    {
        if (this.joystickBackground == null) this.joystickBackground = GetComponent<RectTransform>();
        if (this.joystickHandle == null) this.joystickHandle = transform.GetChild(0).GetComponent<RectTransform>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        this.OnDrag(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 position;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(this.joystickBackground, eventData.position, eventData.pressEventCamera, out position);
        position.x = (position.x / this.joystickBackground.sizeDelta.x) * 2;
        position.y = (position.y / this.joystickBackground.sizeDelta.y) * 2;
        this.inputVector = new Vector2(position.x, position.y);
        this.inputVector = (this.inputVector.magnitude > 1.0f) ? this.inputVector.normalized : this.inputVector;
        this.joystickHandle.anchoredPosition = new Vector2(
            this.inputVector.x * this.handleLimit, 
            this.inputVector.y * this.handleLimit
        );
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        this.inputVector = Vector2.zero;
        this.joystickHandle.anchoredPosition = Vector2.zero;
    }
}
