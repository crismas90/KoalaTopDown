using UnityEngine;

public class BotAIWeaponRange : MonoBehaviour
{
    [Header("������")]                      // ������-�� �� ������������
    BotAI botAI;
    BotAIRangeWeaponHolder weaponHolderRange;   // ������ �� ������ weaponHolder (��� ��������)
    public WeaponClass weaponClass;         // ������ �� ����� ������
    public Transform firePoint;             // ����� ��� ��������
    public Transform pivot;                 // ����� weaponHolder (������������ ��� ������������)

    [Header("��������� ������")]
    bool rayCastWeapon;                     // ������� ������
    [HideInInspector] public string weaponName;     // �������� ������
    GameObject bulletPrefab;                // ������ �������
    float bulletSpeed;                      // �������� �������
    int damage;                             // ���� (�������� ����� ������� �� �������)
    float pushForce;                        // ���� ������ (�������� ����� ������� �� �������)
    [HideInInspector] public float fireRate;                // ���������������� ������ (10 - 0,1 ��������� � �������)
    [HideInInspector] public float nextTimeToFire;          // ��� �������� (����� �������� � ���� ���)
    float forceBackFire;                    // ������ ������
    float recoilX;                          // ������� ��������
    LayerMask layerRayCast;                 // ���� ��� ���������

/*    // ��� ����� ������
    bool needFlip;                          // ����� ���� (��� ����������� ����������� ������)    
    bool leftFlip;                          // ������ �����
    bool rightFlip = true;                  // ������ ������
*/
    [Header("�������")]
    public Animator flashEffectAnimator;    // ���� ��� ��������
    public bool singleFlash;                // ��������� ����
    bool flashActive;                       // ���� ������� (��� �����������)
    public LineRenderer lineRenderer;       // ����� ��� ������ (������)
    LineRenderer lineRaycast;               // ����� ��� ������ (������)
    public TrailRenderer tracerEffect;      // ������ (���� �� ������������)

    [Header("������ ������ ��� ��������")]
    public float cameraAmplitudeShake = 1f; // ���������
    public float cameraTimedeShake = 0.1f;  // ������������

    void Awake()
    {
        
        //weaponHolderGO = GetComponentInParent<WeaponHolder>().gameObject;       // ������� ������ weaponHolder
        weaponName = weaponClass.weaponName;                                    // ��� ������
        rayCastWeapon = weaponClass.rayCastWeapon;                              // ������� ������
        layerRayCast = weaponClass.layerRayCast;                                       // ���� � ���������
        if (weaponClass.bulletPrefab)
            bulletPrefab = weaponClass.bulletPrefab;                                  // ��� ������� (���� �� ������� ������)
        bulletSpeed = weaponClass.bulletSpeed;                                  // ��������
        damage = weaponClass.damage;                                            // ����
        pushForce = weaponClass.pushForce;                                      // ���� ������
        fireRate = weaponClass.fireRate;                                        // ����������������
        forceBackFire = weaponClass.forceBackFire;                              // ���� ������
        recoilX = weaponClass.recoil;                                           // ������� ��������
        //flashEffect = weaponClass.flashEffect;                                  // ������ ������� ��� �������� (����) 
    }

    private void Start()
    {
        botAI = GetComponentInParent<BotAI>();
        weaponHolderRange = GetComponentInParent<BotAIRangeWeaponHolder>();                    // ������� ������ weaponHolder      
    }

    private void Update()
    {
/*        // ���� ������
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
        }*/

        // ������� ������ ������ (�������� ����� ����������)
        if (!singleFlash && Time.time >= nextTimeToFire + 0.1f)
        {
            flashEffectAnimator.SetBool("Fire", false);
            flashActive = false;
            if (lineRaycast)
                lineRaycast.enabled = false;
        }
    }



    private void FixedUpdate()
    {
        // ��������
        if (!weaponHolderRange.fireStart)                        // ���� �� ������ ��������
        {
            return;                                         // �������
        }

        if (Time.time >= nextTimeToFire)                    // ���� �������� �������� � �� ������
        {
            nextTimeToFire = Time.time + 1f / fireRate;                                     // ��������� ��
            if (!rayCastWeapon)
                FireProjectile();                                                           // ������� �����
            if (rayCastWeapon)
                FireRayCast();                                                              // ������� ���������
            CMCameraShake.Instance.ShakeCamera(cameraAmplitudeShake, cameraTimedeShake);    // ������ ������
            if (flashEffectAnimator != null)                                                // ���� ���������� ����
                Flash();
        }




        // ������� ���� ��� ����� ������� � ������� ������
        //mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);                // ��������� ����                  
        //Vector3 aimDirection = mousePosition - transform.position;                          // ���� ����� ���������� ���� � pivot ������          
        //float aimAngle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;       // ������� ���� � ��������             
        //Quaternion qua1 = Quaternion.Euler(0, 0, aimAngle);                                 // ������� ���� ���� � Quaternion
        //weaponHolderGO.transform.rotation = Quaternion.Lerp(weaponHolderGO.transform.rotation, qua1, Time.fixedDeltaTime * 15); // ������ Lerp ����� weaponHoder � ����� �����



        // ����������� ������ ����� ��� ������ ������
        /*        if (aimAngle > 0)
                {
                    spriteRenderer.sortingOrder = -1;
                }
                else
                {
                    spriteRenderer.sortingOrder = 1;
                }*/
    }

    // ����
    void Flash()
    {
        if (!singleFlash)
        {
            if (!flashActive)
            {
                flashEffectAnimator.SetBool("Fire", true);
                flashActive = true;
            }
        }
        else
        {
            flashEffectAnimator.SetTrigger("Fire");
        }
    }


    // ����
/*    void Flip()
    {
        if (leftFlip)                                                                                   // �������� ������
        {
            transform.localScale = new Vector3(transform.localScale.x, -1, transform.localScale.z);     // ������������ ������ ����� scale
        }
        if (rightFlip)
        {
            transform.localScale = new Vector3(transform.localScale.x, 1, transform.localScale.z);
        }
        needFlip = false;
    }*/


    public void FireProjectile()
    {
        float randomBulletX = Random.Range(-recoilX, recoilX);                                              // ������� ��������
        firePoint.Rotate(0, 0, randomBulletX);                                                              // ���� �������
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);              // ������� ������ ������� � �������� � ��������� �����
        bullet.GetComponent<Bullet>().damage = damage;                                                      // ����������� ���� �������
        bullet.GetComponent<Bullet>().pushForce = pushForce;                                                // ����������� ���� ������ �������
        bullet.GetComponent<Rigidbody2D>().AddForce(firePoint.right * bulletSpeed, ForceMode2D.Impulse);    // ��� �������        
        botAI.ForceBackFire(firePoint.transform.position, forceBackFire);                                   // ��� ������ ������
        firePoint.Rotate(0, 0, -randomBulletX);                                                             // � ���� ���������� �������
        if (botAI.isFriendly)
        {
            bullet.layer = LayerMask.NameToLayer("BulletPlayer");           // ���� ����
            bullet.GetComponent<Bullet>().layerExplousion = LayerMask.GetMask("Enemy", "ObjectsDestroyble", "Default");
        }
    }

    public void FireRayCast()
    {
        // ��������� ��� ��������
        TrailRenderer tracer = Instantiate(tracerEffect, firePoint.position, Quaternion.identity);          // ������� ������
        tracer.AddPosition(firePoint.position);                                                             // ��������� �������
        //tracer.transform.SetParent(transform, true); 

        // �������
        float randomBulletX = Random.Range(-recoilX, recoilX);

        // �������2�
        RaycastHit2D hit = Physics2D.Raycast(firePoint.position, firePoint.right + new Vector3(randomBulletX, 0, 0), Mathf.Infinity, layerRayCast);
        if (hit.collider != null)
        {
            //Debug.Log("Hit!");
            if (hit.collider.TryGetComponent<Fighter>(out Fighter fighter))
            {
                Vector2 vec2 = (fighter.transform.position - botAI.transform.position).normalized;
                fighter.TakeDamage(damage, vec2, pushForce);
            }

            tracer.transform.position = hit.point;                      // �������� ������� ������� ��������             

            /*            if (!lineRaycast)
                        {
                            lineRaycast = Instantiate(lineRenderer, firePoint.position, Quaternion.identity);
                        }
                        lineRaycast.enabled = true;
                        lineRaycast.SetPosition(0, firePoint.position);
                        lineRaycast.SetPosition(1, hit.point);*/

            //Debug.DrawRay(firePoint.position, firePoint.right * 100f, Color.yellow);
        }



        //Debug.Log("Hit");



        /*        if (Physics2D.Raycast(ray, out hit, Mathf.Infinity, layerRayCast))
                {
                    Debug.DrawLine(ray.origin, hit.point, Color.red, 1.0f);                                     // �����, ������� �����

                    //tracer.transform.position = hit.point;                                                    // �������� ������� ������� ���� 

                    if (hit.collider.TryGetComponent<Fighter>(out Fighter fighter))
                    {
                        fighter.TakeDamage(damage);
                        Vector2 vec2 = (player.transform.position - GameManager.instance.player.transform.position).normalized;
                        fighter.rb2D.AddForce(vec2 * pushForce, ForceMode2D.Impulse);
                    }
                }*/
    }
}