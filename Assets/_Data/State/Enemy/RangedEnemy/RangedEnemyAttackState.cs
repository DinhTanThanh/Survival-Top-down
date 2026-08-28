using UnityEngine;
public class RangedEnemyAttackState : IState
{
    private readonly RangedEnemyController _controller;
    private readonly static int attack = Animator.StringToHash("Attack");
    public RangedEnemyAttackState(RangedEnemyController controller)
    {
        _controller = controller;
    }
    public void Enter()
    {
        if (_controller == null && _controller.Animator == null) return;
        _controller.Animator.SetTrigger(attack);
    }

    public void Execute()
    {
        if (_controller == null || _controller.RangedEnemyMoving == null||_controller.DamageReceiver==null) return;
        if (_controller.DamageReceiver.GetIsDead())
        {
            _controller.StateManager.ChangeState(_controller.DeadState);
        }
        if (_controller.RangedEnemyAttack.GetAttack())
        {
            _controller.StateManager.ChangeState(_controller.IdleState);
        }
    }

    public void Exit()
    {
    }
}
