using UnityEngine;

public class SpawnLargeMinion : BasePooling
{
    private static SpawnLargeMinion instance;
    public static SpawnLargeMinion Instance => instance;
    protected override void Awake()
    {
        base.Awake();
        SpawnLargeMinion.instance = this;
    }
}
