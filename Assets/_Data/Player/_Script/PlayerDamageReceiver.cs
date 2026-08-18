using TMPro;
using UnityEngine;

public class PlayerDamageReceiver : DamageReceiver
{
    [SerializeField] protected PlayerController playerController;
    [SerializeField] protected TextMeshProUGUI deadNoticeText;
    [SerializeField] protected string deathMessage = "PLAYER DIED (KEEP PLAYING)";
    [SerializeField] protected bool hasShownNotice = false;

    protected override void LoadComponent()
    {
        base.LoadComponent();
        this.LoadPlayerController();
        this.LoadDeadNoticeText();
        this.SetBaseHp(this.playerController.EntitySO.baseHp);
        this.SetHp(this.playerController.EntitySO.baseHp);
        this.SetDamageMultiplier(this.playerController.EntitySO.damageMultiplier);
        this.SetDefence(this.playerController.EntitySO.baseDefence);
    }

    protected virtual void LoadPlayerController()
    {
        if (this.playerController != null) return;
        this.playerController = GetComponentInParent<PlayerController>();
        Debug.LogWarning(transform.name + " : LoadPlayerController");
    }

    protected virtual void LoadDeadNoticeText()
    {
        if (this.deadNoticeText != null) return;
        GameObject noticeObj = GameObject.Find("DeadNoticeText");
        if (noticeObj != null)
        {
            this.deadNoticeText = noticeObj.GetComponent<TextMeshProUGUI>();
        }
    }

    protected override void Update()
    {
        if (this.isDead)
        {
            this.ShowDeadNotice();
        }
        else
        {
            this.HideDeadNotice();
        }
    }

    protected virtual void ShowDeadNotice()
    {
        if (this.deadNoticeText != null)
        {
            this.deadNoticeText.gameObject.SetActive(true);
            this.deadNoticeText.text = this.deathMessage;
        }

        if (!this.hasShownNotice)
        {
            this.hasShownNotice = true;
            Debug.LogWarning("[PLAYER DIED] " + this.deathMessage);
        }
    }

    protected virtual void HideDeadNotice()
    {
        if (this.hasShownNotice)
        {
            this.hasShownNotice = false;
            if (this.deadNoticeText != null)
            {
                this.deadNoticeText.gameObject.SetActive(false);
            }
        }
    }
}
