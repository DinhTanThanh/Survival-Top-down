using UnityEngine;

public class BaseWeaponController : LoadMonoBehaviour
{
    [SerializeField] protected WeaponData weaponData;
    public WeaponData WeaponData => weaponData;
    protected override void LoadComponent()
    {
        base.LoadComponent();
        this.LoadWeaponData();
    }
    protected virtual void LoadWeaponData()
    {
        if (this.weaponData != null) return;
        this.weaponData = Resources.Load<WeaponData>("WeaponData/" + transform.name + "Data");
        Debug.LogWarning(transform.name + " : LoadWeaponData");
    }
}
