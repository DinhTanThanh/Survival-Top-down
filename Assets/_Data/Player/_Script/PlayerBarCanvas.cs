using UnityEngine;

public class PlayerBarCanvas : LoadMonoBehaviour
{
    [SerializeField] protected PlayerController playerController;
    public PlayerController PlayerController => playerController;
    protected override void LoadComponent()
    {
        base.LoadComponent();
        this.LoadPlayerController();
    }
    protected virtual void LoadPlayerController()
    {
        if (this.playerController != null) return;
        this.playerController = GetComponentInParent<PlayerController>();
        Debug.LogWarning(transform.name + " : LoadPlayerController");
    }
}
