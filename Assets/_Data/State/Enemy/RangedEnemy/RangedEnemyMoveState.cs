using UnityEngine;

public class RangedEnemyMoveState : IState
{
    private readonly RangedEnemyController _controller;
    private readonly static int isRunning = Animator.StringToHash("IsRunning");
    public RangedEnemyMoveState(RangedEnemyController controller)
    {
        _controller = controller;
    }
    public void Enter()
    {
        if (_controller == null || _controller.Animator == null) return;
        _controller.Animator.SetBool(isRunning, true);
    }

    public void Execute()
    {
        if (_controller == null || _controller.RangedEnemyMoving == null||_controller.DamageReceiver==null) return;
        if (!_controller.RangedEnemyMoving.IsReachedLimit() && !_controller.RangedEnemyAttack.GetAttack())
        {
            _controller.RangedEnemyMoving.RangedMoving();
        }
        if (_controller.DamageReceiver.GetIsDead())
        {
            _controller.StateManager.ChangeState(_controller.DeadState);
        }
        if (_controller.RangedEnemyAttack.isReachedAttackRange() && !_controller.RangedEnemyAttack.GetAttack())
        {
            _controller.StateManager.ChangeState(_controller.AttackState);
        }
        if (_controller.RangedEnemyAttack.GetAttack())
        {
            _controller.StateManager.ChangeState(_controller.IdleState);
        }
    }

    public void Exit()
    {
        if (_controller == null || _controller.Animator == null) return;
        _controller.Animator.SetBool(isRunning, false);
    }
}
