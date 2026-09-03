using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHitBox : LoadMonoBehaviour
{
    [SerializeField] protected Collider colliderr;
    [SerializeField] protected Transform enemyRoot;
    [SerializeField] protected MeleeEnemyController meleeEnemyController;
    protected override void LoadComponent()
    {
        base.LoadComponent();
        this.LoadCollider();
        this.LoadMeleeEnemyController();
        this.SetEnemyRoot(this.meleeEnemyController.transform);
    }
    protected virtual void SetEnemyRoot(Transform enemyRoot)
    {
        if (enemyRoot == null) return;
        this.enemyRoot = enemyRoot;
    }
    protected virtual void LoadMeleeEnemyController()
    {
        if (this.meleeEnemyController != null) return;
        this.meleeEnemyController=GetComponentInParent<MeleeEnemyController>();
        Debug.LogWarning(transform.name + " : LoadMeleeEnemyController");
    }
    protected virtual void LoadCollider()
    {
        if (this.colliderr != null) return;
        this.colliderr = GetComponentInChildren<BoxCollider>();
        Debug.LogWarning(transform.name + " : LoadCollider");
    }
    public virtual void EnableCollider()
    {
        this.colliderr.enabled = true;
    }
    public virtual void DisableCollider()
    {
        this.colliderr.enabled = false;
        this.meleeEnemyController.MeleeEnemyAttack.SetIsAttack(true);
    }
    protected virtual void GoBackListEnemyDead()
    {
        this.transform.parent.gameObject.SetActive(false);
        SpawnMeleeEnemy.Instance.GoBackList(this.transform.parent.gameObject);
    }
}
