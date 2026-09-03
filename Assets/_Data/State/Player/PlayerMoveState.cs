using UnityEngine;

public class PlayerMoveState : IState
{
    private readonly PlayerController _controller;
    private static readonly int IsRunningHash = Animator.StringToHash("IsRunning");

    public PlayerMoveState(PlayerController controller)
    {
        _controller = controller;
    }

    public void Enter()
    {
        if (_controller != null && _controller.Animator != null)
        {
            _controller.Animator.SetBool(IsRunningHash, true);
        }
    }

    public void Execute()
    {
        if (InputSystem.Instance == null || _controller == null || _controller.PlayerStateManager == null) return;
        if (_controller.DashExplosionSkill != null && _controller.DashExplosionSkill.GetIsDash()) return;
        bool hasInput = InputSystem.Instance.GetHorizontal() != 0f || InputSystem.Instance.GetVertical() != 0f;
        if (hasInput)
        {
            if (_controller.PlayerMoving != null)
            {
                _controller.PlayerMoving.Moving();
            }

            if (InputSystem.Instance.GetIsAttack() && _controller.PlayerShooting != null && _controller.PlayerShooting.CanFire)
            {
                _controller.PlayerStateManager.ChangeState(_controller.ShotState);
            }
        }
        else
        {
            _controller.PlayerStateManager.ChangeState(_controller.IdleState);
        }
    }

    public void Exit()
    {
        bool hasInput = InputSystem.Instance != null && (InputSystem.Instance.GetHorizontal() != 0f || InputSystem.Instance.GetVertical() != 0f);
        if (!hasInput && _controller != null && _controller.Animator != null)
        {
            _controller.Animator.SetBool(IsRunningHash, false);
        }
    }
}


