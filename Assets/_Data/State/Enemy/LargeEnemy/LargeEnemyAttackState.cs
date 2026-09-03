using UnityEngine;

public class LargeEnemyAttackState : IState
{
    private readonly LargeEnemyController _controller;
    private static readonly int attack = Animator.StringToHash("Attack");
    private float _timer;
    private float _timeDelay = 0.5f;

    public LargeEnemyAttackState(LargeEnemyController controller)
    {
        _controller = controller;
    }
    public void Enter()
    {
        if (_controller == null || _controller.Animator == null) return;
        _timer = 0f;
        _controller.Animator.SetTrigger(attack);
        if (_controller.LargeAttack != null)
        {
            _controller.LargeAttack.SetIsAttack(false);
        }
    }

    public void Execute()
    {
        if (_controller == null || _controller.StateManager == null || _controller.DamageReceiver == null) return;
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
        _timer += Time.fixedDeltaTime;
        if (_timer < _timeDelay) return;

        if (_controller.LargeMoving != null && _controller.LargeMoving.CheckCanMoving())
        {
            _controller.StateManager.ChangeState(_controller.MoveState);
        }
        else
        {
            _controller.StateManager.ChangeState(_controller.IdleState);
        }
    }

    public void Exit()
    {
    }
}
