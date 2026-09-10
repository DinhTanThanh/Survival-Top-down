using UnityEngine;

public class LargeEnemyIdleState : IState
{
    private readonly LargeEnemyController _controller;
    private static readonly int move = Animator.StringToHash("IsMoving");

    public LargeEnemyIdleState(LargeEnemyController controller)
    {
        _controller = controller;
    }

    public void Enter()
    {
        if (_controller == null || _controller.Animator == null) return;
        _controller.Animator.SetBool(move, false);
    }

    public void Execute()
    {
        if (_controller == null || _controller.StateManager == null || _controller.DamageReceiver == null || _controller.LargeAttack == null || _controller.LargeMoving == null) return;

        if (_controller.DamageReceiver.GetIsDead())
        {
            _controller.StateManager.ChangeState(_controller.DeadState);
            return;
        }

        if (_controller.DamageReceiver.GetIsTakeDamage())
        {
            _controller.StateManager.ChangeState(_controller.GetHitState);
            return;
        }

        if (_controller.CanDirectAttack() && _controller.LargeAttack.GetIsAttack() && _controller.LargeAttack.IsReachedDistance())
        {
            _controller.StateManager.ChangeState(_controller.AttackState);
            return;
        }

        if (_controller.LargeMoving.CheckCanMoving())
        {
            _controller.StateManager.ChangeState(_controller.MoveState);
            return;
        }
    }

    public void Exit()
    {
    }
}
