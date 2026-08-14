using UnityEngine;

public class CoolDown : LoadMonoBehaviour
{
    [SerializeField] protected float timer;
    [SerializeField] protected float timeDelay;
    protected virtual void SetTimeDelay(float timeDelay)
    {
        this.timeDelay = timeDelay;
    }
    protected virtual bool Timing()
    {
        this.timer += Time.deltaTime;
        if (this.timer < this.timeDelay) return false;
        this.timer = 0f;
        return true;
    }
}
