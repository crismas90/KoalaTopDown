using UnityEngine;

/// <summary>
/// Поворачивает хитбокс (для удара мечом)
/// </summary>

public class HitBoxPivot : MonoBehaviour
{
    public WeaponHolder weaponHolder;
    void Start()
    {
        //weaponHolder = GetComponentInParent<WeaponHolder>();
    }
   
    void Update()
    {
        Quaternion qua1 = Quaternion.Euler(0, 0, weaponHolder.aimAngle);                                // создаем этот угол в Quaternion
        transform.rotation = Quaternion.Lerp(transform.rotation, qua1, Time.fixedDeltaTime * 15);       // делаем Lerp между weaponHoder и нашим углом
    }
}
