using UnityEngine;

public class LargeEnemyAttackState : IState
{
    private readonly LargeEnemyController _controller;
    private float _timer;
    private float _timeDelay = 0.75f;

    public LargeEnemyAttackState(LargeEnemyController controller)
    {
        _controller = controller;
    }

    public void Enter()
    {
        if (_controller == null) return;
        _timer = 0f;

        if (_controller.LargeAttack != null)
        {
            _timeDelay = _controller.LargeAttack.ExecuteAttack();
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
