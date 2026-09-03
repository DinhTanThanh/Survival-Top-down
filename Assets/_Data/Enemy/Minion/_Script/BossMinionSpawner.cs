using System.Collections.Generic;
using UnityEngine;

public class BossMinionSpawner : LoadMonoBehaviour
{
    [SerializeField] protected int maxMinion = 4;
    [SerializeField] protected float spawnRadius = 3f;
    [SerializeField] protected float attackSurroundRadius = 0.8f;
    [SerializeField] protected float respawnTimer = 0f;
    [SerializeField] protected float autoRespawnDelay = 35f;
    [SerializeField] protected bool hasReSpawn;
    [SerializeField] protected GameObject minionPrefab;
    [SerializeField] protected LargeEnemyController bossController;
    [SerializeField] protected List<MinionController> activeMinions=new List<MinionController>();
    public List<MinionController> ActiveMinions => activeMinions;
    protected override void LoadComponent()
    {
        base.LoadComponent();
        this.LoadLargeEnemyController();
        this.SetHasRespawn(true);
    }
    private void Start()
    {
        this.SpawnAllMinions();
    }
    private void Update()
    {
        this.CleanDeadMinions();
        this.HandleAutoRespawn();
    }
    protected virtual MinionController SpawnSingleMinion()
    {
        if (this.minionPrefab == null) return null;
        int index = this.activeMinions.Count;
        float angle = index * (360f / this.maxMinion) * Mathf.Deg2Rad;
        Vector3 offset=new Vector3(Mathf.Cos(angle),0,Mathf.Sin(angle))*this.spawnRadius;
        Vector3 spawnPosition = this.transform.parent.position + offset;
        if (SpawnEnemyPoolingManager.Instance == null) return null;
        MinionController minion = SpawnEnemyPoolingManager.Instance.SpawnEnemy(this.minionPrefab, spawnPosition, Quaternion.identity);
        minion.SetBossOwner(this.bossController.transform);
        this.activeMinions.Add(minion);
        return minion;
    }
    public virtual void SpawnAllMinions()
    {
        int needed = this.maxMinion - this.activeMinions.Count;
        for(int i = 0; i < needed; i++)
        {
            this.SpawnSingleMinion();
        }
    }
    public virtual void HandleAutoRespawn()
    {
        if (!this.hasReSpawn) return;
        if (this.activeMinions.Count >= this.maxMinion)
        {
            this.respawnTimer = 0f;
            return;
        }
        this.respawnTimer += Time.deltaTime;
        if (this.respawnTimer >= this.autoRespawnDelay)
        {
            this.respawnTimer = 0f;
            this.SpawnSingleMinion();
        }
    }
    public virtual void CleanDeadMinions()
    {
        for (int i = this.activeMinions.Count - 1; i >= 0; i--)
        {
            MinionController minion = this.activeMinions[i];
            if (minion == null || (minion.DamageReceiver != null && minion.DamageReceiver.GetIsDead()))
            {
                this.activeMinions.RemoveAt(i);
                if (this.CheckListNone())
                {
                    this.hasReSpawn = false;
                    this.bossController.LargeMoving.SetMaximumDistance(1.5f);
                    this.bossController.LargeMoving.SetMinximumDistance(0.5f);
                }
            }
        }
    }
    public virtual void OrderAttack(Transform target)
    {
        if (target == null) return;
        int count = this.activeMinions.Count;
        for (int i = 0; i < count; i++)
        {
            MinionController minion = this.activeMinions[i];
            if (minion == null) continue;
            minion.MinionnMoving.SetSpeedMovement(minion.EntitySO.baseSpeed);
            float angle = i * (360f / Mathf.Max(count, 1)) * Mathf.Deg2Rad;
            float attackRange = minion.EntitySO.attackRange;
            float radius = Mathf.Min(this.attackSurroundRadius, attackRange * 0.8f);
            Vector3 offset = new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)) * radius;
            minion.ReceiveCommand(new MinionCommand(MinionCommandType.AttackTarget, target, offset, 0));
        }
    }
    public virtual void OrderRecall()
    {
        Transform bossTransform = this.bossController != null ? this.bossController.transform : this.transform;
        int count = this.activeMinions.Count;
        for (int i = 0; i < count; i++)
        {
            MinionController minion = this.activeMinions[i];
            if (minion == null) continue;
            minion.MinionnMoving.SetSpeedMovement(minion.EntitySO.baseSpeed * 2f);
            float angle = i * (360f / Mathf.Max(count, 1)) * Mathf.Deg2Rad;
            Vector3 offset = new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)) * this.spawnRadius;
            minion.ReceiveCommand(new MinionCommand(MinionCommandType.GuardBoss, bossTransform, offset, 0));
        }
    }
    public virtual void HealMinions(float amount)
    {
        for (int i = 0; i < this.activeMinions.Count; i++)
        {
            MinionController minion = this.activeMinions[i];
            if (minion != null && minion.DamageReceiver != null && !minion.DamageReceiver.GetIsDead())
            {
                minion.DamageReceiver.AddHealth(amount);
            }
        }
    }
    protected virtual bool CheckListNone()
    {
        return this.activeMinions.Count <= 0;
    }
    protected virtual void SetHasRespawn(bool hasRespawn)
    {
        this.hasReSpawn = hasRespawn;
    }
    public virtual bool GetHasRespawn()
    {
        return this.hasReSpawn;
    }
    protected virtual void LoadLargeEnemyController()
    {
        if (this.bossController != null) return;
        this.bossController = GetComponentInParent<LargeEnemyController>();
        Debug.LogWarning(transform.name + " : LoadLargeEnemyController");
    }
}
