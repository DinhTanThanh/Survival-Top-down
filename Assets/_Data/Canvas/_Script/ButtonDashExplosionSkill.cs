using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class ButtonDashExplosionSkill : BaseButton
{
    [SerializeField] protected float timer;
    [SerializeField] protected float elapsedTime;
    [SerializeField] protected float coolDown;
    [SerializeField] protected float explosionRedius;
    [SerializeField] protected float dashDistance;
    [SerializeField] protected float dashDuration;
    [SerializeField] protected bool canUseSkill;
    [SerializeField] protected Transform player;
    [SerializeField] protected GameObject rangeIndicator;
    [SerializeField] protected Rigidbody rb;
    [SerializeField] protected TextMeshProUGUI textMeshProUGUI;
    [SerializeField] protected DashExplosionData dashExplosionData;
    [SerializeField] protected PlayerController playerController;
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
        this.LoadRigidbody();
        this.SetExplosionRadius(this.dashExplosionData.ExplosionRadius);
        this.SetDashDistance(this.dashExplosionData.DashDistance);
        this.SetDashDuration(this.dashExplosionData.DashDuration);
        this.SetCoolDown(this.dashExplosionData.CoolDown);
    }
    protected virtual void SetExplosionRadius(float explosionRadius)
    {
        this.explosionRedius= explosionRadius;
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
    protected virtual void LoadRigidbody()
    {
        if (this.playerController == null) return;
        if (this.rb != null) return;
        this.rb=this.playerController.GetComponent<Rigidbody>();
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
    private void Update()
    {
        if (Keyboard.current != null && (Keyboard.current.lKey.wasPressedThisFrame || Keyboard.current.leftShiftKey.wasPressedThisFrame))
        {
            this.ExecuteDashExplosionSkill();
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
    public virtual void ExecuteDashExplosionSkill()
    {
        if (!this.canUseSkill) return;
        StartCoroutine(PerformDash());
    }
    IEnumerator PerformDash()
    {
        PlayerMoving playerMoving = this.playerController != null ? this.playerController.GetComponentInChildren<PlayerMoving>() : null;

        if (playerMoving != null) playerMoving.IsDashing = true;
        if (InputSystem.Instance != null) InputSystem.Instance.IsInputFrozen = true;

        Camera mainCam = Camera.main;
        Vector3 camForward = Vector3.forward;
        if (mainCam != null)
        {
            camForward = mainCam.transform.forward;
            camForward.y = 0f;
            if (camForward == Vector3.zero) camForward = Vector3.forward;
            camForward.Normalize();
        }

        Vector3 dashDirection = this.player != null ? this.player.forward : transform.forward;
        dashDirection.y = 0f;

        if (Vector3.Dot(dashDirection, camForward) < -0.1f)
        {
            dashDirection = camForward;
        }

        if (dashDirection == Vector3.zero) dashDirection = camForward;
        dashDirection.Normalize();

        Vector3 posStart = this.rb != null ? this.rb.position : (this.player != null ? this.player.position : transform.position);
        Vector3 posDestination = posStart + dashDirection * this.dashDistance;
        Quaternion dashRotation = Quaternion.LookRotation(dashDirection);

        if (this.playerController != null && this.playerController.Animator != null)
        {
            this.playerController.Animator.SetTrigger("IsRunGuard");
        }

        this.elapsedTime = 0f;
        while (this.elapsedTime <= this.dashDuration)
        {
            if (this.rb != null)
            {
                this.rb.linearVelocity = Vector3.zero;
                this.rb.MoveRotation(dashRotation);
            }

            float t = this.dashDuration > 0f ? (this.elapsedTime / this.dashDuration) : 1f;
            Vector3 nextPos = Vector3.Lerp(posStart, posDestination, t);

            if (this.rb != null)
            {
                this.rb.MovePosition(nextPos);
            }
            else if (this.player != null)
            {
                this.player.position = nextPos;
            }

            this.elapsedTime += Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }

        if (this.rb != null)
        {
            this.rb.linearVelocity = Vector3.zero;
            this.rb.MoveRotation(dashRotation);
            this.rb.MovePosition(posDestination);
        }
        else if (this.player != null)
        {
            this.player.position = posDestination;
        }

        this.elapsedTime = 0f;

        if (playerMoving != null) playerMoving.IsDashing = false;
        if (InputSystem.Instance != null) InputSystem.Instance.IsInputFrozen = false;
        this.canUseSkill = false;
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
}
