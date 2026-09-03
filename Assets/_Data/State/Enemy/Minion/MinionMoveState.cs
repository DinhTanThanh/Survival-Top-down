using UnityEngine;

public class MinionMoveState : IState
{
    private readonly MinionController _controller;
    private readonly static int move = Animator.StringToHash("IsMoving");
    public MinionMoveState(MinionController controller)
    {
        _controller = controller;
    }
    public void Enter()
    {
        if (_controller == null || _controller.Animator == null) return;
        _controller.Animator.SetBool(move, true);
    }

    public void Execute()
    {
        if (_controller == null || _controller.StateManager == null || _controller.DamageReceiver == null || _controller.MinionnMoving == null) return;
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

        if (_controller.MinionAttack != null && _controller.MinionAttack.GetIsAttack() && _controller.MinionAttack.IsReachedAttackRange() && _controller.MinionAttack.IsReachedAngletAttack())
        {
            _controller.StateManager.ChangeState(_controller.AttackState);
            return;
        }

        if (!_controller.MinionnMoving.IsReachedTargetLimit())
        {
            _controller.MinionnMoving.Moving();
        }
        else
        {
            _controller.StateManager.ChangeState(_controller.IdleState);
        }
    }

    public void Exit()
    {
        if(_controller==null || _controller.Animator == null) return;
        _controller.Animator.SetBool(move, false);
    }
}
