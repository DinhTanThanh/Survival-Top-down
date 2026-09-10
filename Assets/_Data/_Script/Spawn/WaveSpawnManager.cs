using System;
using System.Collections.Generic;
using UnityEngine;

public enum BossType
{
    None,
    Mummy,
    Alien,
    Zombie,
    Elemental,
    Random
}

public enum WaveType
{
    Normal,
    Swarm,
    Boss,
    Elite
}

[System.Serializable]
public class WaveConfig
{
    [Header("Wave Info")]
    public string waveName = "Wave";
    public WaveType waveType = WaveType.Normal;
    public float waveDuration = 30f;
    public float spawnInterval = 2.5f;

    [Header("Enemy Spawning")]
    public int minMeleePerSpawn = 1;
    public int maxMeleePerSpawn = 3;
    public int minRangedPerSpawn = 0;
    public int maxRangedPerSpawn = 2;
    public int maxEnemiesAlive = 20;

    [Header("Boss Settings")]
    public BossType bossType = BossType.None;
    public int bossCount = 1;

    [Header("Swarm / Ambush Event")]
    public bool hasSwarmEvent = false;
    public float swarmTriggerTime = 15f;
    public int swarmMeleeCount = 6;
    public int swarmRangedCount = 3;
}

public class WaveSpawnManager : LoadMonoBehaviour
{
    [Header("--- Wave Configurations ---")]
    [SerializeField] protected List<WaveConfig> waveConfigs = new List<WaveConfig>();
    [SerializeField] protected int currentWaveIndex = 0;
    [SerializeField] protected float waveBreakDuration = 4f;

    [Header("--- Enemy Prefabs & References ---")]
    [SerializeField] protected GameObject meleeEnemy;
    [SerializeField] protected GameObject rangedEnemy;

    [Header("--- Boss Prefabs & References ---")]
    [SerializeField] protected GameObject mummy;
    [SerializeField] protected GameObject alien;
    [SerializeField] protected GameObject zombie;
    [SerializeField] protected GameObject elemental;

    [Header("--- Scene References ---")]
    [SerializeField] protected ManagerSpawnPoint managerSpawnPoint;
    [SerializeField] protected Transform playerTransform;

    [Header("--- Runtime State ---")]
    [SerializeField] protected int numberMeleeEnemyOnScene;
    [SerializeField] protected int numberRangedEnemyOnScene;
    [SerializeField] protected float waveTimer = 0f;
    [SerializeField] protected float spawnTimer = 0f;
    [SerializeField] protected float breakTimer = 0f;
    [SerializeField] protected bool isWaveActive = false;
    [SerializeField] protected bool isBreakTime = false;
    [SerializeField] protected bool hasTriggeredSwarm = false;
    [SerializeField] protected bool isEndlessMode = false;
    [SerializeField] protected List<GameObject> activeBosses = new List<GameObject>();

    [Header("--- Periodic Boss Spawning ---")]
    [SerializeField] protected bool enablePeriodicBossSpawn = true;
    [SerializeField] protected float bossSpawnInterval = 30f;
    [SerializeField] protected float bossSpawnTimer = 0f;

    [Header("--- Enemy Spawn Rate Modifier ---")]
    [Tooltip("Multiplier for enemy spawn interval (higher = slower spawn rate)")]
    [SerializeField] protected float spawnIntervalMultiplier = 1.3f;
    [SerializeField] protected bool hasAllowSpawnLargeMinion;
    [SerializeField] protected bool isSpawnTempt;
    [SerializeField] protected Transform pointerCurrent;

    public static event Action<int, WaveConfig> OnWaveStarted;
    public static event Action<int> OnWaveCompleted;
    public static event Action<GameObject> OnBossSpawned;
    public static event Action<GameObject> OnBossDefeated;
    public static event Action OnSwarmWarning;

    public int CurrentWaveNumber => currentWaveIndex + 1;
    public int CurrentWaveIndex => currentWaveIndex;
    public float WaveTimer => waveTimer;
    public float WaveDuration => GetCurrentWaveConfig() != null ? GetCurrentWaveConfig().waveDuration : 0f;
    public bool IsWaveActive => isWaveActive;
    public bool IsBreakTime => isBreakTime;
    public float BreakTimer => breakTimer;
    public float BreakDuration => waveBreakDuration;
    public int TotalEnemiesOnScene => numberMeleeEnemyOnScene + numberRangedEnemyOnScene;
    public List<GameObject> ActiveBosses => activeBosses;
    public WaveConfig CurrentWaveConfig => GetCurrentWaveConfig();

    protected override void LoadComponent()
    {
        base.LoadComponent();
        this.LoadManagerSpawnPoint();
        this.LoadPlayer();
        this.LoadMeleeEnemy();
        this.LoadRangedEnemy();
        this.LoadBosses();
        this.EnsureDefaultWaves();
    }

    private void Start()
    {
        if (this.waveConfigs.Count == 0)
        {
            this.EnsureDefaultWaves();
        }
        this.StartWave(0);
    }

    private void Update()
    {
        this.CleanDeadBosses();

        if (this.isBreakTime)
        {
            this.HandleBreakTime();
            return;
        }

        if (!this.isWaveActive) return;

        this.HandleWaveTimer();
        this.HandleEnemySpawning();
        this.HandlePeriodicBossSpawning();
        this.HandleSwarmEvent();
        this.CheckWaveCompletion();
    }

    #region Wave Lifecycle & State

    public virtual void StartWave(int waveIndex)
    {
        this.currentWaveIndex = waveIndex;
        this.waveTimer = 0f;
        this.spawnTimer = 0f;
        this.breakTimer = 0f;
        this.bossSpawnTimer = 0f;
        this.hasTriggeredSwarm = false;
        this.isWaveActive = true;
        this.isBreakTime = false;

        WaveConfig config = this.GetCurrentWaveConfig();
        Debug.Log($"[WaveSpawnManager] === STARTING {config.waveName} (Wave {this.CurrentWaveNumber}) - Type: {config.waveType} ===");

        // Spawn Boss if wave is a Boss Wave
        if (config.waveType == WaveType.Boss || config.bossType != BossType.None)
        {
            this.ExecuteSpawnBossWave(config);
        }

        // Trigger Swarm immediately if Swarm Wave
        if (config.waveType == WaveType.Swarm)
        {
            this.TriggerSwarm(config.swarmMeleeCount, config.swarmRangedCount);
            this.hasTriggeredSwarm = true;
        }

        OnWaveStarted?.Invoke(this.currentWaveIndex, config);
    }

    protected virtual void HandleWaveTimer()
    {
        this.waveTimer += Time.deltaTime;
    }

    protected virtual void HandleBreakTime()
    {
        this.breakTimer += Time.deltaTime;
        if (this.breakTimer >= this.waveBreakDuration)
        {
            this.isBreakTime = false;
            this.breakTimer = 0f;
            this.StartNextWave();
        }
    }

    protected virtual void StartNextWave()
    {
        int nextWave = this.currentWaveIndex + 1;
        if (nextWave >= this.waveConfigs.Count)
        {
            this.isEndlessMode = true;
            this.waveConfigs.Add(this.GenerateEndlessWaveConfig(nextWave));
        }
        this.StartWave(nextWave);
    }

    protected virtual void CheckWaveCompletion()
    {
        WaveConfig config = this.GetCurrentWaveConfig();
        if (config == null) return;

        bool isDurationFinished = this.waveTimer >= config.waveDuration;
        bool areBossesDefeated = this.activeBosses.Count == 0;

        if (config.waveType == WaveType.Boss)
        {
            // Boss Wave ends when all bosses are defeated
            if (areBossesDefeated && (this.waveTimer >= 3f || isDurationFinished))
            {
                this.CompleteWave();
            }
        }
        else
        {
            // Normal / Swarm wave ends when duration finishes and enemy count is low enough
            if (isDurationFinished && this.TotalEnemiesOnScene <= 5)
            {
                this.CompleteWave();
            }
        }
    }

    public virtual void CompleteWave()
    {
        if (!this.isWaveActive) return;

        this.isWaveActive = false;
        this.isBreakTime = true;
        this.breakTimer = 0f;

        Debug.Log($"[WaveSpawnManager] === WAVE {this.CurrentWaveNumber} COMPLETED! Break time: {this.waveBreakDuration}s ===");
        OnWaveCompleted?.Invoke(this.currentWaveIndex);
    }

    #endregion

    #region Enemy Spawning

    protected virtual void HandleEnemySpawning()
    {
        WaveConfig config = this.GetCurrentWaveConfig();
        if (config == null) return;

        // Check max enemies limit on scene
        if (this.TotalEnemiesOnScene >= config.maxEnemiesAlive) return;

        this.spawnTimer += Time.deltaTime;
        float effectiveInterval = Mathf.Max(0.5f, config.spawnInterval * this.spawnIntervalMultiplier);
        if (this.spawnTimer < effectiveInterval) return;

        this.spawnTimer = 0f;

        // Determine how many enemies to spawn this interval
        int meleeToSpawn = UnityEngine.Random.Range(config.minMeleePerSpawn, config.maxMeleePerSpawn + 1);
        int rangedToSpawn = UnityEngine.Random.Range(config.minRangedPerSpawn, config.maxRangedPerSpawn + 1);

        for (int i = 0; i < meleeToSpawn; i++)
        {
            if (this.TotalEnemiesOnScene >= config.maxEnemiesAlive) break;
            this.SpawnSingleMeleeEnemy();
        }

        for (int i = 0; i < rangedToSpawn; i++)
        {
            if (this.TotalEnemiesOnScene >= config.maxEnemiesAlive) break;
            this.SpawnSingleRangedEnemy();
        }
    }

    protected virtual void SpawnSingleMeleeEnemy()
    {
        if (this.meleeEnemy == null || SpawnMeleeEnemy.Instance == null) return;

        Vector3 spawnPos = this.GetRandomSpawnPosition();
        SpawnMeleeEnemy.Instance.ExecuteSpawnPooling(this.meleeEnemy, spawnPos, Quaternion.identity);
        this.numberMeleeEnemyOnScene++;
    }

    protected virtual void SpawnSingleRangedEnemy()
    {
        if (this.rangedEnemy == null || SpawnRangedEnemy.Instance == null) return;

        Vector3 spawnPos = this.GetRandomSpawnPosition();
        SpawnRangedEnemy.Instance.ExecuteSpawnPooling(this.rangedEnemy, spawnPos, Quaternion.identity);
        this.numberRangedEnemyOnScene++;
    }

    #endregion

    #region Swarm Events

    protected virtual void HandleSwarmEvent()
    {
        WaveConfig config = this.GetCurrentWaveConfig();
        if (config == null || !config.hasSwarmEvent || this.hasTriggeredSwarm) return;

        if (this.waveTimer >= config.swarmTriggerTime)
        {
            this.hasTriggeredSwarm = true;
            this.TriggerSwarm(config.swarmMeleeCount, config.swarmRangedCount);
        }
    }

    public virtual void TriggerSwarm(int meleeCount, int rangedCount)
    {
        Debug.LogWarning($"[WaveSpawnManager] !!! SWARM INCOMING: {meleeCount} Melee, {rangedCount} Ranged enemies attacking !!!");
        OnSwarmWarning?.Invoke();

        List<Transform> spawnPoints = this.GetActiveSpawnPoints();
        if (spawnPoints.Count == 0) return;

        for (int i = 0; i < meleeCount; i++)
        {
            Transform sp = spawnPoints[i % spawnPoints.Count];
            Vector3 offset = new Vector3(UnityEngine.Random.Range(-2f, 2f), 0, UnityEngine.Random.Range(-2f, 2f));
            if (this.meleeEnemy != null && SpawnMeleeEnemy.Instance != null)
            {
                SpawnMeleeEnemy.Instance.ExecuteSpawnPooling(this.meleeEnemy, sp.position + offset, Quaternion.identity);
                this.numberMeleeEnemyOnScene++;
            }
        }

        for (int i = 0; i < rangedCount; i++)
        {
            Transform sp = spawnPoints[(i + 2) % spawnPoints.Count];
            Vector3 offset = new Vector3(UnityEngine.Random.Range(-2f, 2f), 0, UnityEngine.Random.Range(-2f, 2f));
            if (this.rangedEnemy != null && SpawnRangedEnemy.Instance != null)
            {
                SpawnRangedEnemy.Instance.ExecuteSpawnPooling(this.rangedEnemy, sp.position + offset, Quaternion.identity);
                this.numberRangedEnemyOnScene++;
            }
        }
    }

    #endregion

    #region Periodic Boss Spawning

    protected virtual void HandlePeriodicBossSpawning()
    {
        if (!this.enablePeriodicBossSpawn || !this.isWaveActive) return;

        this.bossSpawnTimer += Time.deltaTime;
        if (this.bossSpawnTimer >= this.bossSpawnInterval)
        {
            this.bossSpawnTimer = 0f;
            BossType randomBoss = this.GetRandomBossType();
            Debug.Log($"[WaveSpawnManager] (30s Timer) Auto-spawning periodic Boss: {randomBoss}");
            this.SpawnBoss(randomBoss);
        }
    }

    #endregion

    #region Boss Spawning & Tracking

    protected virtual void ExecuteSpawnBossWave(WaveConfig config)
    {
        int count = Mathf.Max(1, config.bossCount);
        for (int i = 0; i < count; i++)
        {
            BossType type = config.bossType;
            if (type == BossType.Random || type == BossType.None)
            {
                type = this.GetRandomBossType();
            }
            this.SpawnBoss(type);
        }
    }

    public virtual GameObject SpawnBoss(BossType bossType)
    {
        GameObject prefab = this.GetBossPrefab(bossType);
        if (prefab == null)
        {
            Debug.LogWarning($"[WaveSpawnManager] Boss prefab for {bossType} is null! Reloading boss references...");
            this.LoadBosses();
            prefab = this.GetBossPrefab(bossType);
            if (prefab == null)
            {
                Debug.LogError($"[WaveSpawnManager] Boss prefab for {bossType} could not be found!");
                return null;
            }
        }

        Vector3 spawnPos = this.GetRandomSpawnPosition(offsetRadius: 4f);
        GameObject bossInstance = null;
        if (SpawnLargeMinion.Instance != null)
        {
            bossInstance = SpawnLargeMinion.Instance.ExecuteSpawnPooling(prefab, spawnPos, Quaternion.identity);
        }

        if (bossInstance == null)
        {
            bossInstance = GameObject.Find(prefab.name);
        }

        if (bossInstance != null && !this.activeBosses.Contains(bossInstance))
        {
            this.activeBosses.Add(bossInstance);
            OnBossSpawned?.Invoke(bossInstance);
            Debug.Log($"[WaveSpawnManager] >>> BOSS SPAWNED: {bossInstance.name} ({bossType}) at {spawnPos} <<<");
        }

        return bossInstance;
    }

    protected virtual GameObject GetBossPrefab(BossType bossType)
    {
        switch (bossType)
        {
            case BossType.Elemental: return this.elemental != null ? this.elemental : this.mummy;
            case BossType.Mummy: return this.mummy != null ? this.mummy : this.elemental;
            case BossType.Alien: return this.alien != null ? this.alien : this.elemental;
            case BossType.Zombie: return this.zombie != null ? this.zombie : this.mummy;
            default: return this.mummy != null ? this.mummy : this.elemental;
        }
    }

    protected virtual BossType GetRandomBossType()
    {
        BossType[] types = new BossType[] { BossType.Elemental, BossType.Mummy, BossType.Alien, BossType.Zombie };
        return types[UnityEngine.Random.Range(0, types.Length)];
    }

    protected virtual void CleanDeadBosses()
    {
        for (int i = this.activeBosses.Count - 1; i >= 0; i--)
        {
            GameObject boss = this.activeBosses[i];
            if (boss == null || !boss.activeInHierarchy)
            {
                Debug.Log($"[WaveSpawnManager] Boss defeated / deactivated: {(boss != null ? boss.name : "null")}");
                if (boss != null) OnBossDefeated?.Invoke(boss);
                this.activeBosses.RemoveAt(i);
            }
            else
            {
                DamageReceiver dr = boss.GetComponentInChildren<DamageReceiver>();
                if (dr != null && dr.GetIsDead())
                {
                    Debug.Log($"[WaveSpawnManager] Boss health reached 0: {boss.name}");
                    OnBossDefeated?.Invoke(boss);
                    this.activeBosses.RemoveAt(i);
                }
            }
        }
    }

    #endregion

    #region Helper & Spawn Position Methods

    public virtual WaveConfig GetCurrentWaveConfig()
    {
        if (this.currentWaveIndex >= 0 && this.currentWaveIndex < this.waveConfigs.Count)
        {
            return this.waveConfigs[this.currentWaveIndex];
        }
        return null;
    }

    protected virtual Vector3 GetRandomSpawnPosition(float offsetRadius = 2f)
    {
        List<Transform> list = this.GetActiveSpawnPoints();
        if (list.Count > 0)
        {
            int randomIndex = UnityEngine.Random.Range(0, list.Count);
            Transform pt = list[randomIndex];
            Vector3 randomOffset = new Vector3(
                UnityEngine.Random.Range(-offsetRadius, offsetRadius),
                0,
                UnityEngine.Random.Range(-offsetRadius, offsetRadius)
            );
            return pt.position + randomOffset;
        }

        // Fallback: spawn relative to player or origin
        Vector3 center = this.playerTransform != null ? this.playerTransform.position : Vector3.zero;
        float angle = UnityEngine.Random.Range(0f, 360f) * Mathf.Deg2Rad;
        float distance = UnityEngine.Random.Range(10f, 15f);
        return center + new Vector3(Mathf.Cos(angle) * distance, 0, Mathf.Sin(angle) * distance);
    }

    protected virtual List<Transform> GetActiveSpawnPoints()
    {
        if (this.managerSpawnPoint != null && this.managerSpawnPoint.ListPointer != null)
        {
            return this.managerSpawnPoint.ListPointer;
        }
        return new List<Transform>();
    }

    public virtual void ReduceNumberMeleeEnemyOnScene()
    {
        this.numberMeleeEnemyOnScene = Mathf.Max(0, this.numberMeleeEnemyOnScene - 1);
    }

    public virtual void ReduceNumberRangedEnemyOnScene()
    {
        this.numberRangedEnemyOnScene = Mathf.Max(0, this.numberRangedEnemyOnScene - 1);
    }

    #endregion

    #region Default Wave Progression & Endless Scaling

    protected virtual void EnsureDefaultWaves()
    {
        if (this.waveConfigs != null && this.waveConfigs.Count > 0) return;

        this.waveConfigs = new List<WaveConfig>
        {
            // Wave 1: Khởi động với bầy quái cận chiến Melee (nhịp chậm hơn)
            new WaveConfig
            {
                waveName = "Wave 1 - Awakening",
                waveType = WaveType.Normal,
                waveDuration = 30f,
                spawnInterval = 4.5f,
                minMeleePerSpawn = 1,
                maxMeleePerSpawn = 2,
                minRangedPerSpawn = 0,
                maxRangedPerSpawn = 0,
                maxEnemiesAlive = 12
            },

            // Wave 2: Xuất hiện quái bắn tỉa Ranged Enemy
            new WaveConfig
            {
                waveName = "Wave 2 - Crossfire",
                waveType = WaveType.Normal,
                waveDuration = 35f,
                spawnInterval = 4.0f,
                minMeleePerSpawn = 1,
                maxMeleePerSpawn = 2,
                minRangedPerSpawn = 1,
                maxRangedPerSpawn = 1,
                maxEnemiesAlive = 16
            },

            // Wave 3: Boss 1 - Mummy Summoner
            new WaveConfig
            {
                waveName = "Wave 3 - Mummy Tomb (BOSS)",
                waveType = WaveType.Boss,
                waveDuration = 60f,
                spawnInterval = 4.5f,
                minMeleePerSpawn = 1,
                maxMeleePerSpawn = 2,
                minRangedPerSpawn = 0,
                maxRangedPerSpawn = 1,
                maxEnemiesAlive = 15,
                bossType = BossType.Mummy,
                bossCount = 1
            },

            // Wave 4: Đột kích Swarm Ambush
            new WaveConfig
            {
                waveName = "Wave 4 - Swarm Ambush",
                waveType = WaveType.Swarm,
                waveDuration = 35f,
                spawnInterval = 3.5f,
                minMeleePerSpawn = 2,
                maxMeleePerSpawn = 3,
                minRangedPerSpawn = 1,
                maxRangedPerSpawn = 2,
                maxEnemiesAlive = 22,
                hasSwarmEvent = true,
                swarmTriggerTime = 12f,
                swarmMeleeCount = 6,
                swarmRangedCount = 3
            },

            // Wave 5: Boss 2 - Alien Blaster
            new WaveConfig
            {
                waveName = "Wave 5 - Alien Invasion (BOSS)",
                waveType = WaveType.Boss,
                waveDuration = 60f,
                spawnInterval = 4.5f,
                minMeleePerSpawn = 1,
                maxMeleePerSpawn = 2,
                minRangedPerSpawn = 1,
                maxRangedPerSpawn = 2,
                maxEnemiesAlive = 18,
                bossType = BossType.Alien,
                bossCount = 1
            },

            // Wave 6: Elite Wave
            new WaveConfig
            {
                waveName = "Wave 6 - Elite Mayhem",
                waveType = WaveType.Elite,
                waveDuration = 40f,
                spawnInterval = 3.8f,
                minMeleePerSpawn = 2,
                maxMeleePerSpawn = 3,
                minRangedPerSpawn = 1,
                maxRangedPerSpawn = 2,
                maxEnemiesAlive = 24,
                hasSwarmEvent = true,
                swarmTriggerTime = 18f,
                swarmMeleeCount = 8,
                swarmRangedCount = 3
            },

            // Wave 7: Boss 3 - Zombie Apocalypse
            new WaveConfig
            {
                waveName = "Wave 7 - Zombie Apocalypse (BOSS)",
                waveType = WaveType.Boss,
                waveDuration = 60f,
                spawnInterval = 4.5f,
                minMeleePerSpawn = 1,
                maxMeleePerSpawn = 3,
                minRangedPerSpawn = 1,
                maxRangedPerSpawn = 2,
                maxEnemiesAlive = 20,
                bossType = BossType.Zombie,
                bossCount = 1
            },

            // Wave 8: Boss 4 - Elemental Titan Golem
            new WaveConfig
            {
                waveName = "Wave 8 - Elemental Titan (BOSS)",
                waveType = WaveType.Boss,
                waveDuration = 70f,
                spawnInterval = 4.5f,
                minMeleePerSpawn = 2,
                maxMeleePerSpawn = 3,
                minRangedPerSpawn = 1,
                maxRangedPerSpawn = 2,
                maxEnemiesAlive = 22,
                bossType = BossType.Elemental,
                bossCount = 1
            },

            // Wave 9: Double Boss Rush (2 Boss ngẫu nhiên)
            new WaveConfig
            {
                waveName = "Wave 9 - Double Boss Rush",
                waveType = WaveType.Boss,
                waveDuration = 80f,
                spawnInterval = 4.0f,
                minMeleePerSpawn = 2,
                maxMeleePerSpawn = 3,
                minRangedPerSpawn = 1,
                maxRangedPerSpawn = 2,
                maxEnemiesAlive = 25,
                bossType = BossType.Random,
                bossCount = 2
            }
        };
    }

    protected virtual WaveConfig GenerateEndlessWaveConfig(int waveIndex)
    {
        int waveNum = waveIndex + 1;
        bool isBossWave = (waveNum % 3 == 0);

        return new WaveConfig
        {
            waveName = isBossWave ? $"Wave {waveNum} - Endless Boss Rampage" : $"Wave {waveNum} - Endless Swarm",
            waveType = isBossWave ? WaveType.Boss : WaveType.Elite,
            waveDuration = Mathf.Clamp(35f + (waveIndex * 3f), 35f, 90f),
            spawnInterval = Mathf.Clamp(4.0f - (waveIndex * 0.05f), 2.2f, 5.0f),
            minMeleePerSpawn = Mathf.Clamp(1 + (waveIndex / 5), 1, 4),
            maxMeleePerSpawn = Mathf.Clamp(2 + (waveIndex / 4), 2, 6),
            minRangedPerSpawn = Mathf.Clamp(1 + (waveIndex / 6), 1, 3),
            maxRangedPerSpawn = Mathf.Clamp(1 + (waveIndex / 5), 1, 4),
            maxEnemiesAlive = Mathf.Clamp(20 + (waveIndex * 2), 20, 40),
            bossType = isBossWave ? BossType.Random : BossType.None,
            bossCount = isBossWave ? Mathf.Clamp(1 + (waveIndex / 6), 1, 3) : 0,
            hasSwarmEvent = true,
            swarmTriggerTime = 15f,
            swarmMeleeCount = Mathf.Clamp(6 + waveIndex, 6, 16),
            swarmRangedCount = Mathf.Clamp(2 + (waveIndex / 2), 2, 8)
        };
    }

    #endregion

    #region Component Loaders

    protected virtual void LoadManagerSpawnPoint()
    {
        if (this.managerSpawnPoint != null) return;
        this.managerSpawnPoint = FindFirstObjectByType<ManagerSpawnPoint>();
        Debug.LogWarning(transform.name + " : LoadManagerSpawnPoint");
    }

    protected virtual void LoadPlayer()
    {
        if (this.playerTransform != null) return;
        GameObject playerObj = GameObject.Find("Player");
        if (playerObj != null)
        {
            this.playerTransform = playerObj.transform;
            Debug.LogWarning(transform.name + " : LoadPlayer");
        }
    }

    protected virtual void LoadMeleeEnemy()
    {
        if (this.meleeEnemy != null) return;
        this.meleeEnemy = GameObject.Find("MeleeEnemy");
        Debug.LogWarning(transform.name + " : LoadMeleeEnemy");
    }

    protected virtual void LoadRangedEnemy()
    {
        if (this.rangedEnemy != null) return;
        this.rangedEnemy = GameObject.Find("RangedEnemy");
        Debug.LogWarning(transform.name + " : LoadRangedEnemy");
    }

    protected virtual void LoadBosses()
    {
        LargeEnemyController[] bosses = FindObjectsByType<LargeEnemyController>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        foreach (var b in bosses)
        {
            string bName = b.gameObject.name.ToLower();
            if (this.mummy == null && bName.Contains("mummy")) this.mummy = b.gameObject;
            if (this.alien == null && bName.Contains("alien")) this.alien = b.gameObject;
            if (this.zombie == null && bName.Contains("zombie")) this.zombie = b.gameObject;
            if (this.elemental == null && bName.Contains("elemental")) this.elemental = b.gameObject;
        }

        if (this.mummy == null)
        {
            this.mummy = GameObject.Find("Mummy");
            if (this.mummy != null) Debug.LogWarning(transform.name + " : LoadMummy");
        }
        if (this.alien == null)
        {
            this.alien = GameObject.Find("Alien");
            if (this.alien != null) Debug.LogWarning(transform.name + " : LoadAlien");
        }
        if (this.zombie == null)
        {
            this.zombie = GameObject.Find("Zombie");
            if (this.zombie != null) Debug.LogWarning(transform.name + " : LoadZombie");
        }
        if (this.elemental == null)
        {
            this.elemental = GameObject.Find("Elemental");
            if (this.elemental != null) Debug.LogWarning(transform.name + " : LoadElemental");
        }
    }

    #endregion

    #region Editor & Debug Helpers

    [ContextMenu("Debug: Next Wave")]
    public void DebugNextWave()
    {
        this.CompleteWave();
    }

    [ContextMenu("Debug: Trigger Swarm Now")]
    public void DebugTriggerSwarm()
    {
        this.TriggerSwarm(8, 4);
    }

    [ContextMenu("Debug: Spawn Mummy")]
    public void DebugSpawnMummy() => this.SpawnBoss(BossType.Mummy);

    [ContextMenu("Debug: Spawn Alien")]
    public void DebugSpawnAlien() => this.SpawnBoss(BossType.Alien);

    [ContextMenu("Debug: Spawn Zombie")]
    public void DebugSpawnZombie() => this.SpawnBoss(BossType.Zombie);

    [ContextMenu("Debug: Spawn Elemental")]
    public void DebugSpawnElemental() => this.SpawnBoss(BossType.Elemental);

    #endregion
}

