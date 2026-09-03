using UnityEngine;

public class PlayerIdleState : IState
{
    private readonly PlayerController _controller;
    private static readonly int IsRunningHash = Animator.StringToHash("IsRunning");

    public PlayerIdleState(PlayerController controller)
    {
        _controller = controller;
    }

    public void Enter()
    {
        if (_controller != null && _controller.Animator != null)
        {
            _controller.Animator.SetBool(IsRunningHash, false);
        }
    }

    public void Execute()
    {
        if (InputSystem.Instance == null || _controller == null || _controller.PlayerStateManager == null) return;

        if (_controller.DashExplosionSkill != null && _controller.DashExplosionSkill.GetIsDash())
        {
            _controller.PlayerStateManager.ChangeState(_controller.MoveState);
            return;
        }

        if (InputSystem.Instance.GetHorizontal() != 0f || InputSystem.Instance.GetVertical() != 0f )
        {
            _controller.PlayerStateManager.ChangeState(_controller.MoveState);
            return;
        }
        if (InputSystem.Instance.GetIsAttack() && _controller.PlayerShooting != null && _controller.PlayerShooting.CanFire)
        {
            _controller.PlayerStateManager.ChangeState(_controller.ShotState);
            return;
        }
    }

    public void Exit()
    {
    }
}
