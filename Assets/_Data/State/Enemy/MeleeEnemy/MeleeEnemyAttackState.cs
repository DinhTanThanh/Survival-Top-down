using UnityEngine;

public class MeleeEnemyAttackState : IState
{
    private readonly MeleeEnemyController _controller;
    private readonly static int attack = Animator.StringToHash("Attack");
    public MeleeEnemyAttackState(MeleeEnemyController controller)
    {
        _controller = controller;
    }

    public void Enter()
    {
        if (_controller == null || _controller.Animator == null) return;
        _controller.Animator.SetTrigger(attack);
    }

    public void Execute()
    {
        if (_controller == null || _controller.MeleeEnemyAttack == null) return;
        if (_controller.DamageReceiver.GetIsDead())
        {
            _controller.StateManager.ChangeState(_controller.DeadState);
        }
        if (_controller.MeleeEnemyAttack.GetIsAttack())
        {
            _controller.StateManager.ChangeState(_controller.IdleState);
        }
    }

    public void Exit()
    {
          
    }
}
