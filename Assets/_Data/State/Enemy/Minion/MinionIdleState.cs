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

    }

    public void Exit()
    {
    }
}
