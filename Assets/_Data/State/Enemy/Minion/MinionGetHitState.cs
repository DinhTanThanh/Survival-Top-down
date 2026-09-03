using UnityEngine;

public class MinionGetHitState : IState
{
    private readonly MinionController _controller;
    private readonly static int gethit = Animator.StringToHash("GetHit");
    private float _timer;
    private float _hitDuration = 0.35f;
    public MinionGetHitState(MinionController controller)
    {
        _controller = controller;
    }
    public void Enter()
    {
        if (_controller == null || _controller.Animator == null) return;
        _timer = 0f;
        _controller.Animator.SetTrigger(gethit);
        _controller.DamageReceiver.SetIsTakeDamage(false);
    }

    public void Execute()
    {
        if (_controller == null || _controller.StateManager == null || _controller.DamageReceiver == null) return;
        if (_controller.DamageReceiver.GetIsDead())
        {
            _controller.StateManager.ChangeState(_controller.DeadState);
        }
        _timer += Time.fixedDeltaTime;
        if (_timer < _hitDuration) return;
        if (!_controller.MinionnMoving.IsReachedTargetLimit())
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
