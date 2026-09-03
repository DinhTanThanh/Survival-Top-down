using UnityEngine;

public class LargeEnemyDeadState : IState
{
    private readonly LargeEnemyController _controller;
    private static readonly int dead = Animator.StringToHash("Dead");
    public LargeEnemyDeadState(LargeEnemyController controller)
    {
        _controller = controller;
    }
    public void Enter()
    {
        if (_controller == null || _controller.Animator == null) return;
        _controller.Animator.SetTrigger(dead);
        if (_controller.DamageReceiver != null)
        {
            _controller.DamageReceiver.ExecuteDead();
            _controller.gameObject.SetActive(false);
            SpawnLargeMinion.Instance.GoBackList(_controller.gameObject);
        }
    }

    public void Execute()
    {
    }

    public void Exit()
    {
    }
}
