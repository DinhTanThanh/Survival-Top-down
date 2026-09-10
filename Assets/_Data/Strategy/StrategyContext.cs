using UnityEngine;

public class StrategyContext : LoadMonoBehaviour
{
    protected IStrategy currentStrategy;

    public virtual IStrategy CurrentStrategy => currentStrategy;

    public virtual void SetStrategy(IStrategy strategy)
    {
        this.currentStrategy = strategy;
    }

    public virtual void ExecuteStrategy()
    {
        if (this.currentStrategy != null)
        {
            this.currentStrategy.Execute();
        }
    }
}
