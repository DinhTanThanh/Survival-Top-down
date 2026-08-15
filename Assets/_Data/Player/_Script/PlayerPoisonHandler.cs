using UnityEngine;

public class PlayerPoisonHandler : LoadMonoBehaviour
{
    [SerializeField] protected int numberTick;
    [SerializeField] protected float timer;
    [SerializeField] protected float timeDelay;
    [SerializeField] protected bool isRefresh;
    [SerializeField] protected float baseDamagePoison;
    [SerializeField] protected PlayerController playerController;
    protected override void LoadComponent()
    {
        base.LoadComponent();
        this.LoadPlayerController();
    }
    protected virtual void LoadPlayerController()
    {
        if (this.playerController != null) return;
        this.playerController = GetComponentInParent<PlayerController>();
        Debug.LogWarning(transform.name + " : LoadPlayerController");
    }
    public virtual void SetBaseDamagePoison(float baseDamagePoison)
    {
        this.baseDamagePoison = baseDamagePoison;
    }
    protected virtual void SetNumebrTick(int numberTick)
    {
        this.numberTick = numberTick;
    }
    protected virtual void SetTimer(int timer)
    {
        this.timer = timer;
    }
    protected virtual void SetTimeDelay(float timeDelay)
    {
        this.timeDelay = timeDelay;
    }
    public virtual void SetIsRefresh(bool isRefresh)
    {
        this.isRefresh = isRefresh;
    }
    private void Update()
    {
        if (!this.isRefresh) return;
        this.HandlePoison();
    }
    protected virtual void HandlePoison()
    {
        if (this.baseDamagePoison == 0)
        {
            Debug.Log("base dame hien tai dang bang 0");
        }
        if (this.numberTick <= 0)
        {
            this.SetIsRefresh(false);
            return;
        }
        if (!this.Timing()) return;
        this.playerController.DamageReceiver.ReduceHp(this.baseDamagePoison);
        this.numberTick--;
    }
    public virtual void Refresh()
    {
        this.SetIsRefresh(true);
        this.SetNumebrTick(3);
        this.SetTimer(0);
        this.SetTimeDelay(1f);
    }
    protected virtual bool Timing()
    {
        this.timer += Time.deltaTime;
        if (this.timer < this.timeDelay) return false;
        this.timer = 0f;
        return true;
    }
}
