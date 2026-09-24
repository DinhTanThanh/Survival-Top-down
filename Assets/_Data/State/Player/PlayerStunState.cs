using UnityEngine;

public class PlayerStunState : IState
{
    private readonly PlayerController _controller;

    private static readonly int IsRunningHash = Animator.StringToHash("IsRunning");
    public PlayerStunState(PlayerController controller)
    {
        _controller = controller;
    }
    public void Enter()
    {
        if(_controller!=null && _controller.Animator != null)
        {
            _controller.Animator.SetBool(IsRunningHash, false);
        }
    }

    public void Execute()
    {
        if (_controller == null || _controller.PlayerStateManager == null) return;
        if (_controller.CrowdManagerEffect != null && _controller.CrowdManagerEffect.IsStunned) return;
        if (InputSystem.Instance == null) return;

        if (_controller.DashExplosionSkill != null && _controller.DashExplosionSkill.GetIsDash())
        {
            _controller.PlayerStateManager.ChangeState(_controller.MoveState);
            return;
        }

        bool hasInput = InputSystem.Instance.GetHorizontal() != 0f || InputSystem.Instance.GetVertical() != 0f;
        if (hasInput)
        {
            _controller.PlayerStateManager.ChangeState(_controller.MoveState);
        }
        else if (InputSystem.Instance.GetIsAttack() && _controller.PlayerShooting != null && _controller.PlayerShooting.CanFire)
        {
            _controller.PlayerStateManager.ChangeState(_controller.ShotState);
        }
        else
        {
            _controller.PlayerStateManager.ChangeState(_controller.IdleState);
        }
    }

    public void Exit()
    {
    }
}
