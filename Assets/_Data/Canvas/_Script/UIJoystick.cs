using UnityEngine;
using UnityEngine.EventSystems;

public class UIJoystick : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    [SerializeField] protected RectTransform joystickBackground;
    [SerializeField] protected RectTransform joystickHandle;
    [SerializeField] protected float handleLimit = 75f;
    [SerializeField] protected float deadZone = 0.15f;

    private Vector2 inputVector = Vector2.zero;
    private Vector2 defaultHandlePos;

    public Vector2 InputVector => inputVector;

    protected virtual void Awake()
    {
        if (this.joystickBackground == null) this.joystickBackground = GetComponent<RectTransform>();
        if (this.joystickHandle == null && transform.childCount > 0) this.joystickHandle = transform.GetChild(0).GetComponent<RectTransform>();
        if (this.joystickHandle != null) this.defaultHandlePos = this.joystickHandle.anchoredPosition;
    }

    protected virtual void Start()
    {
        if (this.joystickBackground == null) this.joystickBackground = GetComponent<RectTransform>();
        if (this.joystickHandle == null && transform.childCount > 0) this.joystickHandle = transform.GetChild(0).GetComponent<RectTransform>();
        if (this.joystickHandle != null) this.defaultHandlePos = this.joystickHandle.anchoredPosition;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        this.OnDrag(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 position;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(this.joystickBackground, eventData.position, eventData.pressEventCamera, out position))
        {
            Vector2 pivotOffset = new Vector2(
                (0.5f - this.joystickBackground.pivot.x) * this.joystickBackground.rect.width,
                (0.5f - this.joystickBackground.pivot.y) * this.joystickBackground.rect.height
            );
            
            position -= pivotOffset;

            position.x = (position.x / this.joystickBackground.rect.width) * 2;
            position.y = (position.y / this.joystickBackground.rect.height) * 2;
            
            Vector2 rawVector = new Vector2(position.x, position.y);
            Vector2 clampedVector = (rawVector.magnitude > 1.0f) ? rawVector.normalized : rawVector;

            this.joystickHandle.anchoredPosition = this.defaultHandlePos + (clampedVector * this.handleLimit);

            if (rawVector.magnitude > this.deadZone)
            {
                this.inputVector = clampedVector.normalized;
            }
            else
            {
                this.inputVector = Vector2.zero;
            }
        }
    }   

    public void OnPointerUp(PointerEventData eventData)
    {
        this.inputVector = Vector2.zero;
        this.joystickHandle.anchoredPosition = this.defaultHandlePos;
    }
}
