using UnityEngine;

public class WaveSpawnManager : LoadMonoBehaviour
{
    [SerializeField] protected int numberMeleeEnemy;
    [SerializeField] protected int numberRangedEnemy;
    [SerializeField] protected int numberCurrentMeleeEnemy;
    [SerializeField] protected int numberCurrentRangedEnemy;
    [SerializeField] protected int numberMeleeEnemyOnScene;
    [SerializeField] protected int numberRangedEnemyOnScene;
    [SerializeField] protected int indexPointerCurrent;
    [SerializeField] protected Transform pointerCurrent;
    [SerializeField] protected GameObject meleeEnemy;
    [SerializeField] protected GameObject rangedEnemy;
    [SerializeField] protected ManagerSpawnPoint managerSpawnPoint;
    protected override void LoadComponent()
    {
        base.LoadComponent();
        this.SetIndexPointerCurrent(-1);
        this.SetNumberMeleeEnemy(5);
        this.SetNumberRangedEnemy(3);
        this.LoadMeleeEnemy();
        this.LoadRangedEnemy();
        this.LoadManagerSppawnPoint();
    }
    protected void LoadManagerSppawnPoint()
    {
        if (this.managerSpawnPoint != null) return;
        this.managerSpawnPoint=FindFirstObjectByType<ManagerSpawnPoint>();
        Debug.LogWarning(transform.name + " : LoadManagerSppawnPoint");
    }
    protected virtual Transform GetPointerCurrent(int indexPointerCurrent)
    {
        return this.managerSpawnPoint.ListPointer[indexPointerCurrent];
    }
    protected virtual void SetIndexPointerCurrent(int indexPointerCurrent)
    {
        this.indexPointerCurrent= indexPointerCurrent;
    }
    protected virtual void SetNumberMeleeEnemy(int numberMeleeEnemy)
    {
        this.numberMeleeEnemy = numberMeleeEnemy;
    }
    protected virtual void SetNumberRangedEnemy(int numberRangedEnemy)
    {
        this.numberRangedEnemy = numberRangedEnemy;
    }
    public virtual void ReduceNumberMeleeEnemyOnScene()
    {
        this.numberMeleeEnemyOnScene--;
    }
    public virtual void ReduceNumberRangedEnemyOnScene()
    {
        this.numberRangedEnemyOnScene--;
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
    protected virtual void RandomNumberMeleeEnemy()
    {
        this.numberCurrentMeleeEnemy = Random.Range(3, this.numberMeleeEnemy);
    }
    protected virtual void RandomNumberRangedEnemy()
    {
        this.numberCurrentRangedEnemy = Random.Range(1, this.numberRangedEnemy);
    }
    private void Update()
    {
        this.SpawnEnemy();
    }
    protected virtual void SpawnEnemy()
    {
        if (this.numberMeleeEnemyOnScene <= 0 && this.numberRangedEnemyOnScene <= 0)
        {
            int indexCurrent = this.indexPointerCurrent + 1;
            if (indexCurrent >= this.managerSpawnPoint.ListPointer.Count - 1) return;
            this.Reborn(indexCurrent);
            Debug.Log("Vo");
        }
        this.ExecuteSpawnMeleeEnemy();
        this.ExecuteSpawnRangedEnemy();
    }
    protected virtual void ExecuteSpawnMeleeEnemy()
    {
        Debug.Log("ExecuteSpawnMeleeEnemy");
        Debug.Log(this.numberCurrentMeleeEnemy);
        if (this.numberCurrentMeleeEnemy <= 0) return;
        SpawnMeleeEnemy.Instance.ExecuteSpawnPooling(this.meleeEnemy, pointerCurrent.position, Quaternion.identity);
        this.numberCurrentMeleeEnemy--;
        Debug.Log(this.numberCurrentMeleeEnemy);
        this.numberMeleeEnemyOnScene++;
        Debug.Log(this.numberMeleeEnemyOnScene);
        Debug.Log("==============================");
    }
    protected virtual void ExecuteSpawnRangedEnemy()
    {
        if (this.numberCurrentRangedEnemy <= 0) return;
        SpawnRangedEnemy.Instance.ExecuteSpawnPooling(this.rangedEnemy, pointerCurrent.position, Quaternion.identity);
        this.numberCurrentRangedEnemy--;
        this.numberRangedEnemyOnScene++;
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
}
