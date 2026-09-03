using UnityEngine;

public class MinionAttackState : IState
{
    private float _timer;
    private float _timeDelay=0.3f;
    private readonly MinionController _controller;
    private readonly static int attack = Animator.StringToHash("Attack");
    public MinionAttackState(MinionController controller)
    {
        _controller = controller;
    }
    public void Enter()
    {
        if (_controller == null || _controller.Animator == null) return;
        _timer = 0f;
        _controller.Animator.SetTrigger(attack);
        _controller.MinionAttack.SetIsAttack(false);
    }

    public void Execute()
    {
        if (_controller == null || _controller.StateManager == null || _controller.DamageReceiver == null) return;
        if (_controller.DamageReceiver.GetIsDead())
        {
            _controller.StateManager.ChangeState(_controller.DeadState);
        }
        _timer += Time.fixedDeltaTime;
        if (_timer < _timeDelay) return;
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
