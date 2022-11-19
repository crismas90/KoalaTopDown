using UnityEngine;

/// <summary>
/// Аналог weaponHolder и hitboxpivot игрока
/// </summary>

public class EnemyHitBoxPivot : MonoBehaviour
{
    Enemy enemy;    
    [HideInInspector] public float aimAngle;                // угол поворота для вращения холдера с оружием и хитбоксПивота

    void Start()
    {
        enemy = GetComponentInParent<Enemy>();
    }
    
    void Update()
    {
        // Поворот хитбокса
        if (enemy.chasing && enemy.targetVisible)
        {
            Vector3 aimDirection = enemy.target.transform.position - transform.position;                // угол между положением мыши и pivot оружия          
            aimAngle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;                     // находим угол в градусах             
            Quaternion qua1 = Quaternion.Euler(0, 0, aimAngle);                                         // создаем этот угол в Quaternion
            transform.rotation = Quaternion.Lerp(transform.rotation, qua1, Time.fixedDeltaTime * 15);   // делаем Lerp между weaponHoder и нашим углом
        }        
    }
}
