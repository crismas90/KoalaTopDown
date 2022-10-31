using UnityEngine;

public class Weapon : MonoBehaviour
{   
    // Ссылки
    public WeaponClass weaponClass;         // ссылка на класс оружия
    public Transform firePoint;             // якорь для снарядов
    GameObject weaponHolder;                // ссылка на weaponHolder (для поворота)

    Vector3 mousePosition;                  // положение мыши

    // Параметры оружия (из класса оружия)
    string weaponName;                      // название оружия
    GameObject bulletPrefab;                // префаб снаряда
    float bulletSpeed;                      // скорость снаряда
    int damage;                             // урон (возможно нужно сделать на снаряде)
    public float fireRate;                  // скорострельность оружия (10 - 0,1 выстрелов в секунду)
    [HideInInspector]
    public float nextTimeToFire = 0f;       // для стрельбы (когда стрелять в след раз)


    private void Awake()
    {
        
    }

    private void Start()
    {
        weaponName = weaponClass.name;                                          // имя
        bulletPrefab = weaponClass.bullet;                                      // тип снаряда
        bulletSpeed = weaponClass.bulletSpeed;                                  // скорость
        damage = weaponClass.damage;                                            // урон
        fireRate = weaponClass.fireRate;                                        // скорострельность
        //GetComponent<Renderer>().material.color = weaponClass.color;            // цвет
        weaponHolder = GetComponentInParent<WeaponHolder>().gameObject;         // находим weaponHolder
    }

    private void FixedUpdate()
    {
        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);                                                            // положение мыши
        Vector3 aimDirection = mousePosition - firePoint.position;                                                                      // угол между положением мыши и якорем оружия
        float aimAngle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;                                                   // находим угол в градусах         
        Quaternion qua1 = Quaternion.Euler(weaponHolder.transform.eulerAngles.x, weaponHolder.transform.eulerAngles.y, aimAngle);       // создаем этот угол в Quaternion (ориентируемся на weaponHolder)        
        weaponHolder.transform.rotation = Quaternion.Lerp(weaponHolder.transform.rotation, qua1, Time.fixedDeltaTime * 15);             // делаем Lerp между weaponHoder и нашим углом        
    }

    public void Fire()
    {
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);                      // создаем префаб снаряда с позицией и поворотом якоря
        //bullet.layer = 7;                                                                                           // назначаем снаряду слой "BulletPlayer"
        bullet.GetComponent<Bullet>().damage = damage;                                                              // присваиваем урон снаряду
        //bullet.transform.Rotate(0.0f, 0.0f, 0.0f, Space.Self);                                                     // поворачиваем снаряд (для ракеты)
        bullet.GetComponent<Rigidbody2D>().AddForce(firePoint.right * bulletSpeed, ForceMode2D.Impulse);          // даём импульс
    }
}
