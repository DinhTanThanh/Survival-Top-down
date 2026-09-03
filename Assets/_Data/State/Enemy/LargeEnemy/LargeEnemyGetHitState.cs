using UnityEngine;

public class LargeEnemyGetHitState : IState
{
    private readonly LargeEnemyController _controller;
    private static readonly int gethit = Animator.StringToHash("GetHit");
    private float _timer;
    private float _hitDuration = 0.35f;

    public LargeEnemyGetHitState(LargeEnemyController controller)
    {
        _controller = controller;
    }
    public void Enter()
    {
        if (_controller == null || _controller.Animator == null) return;
        _timer = 0f;
        _controller.Animator.SetTrigger(gethit);
        if (_controller.DamageReceiver != null)
        {
            _controller.DamageReceiver.SetIsTakeDamage(false);
        }
        _controller.SwitchToDirectAttack();
    }

    public void Execute()
    {
        if (_controller == null || _controller.StateManager == null || _controller.DamageReceiver == null) return;
        if (_controller.DamageReceiver.GetIsDead())
        {
            _controller.StateManager.ChangeState(_controller.DeadState);
            return;
        }
        _timer += Time.fixedDeltaTime;
        if (_timer < _hitDuration) return;

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
