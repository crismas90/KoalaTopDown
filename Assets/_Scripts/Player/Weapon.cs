using UnityEngine;

public class Weapon : MonoBehaviour
{
    // Ссылки
    Player player;
    SpriteRenderer spriteRenderer;          
    public WeaponClass weaponClass;         // ссылка на класс оружия
    public Transform firePoint;             // якорь для снарядов
    public Transform pivot;                 // якорь weaponHolder
    GameObject weaponHolder;                // ссылка на weaponHolder (для поворота)

    Vector3 mousePosition;                  // положение мыши

    // Параметры оружия (из класса оружия)
    string weaponName;                      // название оружия
    GameObject bulletPrefab;                // префаб снаряда
    float bulletSpeed;                      // скорость снаряда
    int damage;                             // урон (возможно нужно сделать на снаряде)
    [HideInInspector] public float fireRate;                  // скорострельность оружия (10 - 0,1 выстрелов в секунду)
    [HideInInspector] public float nextTimeToFire = 0f;       // для стрельбы (когда стрелять в след раз)

    bool needFlip;                          // нужен флип (для правильного отображения оружия)    
    bool leftFlip;                          // оружие слева
    bool rightFlip = true;                  // оружие справа


    private void Awake()
    {
        
    }

    private void Start()
    {
        player = GameManager.instance.player;
        weaponHolder = GetComponentInParent<WeaponHolder>().gameObject;         // находим weaponHolder
        spriteRenderer = GetComponent<SpriteRenderer>();
        weaponName = weaponClass.name;                                          // имя
        bulletPrefab = weaponClass.bullet;                                      // тип снаряда
        bulletSpeed = weaponClass.bulletSpeed;                                  // скорость
        damage = weaponClass.damage;                                            // урон
        fireRate = weaponClass.fireRate;                                        // скорострельность
        //GetComponent<Renderer>().material.color = weaponClass.color;            // цвет
    }

    private void FixedUpdate()
    {
        // Поворот оружия
        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);                                                            // положение мыши                  
        Vector3 aimDirection = mousePosition - pivot.transform.position;                                                                // угол между положением мыши и weaponHolder (но нужно между firePoint)          
        float aimAngle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;                                                   // находим угол в градусах             
        Quaternion qua1 = Quaternion.Euler(0, 0, aimAngle);                                                                             // создаем этот угол в Quaternion
        weaponHolder.transform.rotation = Quaternion.Lerp(weaponHolder.transform.rotation, qua1, Time.fixedDeltaTime * 15);             // делаем Lerp между weaponHoder и нашим углом

        // Флип оружия
        if (Mathf.Abs(aimAngle) > 90 && rightFlip)
        {
            needFlip = true;
            leftFlip = true;
            rightFlip = false;
        }
        if (Mathf.Abs(aimAngle) <= 90 && leftFlip)
        {
            needFlip = true;
            rightFlip = true;
            leftFlip = false;
        }
        if (needFlip)
        {
            Flip();
        }

        /*        if (aimAngle > 0)
                {
                    spriteRenderer.sortingOrder = -1;
                }
                else
                {
                    spriteRenderer.sortingOrder = 1;
                }*/
    }

    void Flip()
    {
        if (leftFlip)                                                                                   // разворот налево
        {
            transform.localScale = new Vector3(transform.localScale.x, -1, transform.localScale.z);     // поворачиваем оружие через scale
            player.spriteRenderer.flipX = true;                                                         // поворачиваем спрайт игрока
        }
        if (rightFlip)
        {
            transform.localScale = new Vector3(transform.localScale.x, 1, transform.localScale.z);          
            player.spriteRenderer.flipX = false;
        }
        needFlip = false;
    }


    public void Fire()
    {
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);              // создаем префаб снаряда с позицией и поворотом якоря
        bullet.GetComponent<Bullet>().damage = damage;                                                      // присваиваем урон снаряду
        bullet.GetComponent<Rigidbody2D>().AddForce(firePoint.right * bulletSpeed, ForceMode2D.Impulse);    // даём импульс
        //bullet.transform.Rotate(0.0f, 0.0f, 0.0f, Space.Self);                                             // поворачиваем снаряд (для ракеты)
    }
}
