using UnityEngine;

public class BaseDisableByDistance : LoadMonoBehaviour
{
    [SerializeField] protected float distanceLimit;
    [SerializeField] protected Vector3 posStart;
    [SerializeField] protected Vector3 posCurrent;
    protected virtual bool IsMaxDistanceReached()
    {
        if(Vector3.Distance(this.posStart, this.posCurrent) >= this.distanceLimit)
        {
            return true;
        }
        return false;
    }
    protected virtual void SetDistanceLimit(float distanceLimit)
    {
        this.distanceLimit = distanceLimit;
    }
}
