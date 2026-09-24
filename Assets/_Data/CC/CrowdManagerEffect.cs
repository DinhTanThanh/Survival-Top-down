using System.Collections;
using UnityEngine;

public class CrowdManagerEffect : LoadMonoBehaviour
{
    [SerializeField] protected CrowdControlType currentCC;
    public bool IsStunned => HasCC(CrowdControlType.Stun);
   
    protected virtual bool HasCC(CrowdControlType cc) => (this.currentCC & cc) != 0;
    public virtual void ApplyCC(CrowdControlType cc, float duration)
    {
        this.currentCC |= cc;
        StartCoroutine(this.RemoveCCAfterTime(cc, duration));
    }
    protected virtual IEnumerator RemoveCCAfterTime(CrowdControlType cc, float duration)
    {
        yield return new WaitForSeconds(duration);
        this.currentCC &= ~cc;  //khi thực hiện xong thì xóa hiệu ứng khống chế đó đi
    }
}
