using UnityEngine;

public class WaveSpawnManager : LoadMonoBehaviour
{
    [SerializeField] protected int numberMeleeEnemy;
    [SerializeField] protected int numberRangedEnemy;
    [SerializeField] protected GameObject meleeEnemy;
    [SerializeField] protected GameObject rangedEnemy;
    protected override void LoadComponent()
    {
        base.LoadComponent();
        this.SetNumberMeleeEnemy(5);
        this.SetNumberRangedEnemy(3);
        this.LoadMeleeEnemy();
        this.LoadRangedEnemy();
    }
    protected virtual void SetNumberMeleeEnemy(int numberMeleeEnemy)
    {
        this.numberMeleeEnemy = numberMeleeEnemy;
    }
    protected virtual void SetNumberRangedEnemy(int numberRangedEnemy)
    {
        this.numberRangedEnemy = numberRangedEnemy;
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
}
