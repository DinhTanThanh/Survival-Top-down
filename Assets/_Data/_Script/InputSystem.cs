using UnityEngine;
using UnityEngine.InputSystem;

public class InputSystem : MonoBehaviour
{
    private static InputSystem instance;
    [SerializeField] protected float horizontal;
    [SerializeField] protected float vertical;
    [SerializeField] protected InputAction moveAction;
    public static InputSystem Instance => instance;
    private void Awake()
    {
        InputSystem.instance = this;
    }
    private void OnEnable()
    {
        this.moveAction.Enable();
    }
    private void OnDisable()
    {
        this.moveAction.Disable();
    }
    private void Update()
    {
        Vector2 moveInput = this.moveAction.ReadValue<Vector2>();
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
