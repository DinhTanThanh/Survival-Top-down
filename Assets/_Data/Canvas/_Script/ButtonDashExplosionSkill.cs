using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class ButtonDashExplosionSkill : BaseButton
{
    [SerializeField] protected float timer;
    [SerializeField] protected float elapsedTime;
    [SerializeField] protected float coolDown;
    [SerializeField] protected float explosionRedius;
    [SerializeField] protected float dashDistance;
    [SerializeField] protected float dashDuration;
    [SerializeField] protected bool canUseSkill;
    [SerializeField] protected bool isElapsed;
    [SerializeField] protected bool isPressedDash;
    [SerializeField] protected bool isDash;
    [SerializeField] protected Transform player;
    [SerializeField] protected GameObject rangeIndicator;
    [SerializeField] protected Rigidbody rb;
    [SerializeField] protected TextMeshProUGUI textMeshProUGUI;
    [SerializeField] protected DashExplosionData dashExplosionData;
    [SerializeField] protected PlayerController playerController;
    [SerializeField] protected PlayerEventAnimation playerEventAnimation;
    protected Vector3 posStart;
    protected Vector3 posDestination;
    public SkillData SkillData => dashExplosionData;
    protected override void LoadComponent()
    {
        base.LoadComponent();
        this.SetCanUseSkill(true);
        this.LoadRangIndicator();
        this.LoadDashExplosionData();
        this.LoadPlayer();
        this.LoadTextMeshProUGUI();
        this.LoadPlayerController();
        this.LoadPlayerEventAnimation();
        this.LoadRigidbody();
        this.SetExplosionRadius(this.dashExplosionData.ExplosionRadius);
        this.SetDashDistance(this.dashExplosionData.DashDistance);
        this.SetDashDuration(this.dashExplosionData.DashDuration);
        this.SetCoolDown(this.dashExplosionData.CoolDown);
    }
    protected virtual void LoadPlayerEventAnimation()
    {
        if (this.playerEventAnimation != null) return;
        this.playerEventAnimation = FindFirstObjectByType<PlayerEventAnimation>();
        Debug.LogWarning(transform.name + " : LoadPlayerEventAnimation");
    }
    private void Update()
    {
        if(Keyboard.current!=null && Keyboard.current.lKey.isPressed)
        {
            this.EnableIsPressDash();
        }
        if (this.canUseSkill) return;
        if (!this.Timing())
        {
            this.textMeshProUGUI.text = (this.coolDown - this.timer).ToString("F1");
            return;
        }
        this.textMeshProUGUI.text = "";
        this.canUseSkill = true;
    }
    private void FixedUpdate()
    {
        if (this.isPressedDash)
        {
            this.ExecuteDashExplosionSkill();
        }
    }
    protected void ExecuteDashExplosionSkill()
    {
        if (!this.isElapsed)
        {
            Vector3 dashDirection = this.playerController != null ? this.playerController.transform.forward : Vector3.forward;
            float h = InputSystem.Instance != null ? InputSystem.Instance.GetHorizontal() : 0f;
            float v = InputSystem.Instance != null ? InputSystem.Instance.GetVertical() : 0f;
            Camera mainCam = Camera.main;
            if (h != 0f || v != 0f)
            {
                Vector3 camForward = mainCam != null ? mainCam.transform.forward : Vector3.forward;
                Vector3 camRight = mainCam != null ? mainCam.transform.right : Vector3.right;
                camForward.y = 0f;
                camRight.y = 0f;
                Vector3 inputDir = (camForward.normalized * v) + (camRight.normalized * h);
                if (inputDir.sqrMagnitude > 0.001f)
                {
                    dashDirection = inputDir.normalized;
                }
            }
            dashDirection.y = 0f;
            if (dashDirection == Vector3.zero) dashDirection = Vector3.forward;

            Quaternion dashRotation = Quaternion.LookRotation(dashDirection);
            if (this.rb != null)
            {
                this.rb.rotation = dashRotation;
            }
            if (this.playerController != null)
            {
                this.playerController.transform.rotation = dashRotation;
            }

            this.posStart = this.rb != null ? this.rb.position : (this.player != null ? this.player.position : Vector3.zero);
            this.posDestination = this.posStart + dashDirection * this.dashDistance;
            this.elapsedTime = 0f;
            this.isElapsed = true;
        }

        if (this.isElapsed)
        {
            float t = this.elapsedTime / this.dashDuration;
            t = Mathf.Clamp01(t);
            Vector3 nextPos = Vector3.Lerp(this.posStart, this.posDestination, t);
            if (this.rb != null)
            {
                this.rb.MovePosition(nextPos);
            }

            this.elapsedTime += Time.fixedDeltaTime;
            if (this.elapsedTime >= this.dashDuration)
            {
                if (this.rb != null)
                {
                    this.rb.MovePosition(this.posDestination);
                }
                if (this.playerEventAnimation != null)
                {
                    this.playerEventAnimation.Explosion();
                }
                this.isElapsed = false;
                this.elapsedTime = 0f;
                this.canUseSkill = false;
                this.isPressedDash = false;
                this.isDash = false;
            }
        }
    }
    public virtual void EnableIsPressDash()
    {
        if (!this.canUseSkill || this.isDash) return;
        this.isPressedDash = true;
        this.isDash = true;

        if (this.playerController != null)
        {
            if (this.playerController.Animator != null)
            {
                this.playerController.Animator.SetTrigger("IsRunGuard");
                this.playerController.Animator.SetBool("IsRunning", true);
            }
            if (this.playerController.PlayerStateManager != null)
            {
                this.playerController.PlayerStateManager.ChangeState(this.playerController.MoveState);
            }
        }
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(this.player.position, this.explosionRedius);
    }
    public virtual void EnableRangeIndicator()
    {
        this.rangeIndicator.SetActive(true);
    }
    public virtual void DisableRangeIndicator()
    {
        this.rangeIndicator.SetActive(false);
    }
    protected virtual bool Timing()
    {
        this.timer += Time.deltaTime;
        if (this.timer < this.coolDown) return false;
        this.timer = 0;
        return true;
    }
    protected virtual void SetExplosionRadius(float explosionRadius)
    {
        this.explosionRedius = explosionRadius;
    }
    protected virtual void SetDashDuration(float dashDuration)
    {
        this.dashDuration = dashDuration;
    }
    protected virtual void SetDashDistance(float dashDistance)
    {
        this.dashDistance = dashDistance;
    }
    protected virtual void SetCoolDown(float coolDown)
    {
        this.coolDown = coolDown;
    }
    protected virtual void SetCanUseSkill(bool canUseSkill)
    {
        this.canUseSkill = canUseSkill;
    }
    public virtual bool GetIsDash()
    {
        return this.isDash;
    }
    protected virtual void LoadRigidbody()
    {
        if (this.playerController == null) return;
        if (this.rb != null) return;
        this.rb = this.playerController.GetComponent<Rigidbody>();
        Debug.LogWarning(transform.name + " : LoadRigidbody");
    }
    protected virtual void LoadPlayer()
    {
        if (this.player != null) return;
        this.player = GameObject.Find("Player")?.transform;
        Debug.LogWarning(transform.name + " : LoadPlayer");
    }
    protected virtual void LoadRangIndicator()
    {
        if (this.rangeIndicator != null) return;
        this.rangeIndicator = GameObject.Find("RangeIndicator");
        Debug.LogWarning(transform.name + " : LoadRangeIndicator");
    }
    protected virtual void LoadDashExplosionData()
    {
        if (this.dashExplosionData != null) return;
        this.dashExplosionData = Resources.Load<DashExplosionData>("WeaponData/DashExplosionData");
        Debug.LogWarning(transform.name + " : LoadDashExplosionData");
    }
    protected virtual void LoadTextMeshProUGUI()
    {
        if (this.textMeshProUGUI != null) return;
        this.textMeshProUGUI = GetComponentInChildren<TextMeshProUGUI>();
        Debug.LogWarning(transform.name + " : LoadTextMesh");
    }
    protected virtual void LoadPlayerController()
    {
        if (this.playerController != null) return;
        this.playerController = FindFirstObjectByType<PlayerController>();
        Debug.LogWarning(transform.name + " : LoadPlayerController");
    }
}
