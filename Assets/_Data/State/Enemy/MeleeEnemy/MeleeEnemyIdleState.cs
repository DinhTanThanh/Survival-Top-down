using UnityEngine;

public class MeleeEnemyIdleState : IState
{
    private readonly MeleeEnemyController _controller;
    private readonly static int isRunningHash = Animator.StringToHash("IsRunning");
    public MeleeEnemyIdleState(MeleeEnemyController controller)
    {
        _controller = controller;
    }
    public void Enter()
    {
        if (_controller == null || _controller.Animator == null) return;
        _controller.Animator.SetBool(isRunningHash, false);
    }

    public void Execute()
    {
        if (_controller == null || _controller.MeleeEnemyMoving == null||_controller.MeleeEnemyAttack==null||_controller.DamageReceiver==null) return;
        if (_controller.DamageReceiver.GetIsDead())
        {
            _controller.StateManager.ChangeState(_controller.DeadState);
        }
        if (!_controller.MeleeEnemyAttack.GetIsAttack())
        {
            if (!_controller.MeleeEnemyMoving.IsReachedLimit())
            {
                _controller.StateManager.ChangeState(_controller.MoveState);
            }
        }
        
        if (_controller.MeleeEnemyAttack.IsReachedDistance() && _controller.MeleeEnemyAttack.IsReachedAngleAttack() && !_controller.MeleeEnemyAttack.GetIsAttack())
        {
            _controller.StateManager.ChangeState(_controller.AttackState);
        }
    }

    public void Exit()
    {
        
    }
}
