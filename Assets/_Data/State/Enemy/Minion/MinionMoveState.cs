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
    }

    public void Execute()
    {
    }

    public void Exit()
    {
    }
}
