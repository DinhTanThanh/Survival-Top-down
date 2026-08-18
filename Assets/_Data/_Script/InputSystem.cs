using UnityEngine;
using UnityEngine.InputSystem;

public class InputSystem : MonoBehaviour
{
    private static InputSystem instance;
    [SerializeField] protected float horizontal;
    [SerializeField] protected float vertical;
    [SerializeField] protected InputAction moveAction;
    [SerializeField] protected UIJoystick uiJoystick;

    public static InputSystem Instance => instance;

    private void Awake()
    {
        InputSystem.instance = this;
        this.InitNewInputSystem();
    }

    protected virtual void InitNewInputSystem()
    {
        if (this.moveAction == null || this.moveAction.bindings.Count == 0)
        {
            this.moveAction = new InputAction("Move", InputActionType.Value);
            this.moveAction.AddCompositeBinding("2DVector")
                .With("Up", "<Keyboard>/w")
                .With("Down", "<Keyboard>/s")
                .With("Left", "<Keyboard>/a")
                .With("Right", "<Keyboard>/d")
                .With("Up", "<Keyboard>/upArrow")
                .With("Down", "<Keyboard>/downArrow")
                .With("Left", "<Keyboard>/leftArrow")
                .With("Right", "<Keyboard>/rightArrow");
        }
    }

    private void Start()
    {
        this.LoadUIJoystick();
    }

    private void OnEnable()
    {
        if (this.moveAction != null)
        {
            this.moveAction.Enable();
        }
    }

    private void OnDisable()
    {
        if (this.moveAction != null)
        {
            this.moveAction.Disable();
        }
    }

    protected virtual void LoadUIJoystick()
    {
        if (this.uiJoystick != null) return;
        this.uiJoystick = FindFirstObjectByType<UIJoystick>();
    }

    private void Update()
    {
        Vector2 moveInput = Vector2.zero;

        if (this.moveAction != null && this.moveAction.enabled)
        {
            moveInput = this.moveAction.ReadValue<Vector2>();
        }

        if (this.uiJoystick == null) this.LoadUIJoystick();
        if (this.uiJoystick != null && this.uiJoystick.InputVector != Vector2.zero)
        {
            moveInput = this.uiJoystick.InputVector;
        }

        this.horizontal = moveInput.x;
        this.vertical = moveInput.y;
    }

    public virtual float GetHorizontal()
    {
        return this.horizontal;
    }

    public virtual float GetVertical()
    {
        return this.vertical;
    }
}
