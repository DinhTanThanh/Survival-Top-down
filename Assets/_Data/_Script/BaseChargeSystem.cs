using System.Collections.Generic;
using UnityEngine;

public class BaseChargeSystem : LoadMonoBehaviour
{
    [SerializeField] protected float timer;
    [SerializeField] protected PlayerShooting playerShooting;
    [SerializeField] protected List<IChargeObserver> listChargeObserver = new List<IChargeObserver>();
    protected override void LoadComponent()
    {
        base.LoadComponent();
        this.LoadPlayerShooting();
    }
    protected virtual void LoadPlayerShooting()
    {
        if (this.playerShooting != null) return;
        this.playerShooting = GetComponentInParent<PlayerShooting>();
        Debug.LogWarning(transform.name + " : LoadPlayerShooting");
    }
    public virtual void AddChargeObserver(IChargeObserver chargeObserver)
    {
        this.listChargeObserver.Add(chargeObserver);
    }
    protected virtual void OnChangeChargeUI()
    {
        foreach (IChargeObserver chargeObserver in this.listChargeObserver)
        {
            chargeObserver.UpdateCharge();
            Debug.Log("==============================");
            Debug.Log(this.playerShooting.MaxCharge);
            Debug.Log(this.playerShooting.CurrentCharge);
            Debug.Log("thuc hien trong BaseChargeSystem");
            Debug.Log("==============================");
        }
    }
    private void Update()
    {
        this.ChargeRecovery();
    }
    protected virtual void ChargeRecovery()
    {
        if (this.CheckReachedCharge(this.playerShooting.CurrentCharge,this.playerShooting.MaxCharge)) return;
        if (!this.Timing(this.playerShooting.ChargeRegenTime)) return;
        this.playerShooting.CurrentCharge++;
        this.OnChangeChargeUI();
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
