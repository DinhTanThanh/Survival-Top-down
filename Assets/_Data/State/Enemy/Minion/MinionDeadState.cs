using UnityEngine;

public class MinionDeadState : IState
{
    private readonly MinionController _controller;
    private readonly static int dead = Animator.StringToHash("Dead");
    protected float timer = 0f;
    protected float timeDelay = 0.08f;
    public MinionDeadState(MinionController controller)
    {
        _controller = controller;
    }
    public void Enter()
    {
        if (_controller == null || _controller.Animator == null) return;
        this.timer = 0f;
        _controller.Animator.SetTrigger(dead);
    }

    public void Execute()
    {
        this.timer += Time.fixedDeltaTime;
        if (this.timer < this.timeDelay) return;
        _controller.GoBackListMinionDead();
    }

    public void Exit()
    {
    }
}
