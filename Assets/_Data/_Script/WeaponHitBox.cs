using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHitBox : LoadMonoBehaviour
{
    [SerializeField] protected Collider colliderr;
    [SerializeField] protected Transform enemyRoot;
    [SerializeField] protected MeleeEnemyController meleeEnemyController;
    [SerializeField] protected List<Vector3> listDirection = new List<Vector3>();
    protected override void LoadComponent()
    {
        base.LoadComponent();
        this.LoadCollider();
        this.LoadMeleeEnemyController();
        this.SetEnemyRoot(this.meleeEnemyController.transform);
        this.SetListDirection(new List<Vector3>() { Vector3.right, Vector3.left });
    }
    protected virtual void SetListDirection(List<Vector3> listDirection)
    {
        this.listDirection = listDirection;
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
    }
    public virtual void ExecuteTurnBack()
    {
        if (this.meleeEnemyController.MeleeEnemyAttack.GetIsAttack()) return;
        this.meleeEnemyController.MeleeEnemyMoving.SetForcedRunning(true);

        StartCoroutine(TurnBackRight());
        this.meleeEnemyController.MeleeEnemyMoving.SetSpeedMovement(0f);
        this.meleeEnemyController.MeleeEnemyAttack.SetIsAttack(true);
        Debug.Log("1");
    }
    IEnumerator TurnBackRight()
    {
        int indexRandom = Random.Range(0, this.listDirection.Count);
        Vector3 posDestination = this.enemyRoot.position + this.enemyRoot.rotation*this.listDirection[indexRandom] * 1.3f;
        Vector3 posCurrent = this.enemyRoot.position;
        float ElapsedTime = 0f;
        float DurationTime = 1f;
        while (ElapsedTime <= DurationTime)
        {
            Vector3 nextPos = Vector3.Lerp(posCurrent, posDestination, ElapsedTime / DurationTime);
            this.meleeEnemyController.Rb.MovePosition(nextPos);
            ElapsedTime += Time.fixedDeltaTime;
            yield return null;
        }
        this.meleeEnemyController.Rb.MovePosition(posDestination);
        this.meleeEnemyController.MeleeEnemyMoving.SetForcedRunning(false);
    }
    protected virtual void GoBackListEnemyDead()
    {
        this.transform.parent.gameObject.SetActive(false);
        SpawnMeleeEnemy.Instance.GoBackList(this.transform.parent.gameObject);
    }
}
