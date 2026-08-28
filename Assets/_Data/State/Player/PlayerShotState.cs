using UnityEngine;

public class PlayerShotState : IState
{
    private readonly PlayerController _controller;
    private static readonly int ShootHash = Animator.StringToHash("Shoot");

    public PlayerShotState(PlayerController controller)
    {
        _controller = controller;
    }

    public void Enter()
    {
        if (_controller != null&&_controller.Animator!=null&&_controller.PlayerShooting!=null)
        {
            _controller.Animator.SetTrigger(ShootHash);
            _controller.PlayerShooting.Shooting();
        }
    }

    public void Execute()
    {
        if (InputSystem.Instance == null || _controller == null || _controller.PlayerStateManager == null) return;

        bool hasInput = InputSystem.Instance.GetHorizontal() != 0f || InputSystem.Instance.GetVertical() != 0f;
        if (hasInput)
        {
            _controller.PlayerStateManager.ChangeState(_controller.MoveState);
        }
        else
        {
            _controller.PlayerStateManager.ChangeState(_controller.IdleState);
        }
    }

    public void Exit()
    {
        if (InputSystem.Instance != null)
        {
            InputSystem.Instance.SetIsAttack(false);
        }
    }
}

