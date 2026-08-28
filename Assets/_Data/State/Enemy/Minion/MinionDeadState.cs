using UnityEngine;

public class MinionDeadState : IState
{
    private readonly MinionController _controller;
    private readonly static int dead = Animator.StringToHash("Dead");
    public MinionDeadState(MinionController controller)
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
