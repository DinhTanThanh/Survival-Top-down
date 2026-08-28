using UnityEngine;

public class MinionAttackState : IState
{
    private readonly MinionController _controller;
    private readonly static int attack = Animator.StringToHash("Attack");
    public MinionAttackState(MinionController controller)
    {
        _controller = controller;
    }
    public void Enter()
    {
    }

    public void Execute()
    {
    }

    public void Exit()
    {
    }
}
