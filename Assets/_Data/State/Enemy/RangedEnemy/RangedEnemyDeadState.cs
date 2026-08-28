using UnityEngine;

public class RangedEnemyDeadState : IState
{
    private readonly RangedEnemyController _controller;
    private readonly static int dead = Animator.StringToHash("Dead");
    public RangedEnemyDeadState(RangedEnemyController controller)
    {
        _controller = controller;
    }
    public void Enter()
    {
        if (_controller == null || _controller.Animator == null) return;
        _controller.Animator.SetTrigger(dead);
        _controller.DamageReceiver.ExecuteDead();
    }

    public void Execute()
    {
    }

    public void Exit()
    {
    }
}
