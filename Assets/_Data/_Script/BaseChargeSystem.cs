using UnityEditor.MPE;
using UnityEditor.ShaderGraph.Drawing.Inspector.PropertyDrawers;
using UnityEngine;

public class BaseChargeSystem : LoadMonoBehaviour
{
    [SerializeField] protected float timer;
    protected virtual int ChargeRecovery(int currentCharge,int maxCharge,float timeDelay)
    {
        if (this.CheckReachedCharge(currentCharge,maxCharge)) return maxCharge;
        if (!this.Timing(timeDelay)) return currentCharge;
        currentCharge++;
        return currentCharge;
    }
    protected bool CheckReachedCharge(int currenCharge,int maxCharge)
    {
        if (currenCharge >= maxCharge) return true;
        return false;
    }
    protected virtual bool Timing(float timeDelay)
    {
        this.timer += Time.deltaTime;
        if (this.timer < timeDelay) return false;
        this.timer = 0f;
        return true;
    }
}
