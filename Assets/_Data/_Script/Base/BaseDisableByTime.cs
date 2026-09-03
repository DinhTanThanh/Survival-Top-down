using UnityEngine;

public class BaseDisableByTime : LoadMonoBehaviour
{
    [SerializeField] protected float timer;
    [SerializeField] protected float delayTime;
    protected virtual void SetTimer(float timer)
    {
        this.timer = timer;
    }
    protected virtual void SetDelayTime(float delayTime)
    {
        this.delayTime = delayTime;
    }
    protected virtual bool Timing()
    {
        this.timer += Time.deltaTime;
        if (this.timer < this.delayTime) return false;
        this.timer = 0f;
        return true;
    }
}
