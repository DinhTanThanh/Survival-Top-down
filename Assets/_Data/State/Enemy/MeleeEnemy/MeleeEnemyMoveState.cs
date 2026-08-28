using UnityEngine;

public class MeleeEnemyMoveState : IState
{
    private readonly MeleeEnemyController _controller;
    private readonly static int isRunning = Animator.StringToHash("IsRunning");
    public MeleeEnemyMoveState(MeleeEnemyController controller)
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
        if (_controller == null || _controller.MeleeEnemyAttack == null) return;
        if(!_controller.MeleeEnemyMoving.IsReachedLimit() && !_controller.MeleeEnemyAttack.GetIsAttack())
        {
            _controller.MeleeEnemyMoving.MeleeMoving();
        }
        if (_controller.DamageReceiver.GetIsDead())
        {
            _controller.StateManager.ChangeState(_controller.DeadState);
        }
        if (_controller.MeleeEnemyAttack.IsReachedDistance() && _controller.MeleeEnemyAttack.IsReachedAngleAttack() && !_controller.MeleeEnemyAttack.GetIsAttack())
        {
            _controller.StateManager.ChangeState(_controller.AttackState);
        }
        if (_controller.MeleeEnemyAttack.GetIsAttack())
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
