using UnityEngine;

public class BotAIWeaponMelee : MonoBehaviour
{
    BotAI botAI;
    BotAIMeleeWeaponHolder weaponHolderMelee;   // ������ �� ������ weaponHolder (��� ��������)
    Animator animator;

    public string weaponName;
    public Transform hitBox;
    public int damage = 10;                             // ����
    public float pushForce = 1;                         // ���� ������
    public float radius = 1;                            // ������
    public float cooldown = 1f;                         // ����������� �����
    float lastAttack;                                   // ����� ���������� ����� (��� ����������� �����)
    [HideInInspector] public LayerMask layerHit;        // ���� ��� ����� (����� �� �����)

    public GameObject bulletPrefab;
    //public bool demon;

    // ����� 
    public TrailRenderer trail;

    void Start()
    {
        botAI = GetComponentInParent<BotAI>();        
        animator = GetComponentInParent<Animator>();
        weaponHolderMelee = GetComponentInParent<BotAIMeleeWeaponHolder>();
        layerHit = botAI.layerHit;
    }


    void Update()
    {
        // �����
        if (!weaponHolderMelee.fireStart)                        // ���� �� ������ ��������
        {
            return;                                         // �������
        }

        if (Time.time - lastAttack > cooldown)          // ���� ������ ��������� � �� ������
        {
            //Debug.Log("Attack!");
            lastAttack = Time.time;                             // ����������� ����� �����
            animator.SetTrigger("Hit");
        }
    }

    public void MeleeAttack()
    {
        Collider2D[] collidersHits = Physics2D.OverlapCircleAll(hitBox.position, radius, layerHit);     // ������� ���� � ������� ������� � ��������
        foreach (Collider2D coll in collidersHits)
        {
            if (coll == null)
            {
                continue;
            }

            if (coll.gameObject.TryGetComponent<Fighter>(out Fighter fighter))
            {
                Vector2 vec2 = (coll.transform.position - botAI.transform.position).normalized;
                fighter.TakeDamage(damage, vec2, pushForce);                
            }
            collidersHits = null;
        }
    }

    public void RangeAttack()
    {
        //float randomBulletX = Random.Range(-recoilX, recoilX);                                              // ������� ��������
        //firePoint.Rotate(0, 0, randomBulletX);                                                              // ���� �������
        GameObject bullet = Instantiate(bulletPrefab, transform.position, transform.rotation);              // ������� ������ ������� � �������� � ��������� �����
        bullet.GetComponent<Bullet>().damage = damage;                                                      // ����������� ���� �������
        bullet.GetComponent<Bullet>().pushForce = pushForce;                                                // ����������� ���� ������ �������
        bullet.GetComponent<Rigidbody2D>().AddForce(transform.right * 5, ForceMode2D.Impulse);    // ��� �������        
        //botAI.ForceBackFire(firePoint.transform.position, forceBackFire);                                   // ��� ������ ������
        //firePoint.Rotate(0, 0, -randomBulletX);                                                             // � ���� ���������� �������
        if (botAI.isFriendly)
        {
            bullet.layer = LayerMask.NameToLayer("BulletPlayer");           // ���� ����
            bullet.GetComponent<Bullet>().layerExplousion = LayerMask.GetMask("Enemy", "ObjectsDestroyble", "Default");
        }
    }


    public void TrailOn(int number)
    {
        if (trail)
        {
            if (number == 1)
                trail.emitting = true;
            if (number == 0)
                trail.emitting = false;
        }

    }

    void OnDrawGizmosSelected()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(hitBox.position, radius);
    }
}
