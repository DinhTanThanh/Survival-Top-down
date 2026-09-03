using UnityEngine;

public class MinionIdleState : IState
{
    private readonly MinionController _controller;
    private readonly static int move = Animator.StringToHash("IsMoving");
    public MinionIdleState(MinionController controller)
    {
        _controller = controller;
    }
    public void Enter()
    {
        if (_controller == null || _controller.Animator == null) return;
        _controller.Animator.SetBool(move, false);
    }

    public void Execute()
    {
        if (_controller == null || _controller.StateManager == null || _controller.DamageReceiver == null || _controller.MinionnMoving == null) return;
        if (_controller.DamageReceiver.GetIsTakeDamage())
        {
            _controller.StateManager.ChangeState(_controller.GetHitState);
            return;
        }
        if (_controller.DamageReceiver.GetIsDead())
        {
            _controller.StateManager.ChangeState(_controller.DeadState);
            return;
        }
        
        _controller.MinionnMoving.FaceTarget();

        if (_controller.MinionAttack.GetIsAttack() && _controller.MinionAttack.IsReachedAttackRange() && _controller.MinionAttack.IsReachedAngletAttack())
        {
            _controller.StateManager.ChangeState(_controller.AttackState);
            return;
        }
        if (!_controller.MinionnMoving.IsReachedTargetLimit())
        {
            _controller.StateManager.ChangeState(_controller.MoveState);
            return;
        }
    }

    public void Exit()
    {
    }
}
