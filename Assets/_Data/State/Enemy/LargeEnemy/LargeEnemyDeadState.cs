using UnityEngine;

public class LargeEnemyDeadState : IState
{
    private readonly LargeEnemyController _controller;
    private static readonly int dead = Animator.StringToHash("Dead");
    private float _timer;
    private float _deathDuration = 2.0f;

    public LargeEnemyDeadState(LargeEnemyController controller)
    {
        _controller = controller;
    }

    public void Enter()
    {
        if (_controller == null) return;
        _timer = 0f;
        if (_controller.Animator != null)
        {
            _controller.Animator.SetTrigger(dead);
        }
        if (_controller.DamageReceiver != null)
        {
            _controller.DamageReceiver.ExecuteDead();
        }
    }

    public void Execute()
    {
        if (_controller == null) return;
        _timer += Time.fixedDeltaTime;
        if (_timer >= _deathDuration)
        {
            _controller.gameObject.SetActive(false);
            if (SpawnLargeMinion.Instance != null)
            {
                SpawnLargeMinion.Instance.GoBackList(_controller.gameObject);
            }
        }
    }

    public void Exit()
    {
    }
}
