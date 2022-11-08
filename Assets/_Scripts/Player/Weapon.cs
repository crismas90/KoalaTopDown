using UnityEngine;

public class Weapon : MonoBehaviour
{
    [Header("Ссылки")]                      // почему-то не отображается
    Player player;
    SpriteRenderer spriteRenderer;
    public WeaponClass weaponClass;         // ссылка на класс оружия
    public Transform firePoint;             // якорь для снарядов
    public Transform pivot;                 // якорь weaponHolder (используется для прицеливания)
    WeaponHolder weaponHolder;              // ссылка на скрипт weaponHolder (для стрельбы)
    //GameObject weaponHolderGO;              // ссылка на объект weaponHolder (для поворота)
    //Vector3 mousePosition;                  // положение мыши

    // Параметры оружия (из класса оружия)
    string weaponName;                      // название оружия
    GameObject bulletPrefab;                // префаб снаряда
    float bulletSpeed;                      // скорость снаряда
    int damage;                             // урон (возможно нужно сделать на снаряде)
    float pushForce;                        // сила толчка (возможно нужно сделать на снаряде)
    [HideInInspector] public float fireRate;                // скорострельность оружия (10 - 0,1 выстрелов в секунду)
    [HideInInspector] public float nextTimeToFire;          // для стрельбы (когда стрелять в след раз)
    float forceBackFire;             // отдача оружия

    // Для флипа оружия
    bool needFlip;                          // нужен флип (для правильного отображения оружия)    
    bool leftFlip;                          // оружие слева
    bool rightFlip = true;                  // оружие справа

    [Header("Эффекты")]
    public Animator flashEffectAnimator;
    public bool singleFlash;
    bool flashEffectActive;

    [Header("Тряска камеры при выстреле")]
    public float cameraAmplitudeShake = 1f;     // амплитуда
    public float cameraTimedeShake = 0.1f;      // длительность


    private void Start()
    {
        player = GameManager.instance.player;
        spriteRenderer = GetComponent<SpriteRenderer>();
        //weaponHolderGO = GetComponentInParent<WeaponHolder>().gameObject;       // находим объект weaponHolder
        weaponHolder = GetComponentInParent<WeaponHolder>();                    // находим скрипт weaponHolder
        weaponName = weaponClass.name;                                          // имя
        bulletPrefab = weaponClass.bullet;                                      // тип снаряда
        bulletSpeed = weaponClass.bulletSpeed;                                  // скорость
        damage = weaponClass.damage;                                            // урон
        pushForce = weaponClass.pushForce;                                      // сила толчка
        fireRate = weaponClass.fireRate;                                        // скорострельность
        forceBackFire = weaponClass.forceBackFire;                              // сила отдачи
        //flashEffect = weaponClass.flashEffect;                                  // эффект вспышки при выстреле (флэш)
    }

    private void Update()
    {
        // Стрельба
        if (weaponHolder.fireStart && Time.time >= nextTimeToFire)                          // если начинаем стрелять и кд готово
        {
            nextTimeToFire = Time.time + 1f / fireRate;                                     // вычисляем кд
            Fire();                                                                         // огонь
            CMCameraShake.Instance.ShakeCamera(cameraAmplitudeShake, cameraTimedeShake);    // тряска камеры

            // Эффект флэш для единичного эффекта (потом все так сделать наверное надо)
            if (singleFlash)
                FlashSingle();
        }

        // Эффект флэш для мультиэффекта
        if (flashEffectAnimator != null && !singleFlash)        // если флэшэффект есть
            Flash();        
    }

    void Flash()
    {
        if (weaponHolder.fireStart && !flashEffectActive)
        {
            flashEffectAnimator.SetBool("Fire", true);
            flashEffectActive = true;
        }
        if (!weaponHolder.fireStart)
        {
            flashEffectAnimator.SetBool("Fire", false);
            flashEffectActive = false;
        }
    }
    void FlashSingle()
    {
        flashEffectAnimator.SetTrigger("Fire");
    }


    private void FixedUpdate()
    {
        // Находим угол для флипа холдера и спрайта игрока
        //mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);                // положение мыши                  
        //Vector3 aimDirection = mousePosition - transform.position;                          // угол между положением мыши и pivot оружия          
        //float aimAngle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;       // находим угол в градусах             
        //Quaternion qua1 = Quaternion.Euler(0, 0, aimAngle);                                 // создаем этот угол в Quaternion
        //weaponHolderGO.transform.rotation = Quaternion.Lerp(weaponHolderGO.transform.rotation, qua1, Time.fixedDeltaTime * 15); // делаем Lerp между weaponHoder и нашим углом

        // Флип оружия и спрайта игрока
        if (Mathf.Abs(weaponHolder.aimAngle) > 90 && rightFlip)
        {
            needFlip = true;
            leftFlip = true;
            rightFlip = false;
        }
        if (Mathf.Abs(weaponHolder.aimAngle) <= 90 && leftFlip)
        {
            needFlip = true;
            rightFlip = true;
            leftFlip = false;
        }
        if (needFlip)
        {
            Flip();
        }

         // Отображение оружия перед или позади игрока
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
        }
        if (rightFlip)
        {
            transform.localScale = new Vector3(transform.localScale.x, 1, transform.localScale.z);             
        }
        needFlip = false;
    }


    public void Fire()
    {
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);              // создаем префаб снаряда с позицией и поворотом якоря
        bullet.GetComponent<Bullet>().damage = damage;                                                      // присваиваем урон снаряду
        bullet.GetComponent<Bullet>().pushForce = pushForce;                                                // присваиваем урон снаряду
        bullet.GetComponent<Rigidbody2D>().AddForce(firePoint.right * bulletSpeed, ForceMode2D.Impulse);    // даём импульс
        //bullet.transform.Rotate(0.0f, 0.0f, -90.0f, Space.Self);                                             // поворачиваем снаряд
        player.ForceBackFire(firePoint.transform.position, forceBackFire);                                  // даём отдачу оружия
    }
}
