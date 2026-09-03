using UnityEngine;
using UnityEngine.Rendering;

public class WaveSpawnManager : LoadMonoBehaviour
{
    [SerializeField] protected int numberMeleeEnemy;
    [SerializeField] protected int numberRangedEnemy;
    [SerializeField] protected int numberCurrentMeleeEnemy;
    [SerializeField] protected int numberCurrentRangedEnemy;
    [SerializeField] protected int numberMeleeEnemyOnScene;
    [SerializeField] protected int numberRangedEnemyOnScene;
    [SerializeField] protected int indexPointerCurrent;
    [SerializeField] protected float timer = 10f;
    [SerializeField] protected float timeDelay = 10f;
    [SerializeField] protected bool hasAllowSpawnLargeMinion;
    [SerializeField] protected bool isSpawnTempt;
    [SerializeField] protected Transform pointerCurrent;
    [SerializeField] protected GameObject meleeEnemy;
    [SerializeField] protected GameObject rangedEnemy;
    [SerializeField] protected GameObject elemental;
    [SerializeField] protected ManagerSpawnPoint managerSpawnPoint;

    protected override void LoadComponent()
    {
        base.LoadComponent();
        this.SetIndexPointerCurrent(-1);
        this.SetNumberMeleeEnemy(5);
        this.SetNumberRangedEnemy(3);
        this.LoadMeleeEnemy();
        this.LoadRangedEnemy();
        this.LoadelEmentalMinion();
        this.LoadManagerSppawnPoint();
    }
    private void Update()
    {
        if (isSpawnTempt) return;
        this.SpawnEnemy();
        if (!this.hasAllowSpawnLargeMinion) return;
        ExecuteSpawnLargeMinion();
    }

    public virtual void ReduceNumberMeleeEnemyOnScene()
    {
        this.numberMeleeEnemyOnScene--;
    }
    public virtual void ReduceNumberRangedEnemyOnScene()
    {
        this.numberRangedEnemyOnScene--;
    }

    protected virtual void RandomNumberMeleeEnemy()
    {
        this.numberCurrentMeleeEnemy = Random.Range(3, this.numberMeleeEnemy);
    }
    protected virtual void RandomNumberRangedEnemy()
    {
        this.numberCurrentRangedEnemy = Random.Range(1, this.numberRangedEnemy);
    }

    protected virtual void SpawnEnemy()
    {
        if (this.numberMeleeEnemyOnScene <= 0 && this.numberRangedEnemyOnScene <= 0)
        {
            int indexCurrent = this.indexPointerCurrent + 1;
            if (indexCurrent >= this.managerSpawnPoint.ListPointer.Count)
            {
                this.hasAllowSpawnLargeMinion = true;
                return;
            }
            this.Reborn(indexCurrent);
        }
        this.ExecuteSpawnMeleeEnemy();
        this.ExecuteSpawnRangedEnemy();
    }
    protected virtual void ExecuteSpawnMeleeEnemy()
    {
        if (this.numberCurrentMeleeEnemy <= 0) return;
        Vector3 offset = new Vector3(Random.Range(-2f, 2f), 0, Random.Range(-2f, 2f));
        SpawnMeleeEnemy.Instance.ExecuteSpawnPooling(this.meleeEnemy, pointerCurrent.position + offset, Quaternion.identity);
        this.numberCurrentMeleeEnemy--;
        this.numberMeleeEnemyOnScene++;
    }
    protected virtual void ExecuteSpawnRangedEnemy()
    {
        if (this.numberCurrentRangedEnemy <= 0) return;
        Vector3 offset = new Vector3(Random.Range(-2f, 2f), 0, Random.Range(-2f, 2f));
        SpawnRangedEnemy.Instance.ExecuteSpawnPooling(this.rangedEnemy, pointerCurrent.position + offset, Quaternion.identity);
        this.numberCurrentRangedEnemy--;
        this.numberRangedEnemyOnScene++;
    }
    protected virtual void ExecuteSpawnLargeMinion()
    {
        int index = this.managerSpawnPoint.ListPointer.Count;
        int indexRandom = 0;
        for (int i = 0; i < 4; i++)
        {
            indexRandom = Random.Range(0, index);
            Transform posSpawn = this.managerSpawnPoint.ListPointer[indexRandom];
            float posRandom = Random.Range(-1f, 2f);
            posSpawn.position += new Vector3(posRandom, posRandom, posRandom);
            SpawnLargeMinion.Instance.ExecuteSpawnPooling(this.elemental, posSpawn.position, Quaternion.identity);
        }
        isSpawnTempt = true;
    }
    protected virtual bool Timing()
    {
        this.timer += Time.deltaTime;
        if (this.timer < this.timeDelay) return false;
        this.timer = 0f;
        return true;
    }
    protected virtual void Reborn(int indexPointer)
    {
        this.indexPointerCurrent = indexPointer;
        this.pointerCurrent = this.GetPointerCurrent(this.indexPointerCurrent);
        this.numberMeleeEnemyOnScene = 0;
        this.numberRangedEnemyOnScene = 0;
        this.RandomNumberMeleeEnemy();
        this.RandomNumberRangedEnemy();
    }
    protected virtual Transform GetPointerCurrent(int indexPointerCurrent)
    {
        return this.managerSpawnPoint.ListPointer[indexPointerCurrent];
    }
    protected virtual void SetIndexPointerCurrent(int indexPointerCurrent)
    {
        this.indexPointerCurrent = indexPointerCurrent;
    }
    protected virtual void SetNumberMeleeEnemy(int numberMeleeEnemy)
    {
        this.numberMeleeEnemy = numberMeleeEnemy;
    }
    protected virtual void SetNumberRangedEnemy(int numberRangedEnemy)
    {
        this.numberRangedEnemy = numberRangedEnemy;
    }
    protected virtual void SetTimeDelay(float timeDelay)
    {
        this.timeDelay = timeDelay;
    }
    protected void LoadManagerSppawnPoint()
    {
        if (this.managerSpawnPoint != null) return;
        this.managerSpawnPoint = FindFirstObjectByType<ManagerSpawnPoint>();
        Debug.LogWarning(transform.name + " : LoadManagerSppawnPoint");
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
    protected virtual void LoadelEmentalMinion()
    {
        if (this.elemental != null) return;
        this.elemental = GameObject.Find("Elemental");
        Debug.LogWarning(transform.name + " : LoadElementalMinion");
    }
}
